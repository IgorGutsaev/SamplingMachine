using Microsoft.AspNetCore.Mvc;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Advertisement.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        public MediaController(IMediaService mediaService,
            IBlobRepository blobRepository,
            ILogger<MediaController> logger) {
            _mediaService = mediaService;
            _blobRepository = blobRepository;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<AdMedia> Get()
            => _mediaService.Get();

        [HttpGet("kiosk")]
        public IEnumerable<KioskMediaLink> GetByKiosk(string uid)
            => _mediaService.GetByKiosk(uid).OrderBy(x => x.Start).ToList();

        [HttpPut]
        public void Put([FromBody] NewMediaRequest request)
            => _mediaService.Put(request);

        [HttpPost("upload")]
        public async Task<string> Upload(IFormFile file, CancellationToken cancellationToken) {
            using MemoryStream stream = new MemoryStream();

            string extension = Path.GetExtension(file.FileName).Replace(".", string.Empty);

            await file.CopyToAsync(stream, cancellationToken);
            stream.Position = 0;

            using var md5 = MD5.Create();
            string uid = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();

            return await _blobRepository.UploadAsync(stream.ToArray(), $"media/{extension}/{uid}");
        }

        private readonly IMediaService _mediaService;
        private readonly IBlobRepository _blobRepository;
        private readonly ILogger<MediaController> _logger;
    }
}