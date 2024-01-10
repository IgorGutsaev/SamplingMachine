using MPT.Vending.API.Dto;
using System.Collections.Concurrent;

namespace MessagingServices
{
    public class StockBalance
    {
        private ConcurrentDictionary<string, IEnumerable<ProductStock>> _stock; // (kioskUid, stock)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kioskUid"></param>
        /// <param name="stock">running low products<></param>
        public void Update(string kioskUid, IEnumerable<ProductStock> stock) {
            if (_stock == null)
                _stock = new ConcurrentDictionary<string, IEnumerable<ProductStock>>();

            _stock.AddOrUpdate(kioskUid, stock, (x, oldValue) => stock);
        }

        /// <summary>
        /// Bind runningLow balance on start
        /// </summary>
        /// <param name="getRunningLowProducts"></param>
        public StockBalance(Func<IEnumerable<KioskStock>> getStock)
        {
            _stock = new ConcurrentDictionary<string, IEnumerable<ProductStock>>();
            var runningLowProducts = getStock();
            foreach (var kiosk in runningLowProducts)
                Update(kiosk.KioskUid, kiosk.Balance);
        }

        public IEnumerable<KioskStock>? Stock
            => _stock?.Select(x => new KioskStock {
                KioskUid = x.Key,
                Balance = x.Value
            });
    }
}