using System;
using Brightsoft.Data.Entities;
using Brightsoft.GraphQL.Helpers;
using Brightsoft.GraphQL.Helpers.Data;
using Brightsoft.GraphQL.Helpers.Interfaces;
using GraphQL;
using GraphQL.Server.Authorization.AspNetCore;
using GraphQL.Types;
using GraphQL.Utilities;
using JuneApp.Models;
using JuneApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace JuneApp.GraphQl.QueryBuilders
{
    public class AccountQueryBuilder : ISchemaBuilder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountQueryBuilder(IServiceProvider serviceProvider, IHttpContextAccessor contextAccessor)
        {
            _serviceProvider = serviceProvider;
            _contextAccessor = contextAccessor;
        }

        public void BuildMutation(MutationRoot mutationRoot)
        {
            mutationRoot.Field<AutoRegisteringObjectGraphType<LoginResultModel>>(
                "login",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "username" }, new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "password" }),
                resolve: context =>
               {
                   var accountService = _contextAccessor.HttpContext.RequestServices.GetRequiredService<IAccountService>();

                   return accountService.LoginAsync(new LoginModel
                   {
                       UserName = context.GetArgument<string>("username"),
                       Password = context.GetArgument<string>("password")
                   });
               }
            );

            mutationRoot.Field<AutoRegisteringObjectGraphType<LoginResultModel>>(
                "register",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "email" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "username" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "password" }),
                resolve: context =>
               {
                   var accountService = _contextAccessor.HttpContext.RequestServices.GetRequiredService<IAccountService>();

                   return accountService.RegisterAsync(new RegisterModel
                   {
                       UserName = context.GetArgument<string>("username"),
                       Password = context.GetArgument<string>("password"),
                       Email = context.GetArgument<string>("email")
                   });
               }
            );
        }

        public void BuildQuery(QueryRoot queryRoot)
        {
            queryRoot.Field<ListGraphType<AutoRegisteringObjectGraphType<UserModel>>>(
                "users",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "sortBy" },
                    new QueryArgument<BooleanGraphType> { Name = "descending" }),
                resolve: context =>
                {
                    var userService = _contextAccessor.HttpContext.RequestServices.GetRequiredService<IUserService>();
                    //TODO pagination needed
                    return userService.GetUsersAsync(new GetUsersModel
                    {
                        SortBy = context.GetArgument<string>("sortBy"),
                        Descending = context.GetArgument<bool>("descending")
                    }).Result.Items;
                }
            ).AuthorizeWith("AdminPolicy");
        }
    }
}