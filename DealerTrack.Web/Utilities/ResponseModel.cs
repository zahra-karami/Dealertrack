using System.Collections.Generic;

namespace DealerTrack.Web.Utilities
{
    public class ResponseModel
    {
        public bool IsSucceeded { set; get; } = false;
        public int ResponseCode { set; get; } = 400;
        public List<string> ResponseMessage { set; get; } = new List<string>();
        public object Result { set; get; }
    }
}
