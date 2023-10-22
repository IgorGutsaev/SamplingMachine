namespace MPT.Vending.Domains.Advertisement.Abstractions
{
    public interface IBlobRepository
    {
        Task<Blob> DownloadAsync(string uid);
        Task<string> UploadAsync(byte[] data, string uid);
    }
}
