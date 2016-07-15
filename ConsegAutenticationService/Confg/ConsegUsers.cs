using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace ConsegAutenticationService.Confg
{
    public class ConsegUsers
    {
        public static List<InMemoryUser> Get()
        {
            ConsegAutenticationService.Context.dbContext db = new ConsegAutenticationService.Context.dbContext();

            var resultUser = db._User.ToList();

            List<InMemoryUser> InUser = new List<InMemoryUser>();

            foreach (var item in resultUser)
            {
                List<Repository.entidades.RolesAccess> roles = db._RolesAccess.Where(d => d.IdUser == item.Id).ToList();
                List<Repository.entidades.UserClaimsConseg> claimsUser = db._UserClaimsConseg.Where(d => d.IdUser == item.Id).ToList();

                string Name = claimsUser.Where(d=>d.IdUser == item.Id).Select(d => d.Name).SingleOrDefault().ToString();

                var admin = roles.Count(d => d.IdUser == item.Id && d.Value == "Admin") == 0 ? "Nao" : "Admin";
                var sivic = roles.Count(d => d.IdUser == item.Id && d.Value == "Sivic") == 0 ? "Nao" : "Sivic";
                var Area = roles.Count(d => d.IdUser == item.Id && d.Value == "Area") == 0 ? "Nao" : "Area";

                var InUserMemory = new InMemoryUser
                {
                    Username = item.Username,
                    Password = item.Password,
                    Subject = item.Subject,                   
                    Claims = new Claim[]
                    {
                        new Claim(Constants.ClaimTypes.Name, claimsUser.Select(d=>d.Name).SingleOrDefault().ToString()),
                        new Claim(Constants.ClaimTypes.GivenName, claimsUser.Select(d=>d.GivenName).SingleOrDefault().ToString()),
                        new Claim(Constants.ClaimTypes.FamilyName, claimsUser.Select(d=>d.FamilyName).SingleOrDefault().ToString()),
                        new Claim(Constants.ClaimTypes.Secret, claimsUser.Select(d=>d.Secret).SingleOrDefault().ToString().Sha256()),
                        new Claim(Constants.ClaimTypes.Email, claimsUser.Select(d=>d.Email).SingleOrDefault().ToString()),
                        new Claim(Constants.ClaimTypes.EmailVerified, claimsUser.Select(d=>d.EmailVerified).SingleOrDefault().ToString(),ClaimValueTypes.Boolean),
                        new Claim(Constants.ClaimTypes.Role,admin),
                        new Claim(Constants.ClaimTypes.Role,sivic),
                        new Claim(Constants.ClaimTypes.Role,Area)
                    }

                };
                InUser.Add(InUserMemory);  
            }
            return InUser;
         }
    }
}