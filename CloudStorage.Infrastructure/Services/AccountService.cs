using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;

namespace CloudStorage.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<AccountStorage> _repository;

        public AccountService(IRepository<AccountStorage> repository)
        {
            _repository = repository;
        }

        public async Task AddFileToStorage(string userId, long size)
        {
            var accountInfo = await _repository.GetByIdAsync(userId);
            accountInfo = await CheckForExist(userId, accountInfo);
            accountInfo.AddFile(size);

            await _repository.UpdateAsync(accountInfo);
            await _repository.SaveChangesAsync();
        }

        public async Task RemoveFileFromStorage(string userId, long size)
        {
            var accountInfo = await _repository.GetByIdAsync(userId);
            accountInfo.RemoveFile(size);

            await _repository.UpdateAsync(accountInfo);
            await _repository.SaveChangesAsync();
        }

        private async Task<AccountStorage> CheckForExist(string userId, AccountStorage? account)
        {
            if(account == null)
            {
                return await CreateAccountInfo(userId);
            }

            return account;
        }

        private async Task<AccountStorage> CreateAccountInfo(string userId)
        {
            var accountInfo = new AccountStorage(userId);

            await _repository.AddAsync(accountInfo);
            await _repository.SaveChangesAsync();

            return accountInfo;
        }
    }
}
