using Azure.Storage.Blobs;
using Filuet.Infrastructure.Abstractions.Helpers;
using MPT.Vending.Domains.SharedContext.Models;
using MPT.Vending.Domains.SharedContext.Abstractions;

namespace MPT.Vending.Domains.SharedContext.Services
{
    public class AzureBlobRepository : IBlobRepository
    {
        private readonly AzureBlobRepositorySettings _settings;

        private readonly BlobContainerClient _container;

        public AzureBlobRepository(Action<AzureBlobRepositorySettings> setupAction) {
            _settings = setupAction?.CreateTargetAndInvoke();

            BlobServiceClient blobServiceClient = new BlobServiceClient(_settings.ConnectionString);

            _container = blobServiceClient.GetBlobContainerClient(_settings.ContainerName);  //_container.CreateIfNotExists();
        }

        public async Task<Blob> DownloadAsync(string uid) {
            BlobClient blockBlob = _container.GetBlobClient(uid);

            using (MemoryStream ms = new MemoryStream()) {
                return await blockBlob.DownloadToAsync(ms).ContinueWith(r => {
                    ms.Position = 0;
                    return Blob.Create(uid, ms.ToArray());
                });
            }
        }

        public async Task<string> UploadAsync(byte[] data, string uid) {
            await _container.CreateIfNotExistsAsync();

            BlobClient blockBlob = _container.GetBlobClient(uid);

            try {
                using (MemoryStream ms = new MemoryStream()) {
                    ms.Write(data, 0, data.Length);
                    ms.Position = 0;
                    await blockBlob.UploadAsync(ms);
                }
            }
            catch (Azure.RequestFailedException ex) {
                if (ex.ErrorCode == "BlobAlreadyExists")
                    return uid;
            }

            return uid;
        }
    }
}
