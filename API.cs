using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodApi
{
    public class Api
    {
        public Authenticate auth { get; set; }
        public Api()
        {
        }

        public void Auth(ref Authenticate _auth)
        {
            auth = _auth;
        }

        //Private Routes
        public async Task<Object> Identity(string version)
        {
            // crm/cod/:version/identities
            string endpoint = "crm/cod/" + version +
                              "/identities";

            return await AuthenticatedRequest(endpoint);
        }

        public async Task<Object> UserInfo()
        {
            // profile.callofduty.com/cod/userInfo/{{TOKEN_SSO}}
            var client = new RestClient("https://profile.callofduty.com/cod/userInfo/MjE1MTI1NjM0NTk5OTcwMjk3OToxNjE4NTMyMjY5MTAzOjc5NGUxYjZhMWJiMTMzZDEwYWE2YmI2YTQ4NGEyZThk" + auth.sso);
            client.Timeout = -1;

            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/json");

            var queryResult = await client.ExecuteAsync<Object>(request);
            return queryResult.Data;
        }

        public async Task<Object> SearchPlayer(string platform, string user)
        {
            // crm/cod/v2/platform/:platform/username/:username/search
            string endpoint = "crm/cod/v2/platform/" + platform +
                                "/username/" + user +
                                "/search";

            return await AuthenticatedRequest(endpoint);
        }


        //Protected Routes
        public async Task<Object> Platforms(string version, string platform, string user)
        {
            // crm/cod/:version/accounts/platform/:platform/gamer/:username
            string endpoint = "crm/cod/" + version +
                                "/accounts/platform/" + platform +
                                "/gamer/" + user;

            return await AuthenticatedRequest(endpoint);
        }

        public async Task<Object> Profile(string version, string game, string platform, string user, string mode)
        {
            // stats/cod/:version/title/:game/platform/:platform/gamer/:username/profile/type/:mode
            string endpoint = "stats/cod/" + version +
                                "/title/" + game +
                                "/platform/" + platform +
                                "/gamer/" + user +
                                "/profile/type/" + mode;

            return await AuthenticatedRequest(endpoint);
        }
        public async Task<Object> Matches(string version, string game, string platform, string user, string mode, string start, string end)
        {
            // crm/cod/:version/title/:game/platform/:platform/gamer/:username/matches/:mode/start/:start/end/:end/details
            string endpoint = "crm/cod/" + version +
                                "/title/" + game +
                                "/platform/" + platform +
                                "/gamer/" + user +
                                "/matches/" + mode +
                                "/start/" + start +
                                "/end/" + end +
                                "/details/";

            return await AuthenticatedRequest(endpoint);
        }
        public async Task<Object> AuthenticatedRequest(string endpoint)
        {
            var client = new RestClient("https://my.callofduty.com/api/papi-client/" + endpoint);
            client.Timeout = -1;

            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("Cookie", "ACT_SSO_COOKIE=" + auth.sso + "; ACT_SSO_COOKIE_EXPIRY=1591153892430; atkn=" + auth.atkn + ";");

            var queryResult = await client.ExecuteAsync<Object>(request);
            return queryResult.Data;
        }
    }
}
