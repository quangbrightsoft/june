using Brightsoft.GraphQL.Helpers.Data;
using Brightsoft.GraphQL.Helpers.Interfaces;
using Brightsoft.JuneApp.Models;
using Brightsoft.JuneApp.Services;
using GraphQL;
using GraphQL.Authorization;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Brightsoft.JuneApp.GraphQl.QueryBuilders
{
    public class AccountQueryBuilder : ISchemaBuilder
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountQueryBuilder(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public void BuildMutation(MutationRoot mutationRoot)
        {
            mutationRoot.Field<AutoRegisteringObjectGraphType<RefreshTokenResultModel>>(
                "refreshToken",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "authenticationToken" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "refreshToken" }),
                resolve: context =>
                {
                    var accountService = _contextAccessor.HttpContext.RequestServices.GetRequiredService<IAccountService>();

                    return accountService.RefreshToken(context.GetArgument<string>("authenticationToken"),
                        context.GetArgument<string>("refreshToken"));
                }
            );

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


            mutationRoot.Field<AutoRegisteringObjectGraphType<UserModel>>(
                "createUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "email" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "fullName" },
                    new QueryArgument<NonNullGraphType<ListGraphType<StringGraphType>>> { Name = "roles" }),
                resolve: context =>
                {
                    var accountService = _contextAccessor.HttpContext.RequestServices.GetRequiredService<IUserService>();

                    return accountService.CreateUserAsync(new CreateUserModel
                    {
                        Roles = context.GetArgument<string[]>("roles"),
                        FullName = context.GetArgument<string>("fullName"),
                        Email = context.GetArgument<string>("email"),
                    });
                }
            ).AuthorizeWith("AdminPolicy");


            mutationRoot.Field<BooleanGraphType>(
                "editUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "fullName" },
                    new QueryArgument<NonNullGraphType<ListGraphType<StringGraphType>>> { Name = "roles" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "email" }),
                resolve: context =>
                {
                    var accountService = _contextAccessor.HttpContext.RequestServices.GetRequiredService<IUserService>();

                    return accountService.UpdateUserAsync(context.GetArgument<int>("id"), new CreateUserModel
                    {
                        Roles = context.GetArgument<string[]>("roles"),
                        FullName = context.GetArgument<string>("fullName"),
                        Email = context.GetArgument<string>("email"),
                    });
                }
            ).AuthorizeWith("AdminPolicy");
        }

        public void BuildQuery(QueryRoot queryRoot)
        {
            queryRoot.Field<ListGraphType<AutoRegisteringObjectGraphType<UserModel>>>(
                "users",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "sortBy" },
                    new QueryArgument<BooleanGraphType> { Name = "descending" },
                    new QueryArgument<IntGraphType> { Name = "first" },
                    new QueryArgument<IntGraphType> { Name = "offset" }),
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
            queryRoot.Field<AutoRegisteringObjectGraphType<UserModel>>(
                "user",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "id" }),
                resolve: context =>
                {
                    var userService = _contextAccessor.HttpContext.RequestServices.GetRequiredService<IUserService>();
                    //TODO pagination needed
                    return userService.GetUserAsync(int.Parse(context.GetArgument<string>("id"))).Result;
                }
            ).AuthorizeWith("AdminPolicy");
        }
    }
}
