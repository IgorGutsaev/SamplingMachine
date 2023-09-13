﻿using Filuet.Infrastructure.Abstractions.Enums;
using Microsoft.AspNetCore.Mvc;
using MPT.Vending.API.Dto;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KioskController : ControllerBase
    {
        public KioskController(ILogger<KioskController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public KioskDto Get(string uid)
            => new KioskDto
            {
                UID = "foo",
                IdleTimeout = TimeSpan.FromMinutes(1),
                Languages = new Language[] { Language.Hindi, Language.English },
                ProductLinks = new KioskProductLink[] {
                    new KioskProductLink {
                        Credit = 1,
                        MaxCountPerSession = 2,
                        TotalCount = 10,
                        Product = new ProductDto {
                            Sku = "Aby",
                            Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Abyssinian"), LocalizedValue.Bind(Language.Hindi, "एबिसिनियन") },
                            Picture = Properties.Resources.Abyssinian
                        }
                    },
                    new KioskProductLink {
                        Credit = 2,
                        MaxCountPerSession = 1,
                        TotalCount = 5,
                        Product = new ProductDto {
                            Sku = "Ori",
                            Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Oriental"), LocalizedValue.Bind(Language.Hindi, "ओरिएंटल") },
                            Picture = Properties.Resources.Oriental
                        }
                    },
                    new KioskProductLink {
                        Credit = 3,
                        MaxCountPerSession = 1,
                        TotalCount = 3,
                        Product = new ProductDto {
                            Sku = "Sav",
                            Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Savannah"), LocalizedValue.Bind(Language.Hindi, "सवाना") },
                            Picture = Properties.Resources.Savannah
                        }
                    },
                    new KioskProductLink {
                        Credit = 1,
                        MaxCountPerSession = 3,
                        TotalCount = 10,
                        Product = new ProductDto {
                            Sku = "Rus",
                            Names =new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Russian blue"), LocalizedValue.Bind(Language.Hindi, "रूसी नीला") },
                            Picture = Properties.Resources.RussianBlue
                        }
                    },
                    new KioskProductLink {
                        Credit = 3,
                        MaxCountPerSession = 1,
                        TotalCount = 1,
                        Product = new ProductDto {
                            Sku = "Ben",
                            Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Bengal"), LocalizedValue.Bind(Language.Hindi, "बंगाल") },
                            Picture = Properties.Resources.Bengal
                        }
                    },
                    new KioskProductLink {
                        Credit = 1,
                        MaxCountPerSession = 5,
                        TotalCount = 6,
                        Product = new ProductDto {
                            Sku = "Ang",
                            Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Angora"), LocalizedValue.Bind(Language.Hindi, "अंगोरा") },
                            Picture = Properties.Resources.Angora
                        }
                    },
                    new KioskProductLink {
                        Credit = 1,
                        MaxCountPerSession = 2,
                        TotalCount = 3,
                        Product = new ProductDto {
                            Sku = "Bir",
                            Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Birman"), LocalizedValue.Bind(Language.Hindi, "बिरमन") },
                            Picture = Properties.Resources.British
                        }
                    },
                    new KioskProductLink {
                        Credit = 2,
                        MaxCountPerSession = 2,
                        TotalCount = 2,
                        Product = new ProductDto {
                            Sku = "Bom",
                            Names =new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Bombay"), LocalizedValue.Bind(Language.Hindi, "बॉम्बे") },
                            Picture = Properties.Resources.Bombay
                        }
                    },
                    new KioskProductLink {
                        Credit = 1,
                        MaxCountPerSession = 2,
                        TotalCount = 4,
                        Product = new ProductDto {
                            Sku = "Brt",
                            Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "British Shorthair"), LocalizedValue.Bind(Language.Hindi, "ब्रिटिश शॉर्टहेयर") },
                            Picture = Properties.Resources.British
                        }
                    },
                    new KioskProductLink {
                        Credit = 3,
                        MaxCountPerSession = 1,
                        TotalCount = 1,
                        Product = new ProductDto {
                            Sku = "Mnc",
                            Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Munchkin"), LocalizedValue.Bind(Language.Hindi, "मंचकिन") },
                            Picture = Properties.Resources.Munchkin
                        }
                    },
                    new KioskProductLink {
                        Credit = 2,
                        MaxCountPerSession = 1,
                        TotalCount = 2,
                        Product = new ProductDto {
                            Sku = "Sbr",
                            Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Siberian"), LocalizedValue.Bind(Language.Hindi, "साइबेरियाई") },
                            Picture = Properties.Resources.Siberian
                        }
                    },
                    new KioskProductLink {
                        Credit = 1,
                        MaxCountPerSession = 2,
                        TotalCount = 4,
                        Product = new ProductDto {
                            Sku = "Sco",
                            Names =new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Scottish fold"), LocalizedValue.Bind(Language.Hindi, "स्कॉटिश मोड़") },
                            Picture = Properties.Resources.Scottish
                        }
                    },
                    new KioskProductLink {
                        Credit = 2,
                        MaxCountPerSession = 1,
                        TotalCount = 2,
                        Product = new ProductDto {
                            Sku = "Bur",
                            Names =new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Burmilla"), LocalizedValue.Bind(Language.Hindi, "बर्मिला") },
                            Picture = Properties.Resources.Burmilla
                        }
                    }
                }
            };

        private readonly ILogger<KioskController> _logger;
    }
}