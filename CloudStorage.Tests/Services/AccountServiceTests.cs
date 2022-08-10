using Moq;
using Xunit;
using CloudStorage.Tests.Base;
using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;
using CloudStorage.Infrastructure.Services;

namespace CloudStorage.Tests.Services
{
    public class AccountServiceTests
    {
        private readonly AccountService _sut;
        private readonly Mock<IRepository<AccountStorage>> _accountRepoMock;

        public AccountServiceTests()
        {
            _accountRepoMock = new Mock<IRepository<AccountStorage>>();
            _sut = new AccountService(_accountRepoMock.Object);
        }

        [Theory]
        [AutoDomainData]
        public async Task AddFileToStorage_Success(AccountStorage account)
        {
            _accountRepoMock.Setup(x => x.GetByIdAsync(account.UserId))
                .ReturnsAsync(account);
            _accountRepoMock.Setup(x => x.AddAsync(account))
                .ReturnsAsync(account);

            var result = await _sut.AddFileToStorage(account.UserId, account.SizeInUse);
            
            Assert.Equal(account.SizeInUse, result.SizeInUse);
            Assert.Equal(account.UserId, result.UserId);
        }

        [Theory]
        [AutoDomainData]
        public async Task CreateAccountInfo_WhenNotExists(string userId)
        {
            var expected = new AccountStorage(userId);
            _accountRepoMock.Setup(x => x.AddAsync(expected))
                .ReturnsAsync(expected);
            _accountRepoMock.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(It.IsAny<int>);

            var result = await _sut.CreateAccountInfo(userId, null);

            Assert.NotNull(result);
            Assert.Equal(expected.UserId, result.UserId);
        }

        [Theory]
        [AutoDomainData]
        public async Task CreateAccountInfo_WhenExists(AccountStorage account)
        {
            var check = new AccountStorage(account.UserId);
            _accountRepoMock.Setup(x => x.AddAsync(account))
                .ReturnsAsync(account);
            await _accountRepoMock.Object.AddAsync(account);
            
            var result = await _sut.CreateAccountInfo(account.UserId, check);

            Assert.NotEqual(account.SizeInUse, result.SizeInUse);
        }

        [Theory]
        [AutoDomainData]
        public async Task RemoveFileFromStorage_NegativeSize(AccountStorage account)
        {
            long size = account.SizeInUse + 100;
            _accountRepoMock.Setup(x => x.AddAsync(account))
                .ReturnsAsync(account);
            _accountRepoMock.Setup(x => x.GetByIdAsync(account.UserId))
                .ReturnsAsync(account);
            await _accountRepoMock.Object.AddAsync(account);
            
            var result = await _sut.RemoveFileFromStorage(account.UserId, size);
            
            Assert.Equal(account.SizeInUse, result.SizeInUse);
        }

        [Theory]
        [AutoDomainData]
        public async Task RemoveFileFromStorage_Success(AccountStorage account)
        {
            long size = account.SizeInUse;
            long expectedSize = account.SizeInUse - size;
            _accountRepoMock.Setup(x => x.AddAsync(account))
                .ReturnsAsync(account);
            _accountRepoMock.Setup(x => x.GetByIdAsync(account.UserId))
                .ReturnsAsync(account);
            await _accountRepoMock.Object.AddAsync(account);

            var result = await _sut.RemoveFileFromStorage(account.UserId, size);
            
            Assert.Equal(expectedSize, result.SizeInUse);
        }
    }
}
