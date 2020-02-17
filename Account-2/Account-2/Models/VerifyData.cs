using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Account_2.Models
{
    [Serializable]
    public class VerifyData
    {
        public string email;
        public int otp;
    }
}