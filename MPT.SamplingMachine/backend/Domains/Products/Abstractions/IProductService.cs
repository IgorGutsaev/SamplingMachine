﻿using MPT.Vending.API.Dto;

namespace MPT.Vending.Domains.Products.Abstractions
{
    public interface IProductService
    {
        IEnumerable<ProductDto> Get(); // Change ProductDto to Product entity
    }
}