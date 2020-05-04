using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4.Test;
using System.Security.Claims;
using IdentityServer4;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using CentralErros.Models;

namespace CentralErros.ConfigStartup
{
    public static class IdentityConfig
    {
         // Recursos : OpenId
        public static IEnumerable<IdentityResource> GetRecursosIdentity()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId()              
            };
        }

        // config recursos api
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>() { 
                new ApiResource(
                    name: "codenation", 
                    displayName: "Codenation Class", 
                    claimTypes: new [] { 
                        ClaimTypes.Role, 
                        ClaimTypes.Email 
                    }
                )
            };            
        }

        // config client 
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>() { 
                new Client
                {
                    ClientName = "Client App 1",
                    ClientId = "codenation.api_client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = {
                        new Secret("codenation.api_secret".Sha256())
                    },
                    AllowedScopes = {  
                        IdentityServerConstants.StandardScopes.OpenId,
                        "codenation"
                    },
                    AlwaysIncludeUserClaimsInIdToken = true
                }
            };
        }

        public static List<TestUser> GetUsers()
        {            
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "jaque",
                    Password = "123456",
                    Claims = new [] { 
                        new Claim(ClaimTypes.Role, "admin"), 
                        new Claim(ClaimTypes.Email, "jaque@mail.com")
                    }
                },
            };
        }
    }
}

