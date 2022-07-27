namespace CloudStorage.Core.Entities
{
    public class AccountStorage
    {
        public string UserId { get; }
        public long SizeInUse { get; set; } = 0;
        public long SizeLimit { get; set; } = 1024 * 1024 * 50;

        public AccountStorage()
        {

        }

        public AccountStorage(string userId)
        {
            UserId = userId;
        }

        public void AddFile(long fileSize)
        {
            var size = SizeInUse + fileSize;

            if(size > SizeLimit)
            {
                throw new Exception();
            }

            SizeInUse = size;
        }

        public void RemoveFile(long fileSize)
        {
            var size = SizeInUse - fileSize;
            if(size < 0)
            {
                throw new Exception();
            }

            SizeInUse = size;
        }
    }
}
