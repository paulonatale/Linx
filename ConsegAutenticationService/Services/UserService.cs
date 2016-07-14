using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.InMemory;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityServer3.Core.Models;
using System.Threading.Tasks;

namespace ConsegAutenticationService.Services
{
    public class UserService : InMemoryUserService
    {
        IOwinContext ctx;
        ConsegAutenticationService.Context.dbContext _db = new Context.dbContext();

        public UserService(List<InMemoryUser> users, OwinEnvironmentService env) : base(users)
        {
            ctx = new OwinContext(env.Environment);
        }

        public override Task PostAuthenticateAsync(PostAuthenticationContext context)
        {          
            return base.PostAuthenticateAsync(context);
        }

        public override async Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            var form = await ctx.Request.ReadFormAsync();
            var extra = form["extra"];
            await base.AuthenticateLocalAsync(context);
        }


    }
}