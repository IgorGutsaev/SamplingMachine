using Filuet.Infrastructure.Abstractions.Enums;
using MPT.Vending.API.Dto;

namespace MPT.Vending.Domains.SharedContext
{
    public static class DemoData
    {
        public static List<KioskDto> _kiosks = new List<KioskDto>();
        public static List<ProductDto> _products = new List<ProductDto>();

        static DemoData()
        {
            _products.AddRange(new[] {
                new ProductDto {
                    Sku = "Aby",
                    Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Abyssinian"), LocalizedValue.Bind(Language.Hindi, "एबिसिनियन") },
                    Picture = Properties.Resources.Abyssinian
                },
                new ProductDto {
                    Sku = "Ori",
                    Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Oriental"), LocalizedValue.Bind(Language.Hindi, "ओरिएंटल") },
                    Picture = Properties.Resources.Oriental
                },
                new ProductDto {
                    Sku = "Sav",
                    Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Savannah"), LocalizedValue.Bind(Language.Hindi, "सवाना") },
                    Picture = Properties.Resources.Savannah
                },
                new ProductDto {
                    Sku = "Rus",
                    Names =new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Russian blue"), LocalizedValue.Bind(Language.Hindi, "रूसी नीला") },
                    Picture = Properties.Resources.RussianBlue
                },
                new ProductDto {
                    Sku = "Ben",
                    Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Bengal"), LocalizedValue.Bind(Language.Hindi, "बंगाल") },
                    Picture = Properties.Resources.Bengal
                },
                new ProductDto {
                    Sku = "Ang",
                    Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Angora"), LocalizedValue.Bind(Language.Hindi, "अंगोरा") },
                    Picture = Properties.Resources.Angora
                },
                new ProductDto {
                    Sku = "Bir",
                    Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Birman"), LocalizedValue.Bind(Language.Hindi, "बिरमन") },
                    Picture = Properties.Resources.Birman
                },
                new ProductDto {
                    Sku = "Bom",
                    Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Bombay"), LocalizedValue.Bind(Language.Hindi, "बॉम्बे") },
                    Picture = Properties.Resources.Bombay
                },
                new ProductDto {
                    Sku = "Brt",
                    Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "British Shorthair"), LocalizedValue.Bind(Language.Hindi, "ब्रिटिश शॉर्टहेयर") },
                    Picture = Properties.Resources.British
                },
                new ProductDto {
                    Sku = "Mnc",
                    Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Munchkin"), LocalizedValue.Bind(Language.Hindi, "मंचकिन") },
                    Picture = Properties.Resources.Munchkin
                },
                new ProductDto {
                    Sku = "Sbr",
                    Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Siberian"), LocalizedValue.Bind(Language.Hindi, "साइबेरियाई") },
                    Picture = Properties.Resources.Siberian
                },
                new ProductDto {
                    Sku = "Sco",
                    Names =new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Scottish fold"), LocalizedValue.Bind(Language.Hindi, "स्कॉटिश मोड़") },
                    Picture = Properties.Resources.Scottish
                },
                new ProductDto {
                    Sku = "Bur",
                    Names =new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Burmilla"), LocalizedValue.Bind(Language.Hindi, "बर्मिला") },
                    Picture = Properties.Resources.Burmilla
                }
            });

            _kiosks.Add(new KioskDto
            {
                UID = "foo",
                Credit = 1,
                IdleTimeout = TimeSpan.FromMinutes(1),
                Languages = new Language[] { Language.Hindi, Language.English },
                ProductLinks = new KioskProductLink[] {
                    new KioskProductLink {
                        Credit = 1,
                        MaxCountPerSession = 2,
                        RemainingQuantity = 10,
                        Product = _products.First(x => x.Sku == "Aby")
                    },
                    new KioskProductLink {
                        Credit = 2,
                        MaxCountPerSession = 1,
                        RemainingQuantity = 5,
                        Product = _products.First(x => x.Sku == "Ori")
                    },
                    new KioskProductLink {
                        Credit = 3,
                        MaxCountPerSession = 1,
                        RemainingQuantity = 3,
                        Product = _products.First(x => x.Sku == "Sav")
                    },
                    new KioskProductLink {
                        Credit = 1,
                        MaxCountPerSession = 3,
                        RemainingQuantity = 10,
                        Product = _products.First(x => x.Sku == "Rus")
                    },
                    new KioskProductLink {
                        Credit = 3,
                        MaxCountPerSession = 1,
                        RemainingQuantity = 1,
                        Product = _products.First(x => x.Sku == "Ben")
                    },
                    new KioskProductLink {
                        Credit = 1,
                        MaxCountPerSession = 5,
                        RemainingQuantity = 6,
                        Product = _products.First(x => x.Sku == "Ang")
                    },
                    new KioskProductLink {
                        Credit = 1,
                        MaxCountPerSession = 2,
                        RemainingQuantity = 3,
                        Product = _products.First(x => x.Sku == "Bir")
                    },
                    new KioskProductLink {
                        Credit = 2,
                        MaxCountPerSession = 2,
                        RemainingQuantity = 2,
                        Product = _products.First(x => x.Sku == "Bom")
                    },
                    new KioskProductLink {
                        Credit = 1,
                        MaxCountPerSession = 2,
                        RemainingQuantity = 4,
                        Product = _products.First(x => x.Sku == "Brt")
                    },
                    new KioskProductLink {
                        Credit = 3,
                        MaxCountPerSession = 1,
                        RemainingQuantity = 1,
                        Product = _products.First(x => x.Sku == "Mnc")
                    },
                    new KioskProductLink {
                        Credit = 2,
                        MaxCountPerSession = 1,
                        RemainingQuantity = 2,
                        Product = _products.First(x => x.Sku == "Sbr")
                    },
                    new KioskProductLink {
                        Credit = 1,
                        MaxCountPerSession = 2,
                        RemainingQuantity = 4,
                        Product = _products.First(x => x.Sku == "Sco")
                    },
                    new KioskProductLink {
                        Credit = 2,
                        MaxCountPerSession = 1,
                        RemainingQuantity = 2,
                        Product = _products.First(x => x.Sku == "Bur")
                    }
                }
            });
        }

        public static void Link(string kioskUid, string sku)
        {
            var links = _kiosks.FirstOrDefault(x => x.UID == kioskUid).ProductLinks.ToList();
            if (links.Any(x => x.Product.Sku == sku))
                return;
            else
            {
                links.Add(new KioskProductLink { Credit = 1, MaxCountPerSession = 1, Product = _products.First(x => x.Sku == sku), RemainingQuantity = 0 });
                _kiosks.FirstOrDefault(x => x.UID == kioskUid).ProductLinks = links;
            }
        }
    }
}
