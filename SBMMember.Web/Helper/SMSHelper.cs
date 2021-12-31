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
        public static string SendSMS()
        {
            Uri targetUri = new Uri("http://trans.dreamztechnolgy.org/smsstatuswithid.aspx?mobile=9823091141&pass=td2200bsiv&senderid=SBMPCM&to=9172293692&msg=testmessage");
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
                return "Request-Timeout";
            }
            catch (Exception ex)
            {
                return "error";
            }
            finally { webRequest.Abort(); }
        }

        public static string GenerateOTP()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            return r;
        }
    }
}
