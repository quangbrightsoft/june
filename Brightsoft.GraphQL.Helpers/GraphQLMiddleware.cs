using GraphQL;
using GraphQL.Instrumentation;
using GraphQL.Types;
using GraphQL.Validation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GraphQL.NewtonsoftJson;

namespace Brightsoft.GraphQL.Helpers
{
    /// <summary>
    /// Intended to be registered with service.UseMiddleware in target project's Startup to initialize the middleware, acts as a controller for all GraphQL requests.
    /// </summary>
    public class GraphQLMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly GraphQLSettings _settings;
        private readonly IDocumentExecuter _executer;
        private readonly IDocumentWriter _writer;

        public GraphQLMiddleware(
            RequestDelegate next,
            GraphQLSettings settings,
            IDocumentExecuter executer,
            IDocumentWriter writer)
        {
            _next = next;
            _settings = settings;
            _executer = executer;
            _writer = writer;
        }

        /// <summary>
        /// GraphQlMiddleware for catching all requests to graphQL endpoint configured in the target project's Startup.cs
        /// </summary>
        /// <param name="context"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context, ISchema schema)
        {
            if (!IsGraphQLRequest(context))
            {
                await _next(context);
                return;
            }
            await ExecuteAsync(context, schema);
        }

        private bool IsGraphQLRequest(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments(_settings.Path)
                && string.Equals(context.Request.Method, "POST", StringComparison.OrdinalIgnoreCase);
        }

        private async Task ExecuteAsync(HttpContext context, ISchema schema)
        {
            var request = Deserialize<GraphQLRequest>(context.Request.Body);

            var result = await _executer.ExecuteAsync(_ =>
            {
                _.Schema = schema;
                _.Query = request?.Query;
                _.OperationName = request?.OperationName;
                _.Inputs = request?.Variables.ToInputs();
                _.UserContext = _settings.BuildUserContext?.Invoke(context);
                _.ValidationRules = _settings.ValidationRules;
                _.EnableMetrics = _settings.EnableMetrics;
                _.ExposeExceptions = true;
                if (_settings.EnableMetrics)
                {
                    _.FieldMiddleware.Use<InstrumentFieldsMiddleware>();
                }
            });

            await WriteResponseAsync(context, result);
        }

        private async Task WriteResponseAsync(HttpContext context, ExecutionResult result)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = result.Errors?.Any() == true ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.OK;

            await _writer.WriteAsync(context.Response.Body, result);
        }

        public static T Deserialize<T>(Stream s)
        {
            using (var reader = new StreamReader(s))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var ser = new JsonSerializer();
                return ser.Deserialize<T>(jsonReader);
            }
        }
    }
}
