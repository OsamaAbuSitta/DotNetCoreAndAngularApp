using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Api.Helper
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, 
        IExceptionHandlerFeature error)
        {
            response.Headers.Add("Application-Error",error.Error.Message);
            response.Headers.Add("Access-Control-Expose-Headers","Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin","*");
        }
    }
}