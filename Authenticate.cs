using System.Linq;
using System.Net;
using RestSharp;

namespace CodApi
{
    public class Authenticate
    {
        public string atkn { get; set; }
        public string sso { get; set; }
        public string xsrf { get; set; }

        public Authenticate()
        {
        }

        public void Login(string email, string password)
        {
            using (WebClient webClient = new WebClient())
            {
                string htmlCode = webClient.DownloadString("https://profile.callofduty.com/cod/login");
                const string _csrf = "_csrf\" value=\"";

                var start = htmlCode.LastIndexOf(_csrf) + _csrf.Length;
                var end = start;

                while (htmlCode[end] != '"') end++;

                xsrf = htmlCode.Substring(start, end - start);

                var client = new RestClient("https://s.activision.com/do_login?new_SiteId=activision");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);

                request.AddHeader("Cookie", "XSRF-TOKEN=" + xsrf + "; new_SiteId=activision;");
                request.AddParameter("username", email);
                request.AddParameter("password", password);
                request.AddParameter("remember_me", "true");
                request.AddParameter("_csrf", xsrf);
                IRestResponse response = client.Execute(request);

                string header = (string)response.Headers
                    .Where(x => x.Name == "Set-Cookie")
                    .Select(x => x.Value)
                    .FirstOrDefault();

                string[] words = header.Split(';');

                foreach (var word in words)
                {
                    if (word.Contains("atkn="))
                    {
                        start = word.LastIndexOf("atkn=") + 5;
                        end = word.Length;
                        atkn = word.Substring(start, end - start);
                    }
                    else if (word.Contains("ACT_SSO_COOKIE="))
                    {
                        start = word.LastIndexOf("ACT_SSO_COOKIE=") + 15;
                        end = word.Length;
                        sso = word.Substring(start, end - start);
                    }
                }
            }
        }
    }
}
