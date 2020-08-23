using System;
using System.Collections.Generic;

namespace DealerTrack.Web.Utilities
{
    [Serializable]
    public class ResponseModel<T>
    {
        public bool IsSucceeded { set; get; } = false;
        public int ResponseCode { set; get; } = 400;
        public List<string> ResponseMessage { set; get; } = new List<string>();
        public T Result { set; get; }
    }
}
