using Filuet.Infrastructure.Abstractions.Enums;
using Filuet.Infrastructure.Abstractions.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{
    public CatalogController(ILogger<CatalogController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Product> GetProducts()
        => new [] { 
            new Product { Names = new Dictionary<string, string>(new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>(Language.English.GetCode(), "Abyssinian"),
                    new KeyValuePair<string, string>(Language.Hindi.GetCode(), "एबिसिनियन") }),
                Sku = "Aby",
                Credit = 1,
                MaxCountPerSession = 2,
                TotalCount = 10,
                Picture = Properties.Resources.Abyssinian },
            new Product { Names = new Dictionary<string, string>(new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>(Language.English.GetCode(), "Oriental"),
                    new KeyValuePair<string, string>(Language.Hindi.GetCode(), "ओरिएंटल बिल्ली") }),
                Sku = "Ori",
                Credit = 2,
                MaxCountPerSession = 1,
                TotalCount = 5,
                Picture = Properties.Resources.Oriental },
            new Product { Names = new Dictionary<string, string>(new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>(Language.English.GetCode(), "Savannah"),
                    new KeyValuePair<string, string>(Language.Hindi.GetCode(), "सवाना बिल्ली") }),
                Sku = "Sav",
                Credit = 3,
                MaxCountPerSession = 1,
                TotalCount = 3,
                Picture = Properties.Resources.Savannah },
            new Product { Names = new Dictionary<string, string>(new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>(Language.English.GetCode(), "Russian blue"),
                    new KeyValuePair<string, string>(Language.Hindi.GetCode(), "रूसी नीला") }),
                Sku = "Rus",
                Credit = 1,
                MaxCountPerSession = 3,
                TotalCount = 10,
                Picture = Properties.Resources.RussianBlue },
            new Product { Names = new Dictionary<string, string>(new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>(Language.English.GetCode(), "Bengal"),
                    new KeyValuePair<string, string>(Language.Hindi.GetCode(), "बंगाल") }),
                Sku = "Ben",
                Credit = 3,
                MaxCountPerSession = 1,
                TotalCount = 1,
                Picture = Properties.Resources.Bengal },
            new Product { Names = new Dictionary<string, string>(new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>(Language.English.GetCode(), "Angora"),
                    new KeyValuePair<string, string>(Language.Hindi.GetCode(), "अंगोरा") }),
                Sku = "Ang",
                Credit = 1,
                MaxCountPerSession = 5,
                TotalCount = 6,
                Picture = Properties.Resources.Angora },
            new Product { Names = new Dictionary<string, string>(new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>(Language.English.GetCode(), "Birman"),
                    new KeyValuePair<string, string>(Language.Hindi.GetCode(), "बिरमन") }),
                Sku = "Bir",
                Credit = 1,
                MaxCountPerSession = 2,
                TotalCount = 3,
                Picture = Properties.Resources.Birman },
            new Product { Names = new Dictionary<string, string>(new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>(Language.English.GetCode(), "Bombay"),
                    new KeyValuePair<string, string>(Language.Hindi.GetCode(), "बॉम्बे") }),
                Sku = "Bom",
                Credit = 2,
                MaxCountPerSession = 2,
                TotalCount = 2,
                Picture = Properties.Resources.Bombay },
            new Product { Names = new Dictionary<string, string>(new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>(Language.English.GetCode(), "British Shorthair"),
                    new KeyValuePair<string, string>(Language.Hindi.GetCode(), "ब्रिटिश शॉर्टहेयर") }),
                Sku = "Brt",
                Credit = 1,
                MaxCountPerSession = 2,
                TotalCount = 4,
                Picture = Properties.Resources.British },
            new Product { Names = new Dictionary<string, string>(new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>(Language.English.GetCode(), "Munchkin"),
                    new KeyValuePair<string, string>(Language.Hindi.GetCode(), "मंचकिन") }),
                Sku = "Mnc",
                Credit = 3,
                MaxCountPerSession = 1,
                TotalCount = 1,
                Picture = Properties.Resources.Munchkin },
            new Product { Names = new Dictionary<string, string>(new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>(Language.English.GetCode(), "Siberian"),
                    new KeyValuePair<string, string>(Language.Hindi.GetCode(), "साइबेरियाई") }),
                Sku = "Sbr",
                Credit = 2,
                MaxCountPerSession = 2,
                TotalCount = 2,
                Picture = Properties.Resources.Siberian },
            new Product { Names = new Dictionary<string, string>(new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>(Language.English.GetCode(), "Scottish fold"),
                    new KeyValuePair<string, string>(Language.Hindi.GetCode(), "स्कॉटिश मोड़") }),
                Sku = "Sco",
                Credit = 2,
                MaxCountPerSession = 1,
                TotalCount = 4,
                Picture = Properties.Resources.Scottish },
            new Product { Names = new Dictionary<string, string>(new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>(Language.English.GetCode(), "Burmilla"),
                    new KeyValuePair<string, string>(Language.Hindi.GetCode(), "बर्मिला") }),
                Sku = "Bur",
                Credit = 3,
                MaxCountPerSession = 1,
                TotalCount = 2,
                Picture = Properties.Resources.Burmilla }
        };

    private readonly ILogger<CatalogController> _logger;
}