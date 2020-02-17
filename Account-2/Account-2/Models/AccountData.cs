using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Account_2.Models
{
    [Serializable]
    public class AccountData
    {
        public string email;
        public string username;
        public string password;
        public int registerOTP;
        public int forgotPasswordOTP;
        public bool verified;
        public string lastSessionKey;

        public AccountData(RegisterData rd)
        {
            username = rd.username;
            email = rd.email;
            password = rd.password;
        }

        public AccountData(string json)
        {
            JsonConvert.PopulateObject(json,this);
        }

        public string GetJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}