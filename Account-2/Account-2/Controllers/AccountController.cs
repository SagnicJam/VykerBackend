using Account_2.Helpers;
using Account_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Account_2.Controllers
{
    [RoutePrefix("accounts")]
    public class AccountController : ApiController
    {
        public string accountTableName = "AccountsTable";

        public AccountController()
        {
            SQLJAM.Initialise();
        }

        [Route("home")]
        [HttpPost]
        public string Home()
        {
            return "Account Home!";
        }

        [Route("register")]
        [HttpPost]
        public string Register([FromBody]RegisterData rd)
        {
            AccountData ad = new AccountData(rd);
            ad.verified = false;
            ad.registerOTP = GetOTP();
            string json = ad.GetJson();
            bool success = SQLJAM.InsertIntoTable(accountTableName,ad.email,json);
            if (success)
            {
                //MailToUser otp for verify here
                ResultData comData = new ResultData(true, "Registered!");
                return comData.GetJson();
            }
            else
            {
                return "User already exists!";
            }
        }

        [Route("verify")]
        [HttpPost]
        public string Verify([FromBody]VerifyData vd)
        {
            string json = SQLJAM.SelectFromTable(accountTableName,vd.email);
            AccountData ad = new AccountData(json);
            if(vd.otp==ad.registerOTP)
            {
                ad.verified = true;
                string json2 = ad.GetJson();
                bool success = SQLJAM.UpdateTable(accountTableName,vd.email,json2);
                if(success)
                {
                    return "Verified!";
                }
                else
                {
                    return "User dont exists!";
                }
            }
            else
            {
                return "Wrong OTP!";
            }
        }

        [Route("login")]
        [HttpPost]
        public string Login([FromBody]LoginData ld)
        {
            string json = SQLJAM.SelectFromTable(accountTableName, ld.email);
            if (json == "")
            {
                return "Email Doesnt Exist!";
            }
            else
            {
                AccountData ad = new AccountData(json);
                if (ad.verified)
                {
                    ad.lastSessionKey = Guid.NewGuid().ToString();

                    if (ad.password == ld.password)
                    {
                        SQLJAM.UpdateTable(accountTableName, ld.email, ad.GetJson());
                        return "Logged in ! " + ad.lastSessionKey;
                    }
                    else
                    {
                        return "Wrong Password!";
                    }
                }
                else
                {
                    return "Account not verified!";
                }
            }
        }

        [Route("forgotPassword")]
        [HttpPost]
        public string ForgotPassword([FromBody]ForgotPasswordData fpd)
        {
            string json = SQLJAM.SelectFromTable(accountTableName,fpd.email);
            if(json=="")
            {
                return "Incorrect email!";
            }
            else
            {
                AccountData ad = new AccountData(json);
                int otp = GetOTP();
                ad.forgotPasswordOTP = otp;
                bool success = SQLJAM.UpdateTable(accountTableName,fpd.email,ad.GetJson());
                if(success)
                {
                    //Mail to user to the otp
                    return "OTP sent: "+otp;
                }
                else
                {
                    return "Something went wrong!";
                }
            }
        }

        [Route("changePassword")]
        [HttpPost]
        public string ChangePassword([FromBody]ChangePasswordData cpd)
        {
            string json = SQLJAM.SelectFromTable(accountTableName,cpd.email);
            if(json=="")
            {
                return "Email not found!";
            }
            else
            {
                AccountData ad = new AccountData(json);
                if(ad.forgotPasswordOTP==-1)
                {
                    return "OTP expired!";
                }
                else if(ad.forgotPasswordOTP!=cpd.otp)
                {
                    return "Wrong otp!";
                }
                else
                {
                    ad.password = cpd.newPassword;
                    ad.forgotPasswordOTP = -1;
                    ad.lastSessionKey = "";
                    bool success = SQLJAM.UpdateTable(accountTableName,cpd.email,ad.GetJson());
                    if(success)
                    {
                        return "Password changed!";
                    }
                    else
                    {
                        return "Something went wrong!";
                    }
                }
            }
        }

        int GetOTP()
        {
            Random r = new Random();
            int rInt = r.Next(100000,999999);
            return rInt;
        }
    }
}
