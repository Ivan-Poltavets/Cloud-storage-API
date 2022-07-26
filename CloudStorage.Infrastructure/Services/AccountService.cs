using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;
using CloudStorage.Infrastructure.Data;

namespace CloudStorage.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<AccountStorage> _repository;

        public AccountService(IRepository<AccountStorage> repository)
        {
            _repository = repository;
        }

        public async void AddFileToStorage(Guid userId, long size)
        {
            var accountInfo = _repository.GetById(userId);
            accountInfo = await CheckForExist(userId, accountInfo);
            accountInfo.AddFile(size);

            _repository.Update(accountInfo);
        }

        public void RemoveFileFromStorage(Guid userId, long size)
        {
            var accountInfo = _repository.GetById(userId);
            accountInfo.RemoveFile(size);

            _repository.Update(accountInfo);
        }

        private async Task<AccountStorage> CheckForExist(Guid userId, AccountStorage? account)
        {
            if(account == null)
            {
                return await CreateAccountInfo(userId);
            }

            return account;
        }

        private async Task<AccountStorage> CreateAccountInfo(Guid userId)
        {
            var accountInfo = new AccountStorage(userId);
            await _repository.AddAsync(accountInfo);

            return accountInfo;
        }
    }
}
