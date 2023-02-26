using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reqres.APITests.Utils
{

    public class BrowseAPIURL
    {
        /// Initialize base reqres login Url.
        /// </summary>
        private readonly string _baseUrl = null;

        // Property of URL
        public string GetURL
        {
            get { return this._baseUrl; }
        }

        /// <summary>
        /// Gets the base API Url from configuration.
        /// </summary>
        /// <param name="apiType">This is API Type</param>
        public BrowseAPIURL(string apiType, string parameter1 = "", string parameter2 = "", string
            parameter3 = "", string parameter4 = "", string parameter5 = "", string parameter6 = "", string parameter7 = "")
        {
            try
            {
                string enviornmentName = ConfigurationManager.AppSettings["Environment"].ToString();
                switch (apiType)
                {
                    // Get API base URL
                    case "ReqresAPIGetListUsers":
                        _baseUrl = string.Format(BrowseAPIURLResource.ReqresAPIGetListUsersBaseUrl, parameter1);
                        break;

                   
                    case "ReqresAPIPOSTCreateUser":
                        _baseUrl = string.Format(BrowseAPIURLResource.ReqresAPIPOSTCreateUserBaseUrl);
                        break;
                    case "ReqresAPIGetSingleUser":
                    case "ReqresAPIPUTUpdateSingleUser":
                    case "ReqresAPIPATCHUpdateSingleUser":
                        _baseUrl = string.Format(BrowseAPIURLResource.ReqresAPIGetSingleUsersBaseUrl, parameter1);
                        break;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}