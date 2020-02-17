using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Account_2.Models
{
    [Serializable]
    public class ChangePasswordData
    {
        public int otp;
        public string email;
        public string newPassword;
    }
}