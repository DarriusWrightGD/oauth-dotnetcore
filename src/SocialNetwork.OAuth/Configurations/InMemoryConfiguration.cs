using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.OAuth.Configurations
{
    public class InMemoryConfiguration
    {
        public static IEnumerable<ApiResource> ApiResources => new[]
            {
                new ApiResource("socialnetwork", "Social Network")
            };

        public static IEnumerable<Client> Clients => new[]
            {
                new Client
                {
                    ClientId = "socialnetwork",
                    ClientSecrets = new [] { new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new [] { "socialnetwork"}
                }
            };

        public static IEnumerable<TestUser> Users => new[]
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "mail@wright.com",
                    Password = "password"
                }
            };
    }
}
