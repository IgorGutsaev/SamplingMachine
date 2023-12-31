﻿using Filuet.Infrastructure.Abstractions.Helpers;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Advertisement.Infrastructure.Entities;
using MPT.Vending.Domains.Advertisement.Infrastructure.Repositories;

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

        public AdMedia Get(string hash) {
            AdMediaEntity? media = _adMediaRepository.Get(x => x.Hash == hash).FirstOrDefault();
            if (media == null)
                return null;

            IEnumerable<KioskMediaLinkEntity> links = _kioskMediaLinkRepository.Get(x => x.MediaId == media.Id).ToList();
            return new AdMedia {
                Hash = media.Hash,
                Name = media.Name,
                Type = media.Type,
                CanDelete = !links.Any(),
                Size = media.Size
            };
        }

        public IEnumerable<KioskMediaLink> GetByKiosk(string kioskUid)
            => _kioskMediaLinkViewRepository.Get(x => x.KioskUid == kioskUid).ToList().Select(x => new KioskMediaLink {
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

        public Dictionary<string, IEnumerable<KioskMediaLink>> GetByKiosks(IEnumerable<string> kiosks)
            => _kioskMediaLinkViewRepository.Get(x => kiosks.Contains(x.KioskUid)).GroupBy(x => x.KioskUid).ToList()
               .ToDictionary(x => x.Key, x => x.Select(e => new KioskMediaLink {
                   Active = e.Active,
                   Media = new AdMedia {
                       Hash = e.Media.Hash,
                       Name = e.Media.Name,
                       Type = e.Media.Type,
                       CanDelete = false,
                       Size = e.Media.Size
                   },
                   Start = e.Start
               }));

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
