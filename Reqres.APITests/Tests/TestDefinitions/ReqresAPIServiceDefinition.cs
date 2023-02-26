using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reqres.APITests.APIs.ReqresAPIService;
using Reqres.APITests.APIs.ReqresAPIService.Response;
using Reqres.APITests.Utils;
using TechTalk.SpecFlow;

namespace Reqres.APITests.Tests.TestDefinitions
{
    [Binding]
    public sealed class ReqresAPIServiceDefinition
    {
        /// <summary>
        /// The static instance of the logger for the class.
        /// </summary>
        private static readonly Logger Logger =
        Logger.GetInstance(typeof(ReqresAPIServiceDefinition));
        /// <summary>
        /// The instance for AIP URL
        /// </summary>
        private BrowseAPIURL apiBaseURL;

        /// <summary>
        /// This is the api URL
        /// </summary>
        public static string _apiURL = "";     


        /// <summary>
        /// Getting the list of users 
        /// </summary>
        /// <param name="apiType">This is the API type</param>
        /// <param name="pageNumber">This is the poage number</param>
        [Given(@"I access the API endpoint ""(.*)"" with (.*)")]
        public void GETRequestForListOfUsers(string apiType, int pageNumber)
        {
            //Create API Base URL object
            apiBaseURL = new BrowseAPIURL(apiType, pageNumber.ToString());
            // Get URL
            _apiURL = apiBaseURL.GetURL;
            new ReqresAPIService().SetURL(_apiURL);
        }

        /// <summary>
        /// Executing the get request
        /// </summary>
        /// <param name="apiOperationType">This is the API operation type</param>
        /// <param name="apiType">This is the API type</param>
        [When(@"I Generate GET API request")]
        public void ExecuteGETRequest()
        {
            RestAPIHelper.GETRequest();
        }

        /// <summary>
        /// Getting the status code of the API
        /// </summary>
        /// <param name="StatusCode">This is the status code</param>
        /// <param name="apiType">This is the API type</param>
        [Then(@"I should be displayed with response (.*) for API ""(.*)""")]
        public void GetResponseForAPI(int StatusCode, string apiType)
        {
            // Get the response status code
            int statusCode = new ReqresAPIService().GetAPIResponse(apiType);
            // Assert the status code
            Logger.LogAssertion("ReqresAPIServiceDefinition", ScenarioContext.Current.ScenarioInfo.Title, () =>
            Assert.AreEqual(StatusCode, statusCode));
            
        }

        /// <summary>
        ///Validating the user from the users list
        /// </summary>
        /// <param name="firstName">This is the users first name</param>
        /// <param name="lastName">This is the users last name</param>
        /// <param name="email">This is the users email id</param>
        /// <param name="userName">This is the users username</param>

        [Then(@"user list response should contain FirstName (.*), LastName (.*), Email (.*), UserName(.*)")]
        public void ValidateReqresAPIGetListUsersResponseData(string firstName, string lastName, string email, string userName)
        {
            // Validate the Responces
            if (new ReqresAPIService().GetAPIResponseStatusCodeForAPIReqresAPIGetListUsers() == 200)
            {
                Logger.LogAssertion("ReqresAPIServiceDefinition", ScenarioContext.Current.ScenarioInfo.Title, () =>
                Assert.IsTrue(new ReqresAPIService().GetAPIResponseUserListDeserialize(firstName, lastName, email, userName)));
            }
            else
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Validating the single user api response
        /// </summary>
        /// <param name="name">This is name of the user</param>
        /// <param name="job">This is job of the user</param>
        [Then(@"response should contain Name (.*),  Job (.*)")]
        public void ValidateReqresAPIPUTSingleUserResponseData(string name, string job)
        {
            Logger.LogAssertion("ReqresAPIServiceDefinition", ScenarioContext.Current.ScenarioInfo.Title, () =>
                  Assert.IsTrue(new ReqresAPIService().GetAPIResponseSingleUserDeserialize(name, job)));
        }


        /// <summary>
        /// Executing the post request to create the user
        /// </summary>
        [When(@"I execute POST request for ReqresAPIPOSTCreateUser API")]
        public void ExecutePOSTRequestForReqresAPIPOSTCreateUser()
        {
            new ReqresAPIService().POSTRequestForReqresAPIPOSTCreateUser();
        }

        /// <summary>
        ///Executing the put request to update the user
        /// </summary>
        /// <param name="name">This is name of the user</param>
        /// <param name="job">This is job of the user</param>
        [When(@"I Generate PUT API request for ReqresAPIPUTUpdateSingleUser with Name (.*),  Job (.*)")]
        public void ExecutePUTRequestForReqresAPIPUTSingleUser(string name, string job)
        {
            new ReqresAPIService().PUTRequestForReqresAPIPUTSingleUser(name, job);
        }

        /// <summary>
        /// Executing the patch request to update the user
        /// </summary>
        /// <param name="name">This is name of the user</param>
        /// <param name="job">This is job of the user</param>
        [When(@"I Generate PATCH API request for ReqresAPIPATCHUpdateSingleUser with Name(.*),  Job (.*)")]
        public void ExecutePATCHRequestForReqresAPIPATCHSingleUser(string name, string job)
        {
            new ReqresAPIService().PATCHRequestForReqresAPIPATCHSingleUser(name, job);
        }

    }
}
