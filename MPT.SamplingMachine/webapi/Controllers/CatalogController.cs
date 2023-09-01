using Filuet.Infrastructure.Abstractions.Business;
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
                    new KeyValuePair<string, string>(Language.Hindi.GetCode(), "एबिसिनियन बिल्ली") }),
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
                Picture = Properties.Resources.Savannah }
        };

    private readonly ILogger<CatalogController> _logger;
}