using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ThienPhucDental.Web.Authentication.JwtBearer
{
    public class AsyncJwtBearerOptions : JwtBearerOptions
    {
        public readonly List<IAsyncSecurityTokenValidator> AsyncSecurityTokenValidators;
        
        private readonly ThienPhucDentalAsyncJwtSecurityTokenHandler _defaultAsyncHandler = new ThienPhucDentalAsyncJwtSecurityTokenHandler();

        public AsyncJwtBearerOptions()
        {
            AsyncSecurityTokenValidators = new List<IAsyncSecurityTokenValidator>() {_defaultAsyncHandler};
        }
    }

}
