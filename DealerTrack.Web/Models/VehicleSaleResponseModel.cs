using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealerTrack.Web.Models
{
    [Serializable]
    public class VehicleSaleResponseModel
    {
        public IList<VehicleSaleModel> List { set; get; }
        public string MostOftenSoldVehicle { get; set; }
    }
}
