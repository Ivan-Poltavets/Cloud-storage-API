namespace CloudStorage.Core.Entities
{
    public class AccountStorage
    {
        public string UserId { get; set; }
        public long SizeInUse { get; set; } = 0;
        public long SizeLimit { get; } = 1024 * 1024 * 50;

        public AccountStorage()
        {

        }

        public AccountStorage(string userId)
        {
            UserId = userId;
        }

        public AccountStorage AddFile(long fileSize)
        {
            var size = SizeInUse + fileSize;

            if(size < SizeLimit)
            {
                SizeInUse = size;
            }

            return this;
        }

        public void RemoveFile(long fileSize)
        {
            var size = SizeInUse - fileSize;
            
            if(size >= 0)
            {
                SizeInUse = size;
            }
        }
    }
}
