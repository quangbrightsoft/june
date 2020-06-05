using Brightsoft.GraphQL.Helpers.Data;
using Brightsoft.GraphQL.Helpers.Interfaces;
using GraphQL;
using GraphQL.Instrumentation;
using GraphQL.Types;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using GraphQL.Authorization;
using GraphQL.NewtonsoftJson;
using GraphQL.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Brightsoft.GraphQL.Helpers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQlServices(this IServiceCollection services)
        {
            services.InitGraphQlApi();
            services.RegisterGraphQlModels();
            services.AddGraphQlAuth();
            return services;
        }

        /// <summary>
        /// Registers GraphQl related services, including IDocumentExecuter, IDocumentWriter, InstrumentFieldsMiddleware, RootSchema
        /// </summary>
        /// <param name="services"></param>
        private static void InitGraphQlApi(this IServiceCollection services)
        {
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<IDocumentWriter, DocumentWriter>();

            services.AddSingleton<InstrumentFieldsMiddleware>();
            services.AddSingleton<ISchema, RootSchema>();

            // Needed for JSON request reading, writing
            // https://stackoverflow.com/questions/47735133/asp-net-core-synchronous-operations-are-disallowed-call-writeasync-or-set-all
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
        }

        /// <summary>
        /// Registers GraphQl models for use with ISupportGraphQLModel and AutoRegisteringObjectGraphType<>
        /// </summary>
        /// <param name="services"></param>
        private static void RegisterGraphQlModels(this IServiceCollection services)
        {
            // Registers all classes implementing ISupportGraphQLModel to make AutoRegisteringObjectGraphType usable directly from ISchemaBuilder
            var allModelTypes = TypeHelper.GetAllTypesAssignableFromInCurrentDomain(typeof(ISupportGraphQLModel));
            foreach (System.Type modelType in allModelTypes)
            {
                var genericBase = typeof(AutoRegisteringObjectGraphType<>);
                var combinedType = genericBase.MakeGenericType(modelType);
                dynamic instance = Activator.CreateInstance(combinedType, args: null);

                var attribute = modelType.GetCustomAttribute<GraphQlModelMetadataAttribute>();
                if (attribute != null)
                {
                    instance.Name = attribute.Name;
                    instance.Description = attribute.Description;
                }

                services.AddSingleton(combinedType, a => instance);
            }
        }

        public static void AddGraphQlAuth(this IServiceCollection services)
        {
            services.TryAddSingleton<IAuthorizationEvaluator, AuthorizationEvaluator>();
            services.AddTransient<IValidationRule, AuthorizationValidationRule>();

            services.TryAddSingleton(s =>
            {
                var authSettings = new AuthorizationSettings();

                authSettings.AddPolicy("AdminPolicy", _ => _.RequireClaim("role", "Admin"));

                return authSettings;
            });
        }

        public static void UseGraphQlWithAuth(this IApplicationBuilder app)
        {
            var settings = new GraphQLSettings
            {
                BuildUserContext = ctx =>
               {
                   var userContext = new GraphQLUserContext
                   {
                       User = ctx.User
                   };

                   return userContext;
               }
            };

            //var rules = app.ApplicationServices.GetServices<IValidationRule>();
            //settings.ValidationRules.AddRange(rules);

            app.UseMiddleware<GraphQLMiddleware>(settings);
        }
    }
}
