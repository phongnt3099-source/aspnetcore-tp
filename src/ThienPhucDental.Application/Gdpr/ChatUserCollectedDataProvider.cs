using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using Microsoft.EntityFrameworkCore;
using ThienPhucDental.Chat;
using ThienPhucDental.Dto;
using ThienPhucDental.EntityFrameworkCore;
using ThienPhucDental.MultiTenancy;

namespace ThienPhucDental.Gdpr
{
    public class ChatUserCollectedDataProvider : ITransientDependency
    {
        private readonly IRepository<ChatMessage, long> _chatMessageRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<UserAccount, long> _userAccountRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IObjectMapper _objectMapper;

        public ChatUserCollectedDataProvider(
            IRepository<ChatMessage, long> chatMessageRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<UserAccount, long> userAccountRepository,
            IRepository<Tenant> tenantRepository,
            IObjectMapper objectMapper)
        {
            _chatMessageRepository = chatMessageRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _userAccountRepository = userAccountRepository;
            _tenantRepository = tenantRepository;
            _objectMapper = objectMapper;
        }

        private Dictionary<UserIdentifier, string> GetFriendUsernames(List<UserIdentifier> users)
        {
            var predicate = PredicateBuilder.New<UserAccount>();

            foreach (var user in users)
            {
                predicate = predicate.Or(ua => ua.TenantId == user.TenantId && ua.UserId == user.UserId);
            }

            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var userList = _userAccountRepository.GetAllList(predicate).Select(ua => new
                {
                    ua.TenantId,
                    ua.UserId,
                    ua.UserName
                }).Distinct();

                return userList.ToDictionary(ua => new UserIdentifier(ua.TenantId, ua.UserId), ua => ua.UserName);
            }
        }

    }
}