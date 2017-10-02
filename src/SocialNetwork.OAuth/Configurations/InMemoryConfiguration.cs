using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialNetwork.OAuth.Configurations
{
    public class InMemoryConfiguration
    {
        public static IEnumerable<ApiResource> ApiResources => new[]
            {
                new ApiResource("socialnetwork", "Social Network")
                {
                    UserClaims = new [] { "email"}
                }
            };


        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
        {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };

        public static IEnumerable<Client> Clients => new[]
            {
                new Client
                {
                    ClientId = "socialnetwork",
                    ClientSecrets = new [] { new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new [] { "socialnetwork"}
                },
                new Client
                {
                    ClientId = "socialnetwork_implicit",
                    ClientSecrets = new [] { new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = new [] {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "socialnetwork"
                    },
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new [] { "http://localhost:51893/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:51893/signout-callback-oidc" }
                },
                new Client
                {
                    ClientId = "socialnetwork_code",
                    ClientSecrets = new [] { new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowedScopes = new [] {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "socialnetwork"
                    },
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new [] { "http://localhost:51893/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:51893/signout-callback-oidc" }
                }
            };

        public static IEnumerable<TestUser> Users => new[]
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "mail@wright.com",
                    Password = "password",
                    Claims = new [] { new Claim("email", "mail@wright.com") }
                }
            };
    }
}
