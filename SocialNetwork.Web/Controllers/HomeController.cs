using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;
using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;

namespace SocialNetwork.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public async Task Logout()
        {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            await HttpContext.Authentication.SignOutAsync("oidc");
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public async Task<ActionResult> Shouts()
        {
            //In real world applications you would look for the unauthorized exception and if you get that exception
            // then you would attempt to refresh the token, if that fails then notifiy the user that they will have to relog into the auth server

            await RefreshTokens();

            var token = await HttpContext.Authentication.GetTokenAsync("access_token");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                var shoutsResponse = await client.GetAsync("http://localhost:62486/api/shouts");
                var shoutsContent = await shoutsResponse.Content.ReadAsStringAsync();
                ViewData["Shouts"] = shoutsContent;

                return View();
            }
        }

        private async Task RefreshTokens()
        {
            var authServerInformation = await DiscoveryClient.GetAsync("http://localhost:52855");

            var client = new TokenClient(authServerInformation.TokenEndpoint, "socialnetwork_code", "secret");

            var refreshToken = await HttpContext.Authentication.GetTokenAsync("refresh_token");

            var tokenResponse = await client.RequestRefreshTokenAsync(refreshToken);

            var identityToken = await HttpContext.Authentication.GetTokenAsync("id_token");


            var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResponse.ExpiresIn);

            var tokens = new[]
            {
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.IdToken,
                    Value = identityToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = tokenResponse.AccessToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = tokenResponse.RefreshToken
                },
                new AuthenticationToken
                {
                    Name = "expires_at",
                    Value = expiresAt.ToString("o", CultureInfo.InvariantCulture)
                }
            };

            var authInfomation = await HttpContext.Authentication.GetAuthenticateInfoAsync("Cookies");

            authInfomation.Properties.StoreTokens(tokens);

            await HttpContext.Authentication.SignInAsync("Cookies",
                authInfomation.Principal, authInfomation.Properties);

        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
