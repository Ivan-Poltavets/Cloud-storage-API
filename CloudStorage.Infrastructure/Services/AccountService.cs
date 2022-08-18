using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;

namespace CloudStorage.Infrastructure.Services;

public class AccountService : IAccountService
{
    private readonly IRepository<AccountExtension> _repository;

    public AccountService(IRepository<AccountExtension> repository)
        => _repository = repository;

    public async Task<AccountExtension> AddFileToStorageAsync(string userId, long size)
    {
        var account = await _repository.GetByIdAsync(userId);
        account = await CreateAccountInfoAsync(userId, account);
        account.AddFile(size);

        await _repository.UpdateAsync(account);
        await _repository.SaveChangesAsync();

        return account;
    }

    public async Task<AccountExtension> RemoveFileFromStorageAsync(string userId, long size)
    {
        var account = await _repository.GetByIdAsync(userId);
        account.RemoveFile(size);

        await _repository.UpdateAsync(account);
        await _repository.SaveChangesAsync();
        
        return account;
    }

    public async Task<AccountExtension> CreateAccountInfoAsync(string userId, AccountExtension? account)
    {
        if(account is null)
        {
            account = new AccountExtension(userId);
            await _repository.AddAsync(account);
            await _repository.SaveChangesAsync();
        }

        return account;
    }
}
