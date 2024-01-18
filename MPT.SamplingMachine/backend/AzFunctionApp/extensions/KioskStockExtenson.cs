using Filuet.Infrastructure.Abstractions.Helpers;
using MPT.Vending.API.Dto;
using System.Linq;
using System.Text;

namespace FunctionApp.extensions
{
    public static class KioskStockExtenson
    {
        public static string Md5Hash(this KioskStock stock) {
            StringBuilder sb = new StringBuilder();
            sb.Append(stock.KioskUid + "|");
            foreach (var s in stock.Stock.OrderBy(x => x.Uid))
                sb.Append($"{s.Uid}.{s.Quantity}.{s.MaxQuantuty}");

            return sb.ToString().CalculateMd5Hash().ToLower();
        }
    }
}
