using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Account_2.Models
{
    [Serializable]
    public class ResultData
    {
        public string data;
        public string message;
        public bool success;

        public ResultData(bool success,string message,string data="")
        {
            this.data = data;
            this.message = message;
            this.success = success;
        }

        public string GetJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}