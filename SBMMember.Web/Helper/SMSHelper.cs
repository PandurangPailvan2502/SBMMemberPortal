using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SBMMember.Web.Helper
{
    public class SMSHelper: ISMSHelper
    {
        public static string SendSMS(string mobileNumber,string Message)
        {
            Uri targetUri = new Uri($"http://trans.dreamztechnolgy.org/smsstatuswithid.aspx?mobile=9823091141&pass=td2200bsiv&senderid=SBMPCM&to={mobileNumber}&msg={Message}");
            HttpWebRequest webRequest =( HttpWebRequest)System.Net.HttpWebRequest.Create(targetUri);
            webRequest.Method = WebRequestMethods.Http.Get;
            try
            {
                string webResponse = string.Empty;
                using (HttpWebResponse getresponse =
               ( HttpWebResponse) webRequest.GetResponse())
{
                    using (StreamReader reader = new
                    StreamReader(getresponse.GetResponseStream()))
                    {
                        webResponse = reader.ReadToEnd();
                        reader.Close();
                    }
                    getresponse.Close();
                }
                return webResponse;
            }
            catch (System.Net.WebException ex)
            {
                return "Request-Timeout:"+ex.Message;
            }
            catch (Exception ex)
            {
                return "error:"+ex.Message;
            }
            finally { webRequest.Abort(); }
        }

        public static string GenerateOTP()
        {
            Random generator = new Random();
            String r = generator.Next(0, 10000).ToString("D4");
            return r;
        }
        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
    }
}
