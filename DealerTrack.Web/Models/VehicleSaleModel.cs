using System;

namespace DealerTrack.Web.Models
{
    [Serializable]
    public class VehicleSaleModel
    {
        public int DealNumber { set; get; }
        public string CustomerName { set; get; }
        public string DealershipName { set; get; }
        public string Vehicle { set; get; }
        public decimal Price { set; get; }
        public DateTime Date { set; get; }

    }
}
