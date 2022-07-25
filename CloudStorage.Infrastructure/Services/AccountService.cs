using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;
using CloudStorage.Infrastructure.Data;

namespace CloudStorage.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly AuthDbContext _dbContext;
        public AccountService(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddFileToStorage(Guid userId, long size)
        {
            var accountInfo = _dbContext.AccountStorages.Find(userId);
            accountInfo = CheckForNull(userId, accountInfo);
            accountInfo.AddFile(size);

            _dbContext.Update(accountInfo);
            _dbContext.SaveChanges();
        }

        public void RemoveFileFromStorage(Guid userId, long size)
        {
            var accountInfo = _dbContext.AccountStorages.Find(userId)!;
            accountInfo.RemoveFile(size);

            _dbContext.Update(accountInfo);
            _dbContext.SaveChanges();
        }

        private AccountStorage CheckForNull(Guid userId, AccountStorage? account)
        {
            if(account == null)
            {
                return CreateAccountInfo(userId);
            }

            return account;
        }

        private AccountStorage CreateAccountInfo(Guid userId)
        {
            var accountInfo = new AccountStorage(userId);

            _dbContext.AccountStorages.Add(accountInfo);
            _dbContext.SaveChanges();

            return accountInfo;
        }
    }
}
