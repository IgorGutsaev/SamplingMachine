using Filuet.Infrastructure.Abstractions.Enums;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Products.Abstractions;

namespace MPT.Vending.Domains.Products.Services
{
    public class ProductService : IProductService
    {
        public IEnumerable<ProductDto> Get()
        {
            List<ProductDto> result = new List<ProductDto>();
            result.Add(new ProductDto
            {
                Sku = "Aby",
                Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Abyssinian"), LocalizedValue.Bind(Language.Hindi, "एबिसिनियन") },
                Picture = Properties.Resources.Abyssinian
            });

            result.Add(new ProductDto
            {
                Sku = "Ori",
                Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Oriental"), LocalizedValue.Bind(Language.Hindi, "ओरिएंटल") },
                Picture = Properties.Resources.Oriental
            });

            result.Add(new ProductDto
            {
                Sku = "Sav",
                Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Savannah"), LocalizedValue.Bind(Language.Hindi, "सवाना") },
                Picture = Properties.Resources.Savannah
            });

            result.Add(new ProductDto
            {
                Sku = "Rus",
                Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Russian blue"), LocalizedValue.Bind(Language.Hindi, "रूसी नीला") },
                Picture = Properties.Resources.RussianBlue
            });

            result.Add(new ProductDto
            {
                Sku = "Ben",
                Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Bengal"), LocalizedValue.Bind(Language.Hindi, "बंगाल") },
                Picture = Properties.Resources.Bengal
            });

            result.Add(new ProductDto
            {
                Sku = "Ang",
                Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Angora"), LocalizedValue.Bind(Language.Hindi, "अंगोरा") },
                Picture = Properties.Resources.Angora
            });

            result.Add(new ProductDto
            {
                Sku = "Bir",
                Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Birman"), LocalizedValue.Bind(Language.Hindi, "बिरमन") },
                Picture = Properties.Resources.Birman
            });

            result.Add(new ProductDto
            {
                Sku = "Bom",
                Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Bombay"), LocalizedValue.Bind(Language.Hindi, "बॉम्बे") },
                Picture = Properties.Resources.Bombay
            });

            result.Add( new ProductDto
            {
                Sku = "Brt",
                Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "British Shorthair"), LocalizedValue.Bind(Language.Hindi, "ब्रिटिश शॉर्टहेयर") },
                Picture = Properties.Resources.British
            });

            result.Add(new ProductDto
            {
                Sku = "Mnc",
                Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Munchkin"), LocalizedValue.Bind(Language.Hindi, "मंचकिन") },
                Picture = Properties.Resources.Munchkin
            });

            result.Add(new ProductDto
            {
                Sku = "Sbr",
                Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Siberian"), LocalizedValue.Bind(Language.Hindi, "साइबेरियाई") },
                Picture = Properties.Resources.Siberian
            });

            result.Add(new ProductDto
            {
                Sku = "Sco",
                Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Scottish fold"), LocalizedValue.Bind(Language.Hindi, "स्कॉटिश मोड़") },
                Picture = Properties.Resources.Scottish
            });

            result.Add(new ProductDto
            {
                Sku = "Bur",
                Names = new LocalizedValue[] { LocalizedValue.Bind(Language.English, "Burmilla"), LocalizedValue.Bind(Language.Hindi, "बर्मिला") },
                Picture = Properties.Resources.Burmilla
            });

            return result;
        }
    }
}