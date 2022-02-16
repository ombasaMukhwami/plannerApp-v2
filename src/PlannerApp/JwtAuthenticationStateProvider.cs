using System.Threading.Tasks;
using PlannerApp.Shared.Constants;
using Blazored.LocalStorage;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace PlannerApp
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _storage;

        public JwtAuthenticationStateProvider(ILocalStorageService storage)
        {
            _storage = storage;
        }
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (await _storage.ContainKeyAsync(PlannerVariables.ACCESS_TOKEN))
            {
                //user is logged in

                var tokenString = await _storage.GetItemAsStringAsync(PlannerVariables.ACCESS_TOKEN);
                var tokeHandler = new JwtSecurityTokenHandler();
                var token = tokeHandler.ReadJwtToken(tokenString);
                var identity = new ClaimsIdentity(token.Claims, PlannerVariables.BEARER);
                var user = new ClaimsPrincipal(identity);
                var authState = new AuthenticationState(user);

                NotifyAuthenticationStateChanged(Task.FromResult(authState));

                return authState;
            }

            return new AuthenticationState(new ClaimsPrincipal()); //Empty claims principal means no identity and the user is not logged in
        }
    }
}
