﻿using Filuet.Hardware.Dispensers.Abstractions.Models;
using MPT.Vending.Domains.Kiosks.Abstractions;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Kiosks.Services
{
    public class DemoReplenishmentService : IReplenishmentService
    {
        public event EventHandler<PoG> onPlanogramChanged;
        public PoG GetPlanogram(string kioskUid)
            => DemoData._planogram;
    }
}