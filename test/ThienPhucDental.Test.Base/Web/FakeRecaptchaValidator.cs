using System.Threading.Tasks;
using ThienPhucDental.Security.Recaptcha;

namespace ThienPhucDental.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
