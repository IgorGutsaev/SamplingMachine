﻿using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Products.Infrastructure.Entities
{
    public class KioskProductLinkEntity : Entity<int>
    {
        public int KioskId { get; set; }
        public int ProductId { get; set; }
        public ProductEntity Product { get; set; }
        public int MaxCountPerSession { get; set; }
        public int RemainingQuantity { get; set; }
        public int Credit { get; set; }
        public bool Disabled { get; set; }
    }
}