namespace CloudStorage.Core.Entities
{
    public class AccountStorage
    {
        public Guid UserId { get; set; }
        public long SizeInUse { get; set; } = 0;
        public long SizeLimit { get; set; } = 1024 * 1024 * 50;
    }
}
