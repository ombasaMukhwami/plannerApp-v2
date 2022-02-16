using System.Threading.Tasks;
using PlannerApp.Shared.Constants;
using System;
using System.Net.Http;
using System.Threading;
using Blazored.LocalStorage;

namespace PlannerApp
{
    public class AuthorizationMessageHandle:DelegatingHandler
    {
        private readonly ILocalStorageService _storage;

        public AuthorizationMessageHandle(ILocalStorageService storage)
        {
            _storage = storage;
        }
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            if(await _storage.ContainKeyAsync(PlannerVariables.ACCESS_TOKEN))
            {
                var token = await _storage.GetItemAsStringAsync(PlannerVariables.ACCESS_TOKEN);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(PlannerVariables.BEARER, token);
            }

            Console.WriteLine("AUTHORIZATIONMESSAGE HANDLER CALLED");

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
