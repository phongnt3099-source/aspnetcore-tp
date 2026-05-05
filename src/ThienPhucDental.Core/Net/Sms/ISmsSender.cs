using System.Threading.Tasks;

namespace ThienPhucDental.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}