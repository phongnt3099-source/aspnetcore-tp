using Abp.Application.Services.Dto;

namespace ThienPhucDental.Authorization.Users.Dto
{
    public interface IGetLoginAttemptsInput: ISortedResultRequest
    {
        string Filter { get; set; }
    }
}