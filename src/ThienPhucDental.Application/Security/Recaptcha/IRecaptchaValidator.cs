using System.Threading.Tasks;

namespace ThienPhucDental.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}