using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Response
{
    public class BarqSMSResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public ResponseData Data { get; set; }
    }

    public class ResponseData
    {
        public string Uid { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public string Cost { get; set; }
        public int Sms_Count { get; set; }
    }

}
