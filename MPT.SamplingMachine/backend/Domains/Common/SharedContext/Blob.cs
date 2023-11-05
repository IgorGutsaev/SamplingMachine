namespace MPT.Vending.Domains.SharedContext.Models
{
    public class Blob
    {
        public byte[] Data { get; private set; }

        public string Uid { get; private set; }

        static public Blob Create(string uid, byte[] data) {
            if (string.IsNullOrWhiteSpace(uid))
                throw new ArgumentException("Names must be specified!");

            if (data == null || data.Length == 0)
                throw new ArgumentException("Empty content!");

            return new Blob { Uid = uid.Trim(), Data = data };
        }
    }
}