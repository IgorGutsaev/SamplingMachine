using MPT.Vending.Domains.SharedContext.Models;

namespace MPT.Vending.Domains.SharedContext.Abstractions
{
    public interface IBlobRepository
    {
        Task<Blob> DownloadAsync(string uid);
        Task<string> UploadAsync(byte[] data, string uid);
    }
}
