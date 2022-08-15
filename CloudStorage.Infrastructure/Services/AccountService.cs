using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;

namespace CloudStorage.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<AccountStorage> _repository;

        public AccountService(IRepository<AccountStorage> repository)
            => _repository = repository;

        public async Task<AccountStorage> AddFileToStorage(string userId, long size)
        {
            var accountInfo = await _repository.GetByIdAsync(userId);
            accountInfo = await CreateAccountInfo(userId, accountInfo);
            accountInfo.AddFile(size);

            await _repository.UpdateAsync(accountInfo);
            await _repository.SaveChangesAsync();

            return accountInfo;
        }

        public async Task<AccountStorage> RemoveFileFromStorage(string userId, long size)
        {
            var accountInfo = await _repository.GetByIdAsync(userId);
            accountInfo.RemoveFile(size);

            await _repository.UpdateAsync(accountInfo);
            await _repository.SaveChangesAsync();
            
            return accountInfo;
        }

        public async Task<AccountStorage> CreateAccountInfo(string userId, AccountStorage? account)
        {
            if(account is null)
            {
                account = new AccountStorage(userId);
                await _repository.AddAsync(account);
                await _repository.SaveChangesAsync();
            }

            return account;
        }
    }
}
