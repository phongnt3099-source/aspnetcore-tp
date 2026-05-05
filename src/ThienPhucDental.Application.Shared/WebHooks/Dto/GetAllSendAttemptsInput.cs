using ThienPhucDental.Dto;

namespace ThienPhucDental.WebHooks.Dto
{
    public class GetAllSendAttemptsInput : PagedInputDto
    {
        public string SubscriptionId { get; set; }
    }
}
