using Abp.Application.Services.Dto;

namespace ThienPhucDental.Notifications.Dto
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}