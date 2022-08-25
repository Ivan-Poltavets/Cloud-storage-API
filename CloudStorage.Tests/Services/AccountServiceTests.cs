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
        private readonly Mock<IMongoRepository<AccountExtension>> _accountRepoMock;

        public AccountServiceTests()
        {
            _accountRepoMock = new Mock<IMongoRepository<AccountExtension>>();
            _sut = new AccountService(_accountRepoMock.Object);
        }

        [Theory]
        [AutoDomainData]
        public async Task AddFileToStorage_Success(AccountExtension account)
        {
            _accountRepoMock.Setup(x => x.GetByIdAsync(account.UserId))
                .ReturnsAsync(account);

            var result = await _sut.AddFileToStorageAsync(account.UserId, account.SizeInUse);
            
            Assert.Equal(account.SizeInUse, result.SizeInUse);
            Assert.Equal(account.UserId, result.UserId);
        }

        [Theory]
        [AutoDomainData]
        public async Task CreateAccountInfo_WhenNotExists(string userId)
        {
            var expected = new AccountExtension(userId);

            var result = await _sut.CreateAccountInfoAsync(userId, null);

            Assert.NotNull(result);
            Assert.Equal(expected.UserId, result.UserId);
        }

        [Theory]
        [AutoDomainData]
        public async Task CreateAccountInfo_WhenExists(AccountExtension account)
        {
            var result = await _sut.CreateAccountInfoAsync(account.UserId, account);

            Assert.Equal(account.SizeInUse, result.SizeInUse);
            _accountRepoMock.Verify(x => x.AddAsync(new AccountExtension(account.UserId)), Times.Never());
        }

        [Theory]
        [AutoDomainData]
        public async Task RemoveFileFromStorage_NegativeSize(AccountExtension account)
        {
            long size = account.SizeInUse + 100;

            _accountRepoMock.Setup(x => x.GetByIdAsync(account.UserId))
                .ReturnsAsync(account);

            var result = await _sut.RemoveFileFromStorageAsync(account.UserId, size);
            
            Assert.Equal(account.SizeInUse, result.SizeInUse);
        }

        [Theory]
        [AutoDomainData]
        public async Task RemoveFileFromStorage_Success(AccountExtension account)
        {
            long size = account.SizeInUse;
            long expectedSize = account.SizeInUse - size;
            _accountRepoMock.Setup(x => x.GetByIdAsync(account.UserId))
                .ReturnsAsync(account);

            var result = await _sut.RemoveFileFromStorageAsync(account.UserId, size);
            
            Assert.Equal(expectedSize, result.SizeInUse);
        }
    }
}
