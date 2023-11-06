using Filuet.Infrastructure.Abstractions.Enums;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Products.Infrastructure.Entities
{
    public class ProductLocalizationEntity : EntityDeletable<int>
    {
        public int ProductId { get; set; }
        public string Attribute { get; set; }
        public Language Language { get; set; }
        public string Value { get; set; }
    }
}