using Filuet.Infrastructure.Abstractions.Helpers;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Advertisement.Infrastructure.Entities;
using MPT.Vending.Domains.Advertisement.Infrastructure.Repositories;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Advertisement.Abstractions
{
    public class MediaService : IMediaService
    {
        public MediaService(AdMediaRepository adMediaRepository,
            KioskMediaLinkRepository kioskMediaLinkRepository,
            KioskMediaLinkViewRepository kioskMediaLinkViewRepository) {
            _adMediaRepository = adMediaRepository;
            _kioskMediaLinkRepository = kioskMediaLinkRepository;
            _kioskMediaLinkViewRepository = kioskMediaLinkViewRepository;
        }

        public IEnumerable<AdMedia> Get() {
            IEnumerable<AdMediaEntity> media = _adMediaRepository.Get(x => true).ToList();
            IEnumerable<KioskMediaLinkEntity> links = _kioskMediaLinkRepository.Get(x => true).ToList();
            foreach (var m in media) {
                yield return new AdMedia {
                    Hash = m.Hash,
                    Name = m.Name,
                    Type = m.Type,
                    CanDelete = !links.Any(x => x.MediaId == m.Id),
                    Size = m.Size
                };
            }
        }

        public IEnumerable<KioskMediaLink> GetByKiosk(string kioskUid)
            => _kioskMediaLinkViewRepository.Get(x => x.KioskUid == kioskUid).Select(x => new KioskMediaLink {
                Active = x.Active,
                Media = new AdMedia {
                    Hash = x.Media.Hash,
                    Name = x.Media.Name,
                    Type = x.Media.Type,
                    CanDelete = false,
                    Size = x.Media.Size
                },
                Start = x.Start
            });

        public void Put(NewMediaRequest request) {
            AdMediaEntity? media = _adMediaRepository.Get(x => x.Hash == request.Hash.ToLower()).FirstOrDefault();
            if (media != null)
                return;

            _adMediaRepository.Put(new AdMediaEntity {
                Hash = request.Hash,
                Name = Path.GetFileNameWithoutExtension(request.FileName),
                Type = EnumHelpers.GetValueFromCode<AdMediaType>(Path.GetExtension(request.FileName).Replace(".", "")),
                Size = request.Size
            });
        }

        public void Delete(string hash)
            => _adMediaRepository.Delete(hash);

        private readonly AdMediaRepository _adMediaRepository;
        private readonly KioskMediaLinkRepository _kioskMediaLinkRepository;
        private readonly KioskMediaLinkViewRepository _kioskMediaLinkViewRepository;
    }
}
