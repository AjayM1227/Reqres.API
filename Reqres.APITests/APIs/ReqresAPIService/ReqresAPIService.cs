using Reqres.APITests.APIs.ReqresAPIService.Request;
using Reqres.APITests.APIs.ReqresAPIService.Response;
using Reqres.APITests.Utils;
using Reqres.APITests.Utils.Exceptions;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reqres.APITests.APIs.ReqresAPIService
{
    public class ReqresAPIService
    {
        public string responseDesription;
        public static string SearchResultId;
        private ReqresAPIGetListUsersResponseProperties _reqresAPIGetListUsersResponseProperties;
        public ReqresApipostCreateUserRequestProperties _reqresApipostCreateUserRequestProperties;
        public ReqresApipostCreateUserResponseProperties _reqresApipostCreateUserResponseProperties;
        public ReqresApiputUpdateSingleUserResponceProperties _reqresApiputUpdateSingleUserResponceProperties;
        public static string resFirstName = "";
        public static string resLastName = "";
        public static string resEmail = "";
        public static string resUserName = "";
        public static string resJob = "";

        /// <summary>
        /// Get API URL
        /// </summary>
        /// <param name="apiType">This is API type</param>
        public void SetURL(string apiType)
        {
            try
            {
                RestAPIHelper.SetUrl(apiType);
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e);
            }
           
        }

        /// <summary>
        /// Get API response APIResponseAPICRMSensis
        /// </summary>
        /// <param name="apiType">This is apiType</param>
        /// <returns>_statusCode</returns>
        public int GetAPIResponse(string apiType)
        {
            int _statusCode = 0;
            switch (apiType)
            {
                case "ReqresAPIGetListUsers":
                case "ReqresAPIGetSingleUser":
                    _statusCode = GetAPIResponseStatusCodeForAPIReqresAPIGetListUsers();
                    break;

                case "ReqresAPIPOSTCreateUser":
                    _statusCode = GetAPIResponseStatusCodeForAPIReqresAPIPOSTCreateUser();
                    break;

                case "ReqresAPIPUTUpdateSingleUser":
                    _statusCode = GetAPIResponseStatusCodeForAPIReqresAPIGetSingleUsers();
                    break;

                case "ReqresAPIPATCHUpdateSingleUser":
                    _statusCode = GetAPIResponseStatusCodeForAPIReqresAPIPATCHSingleUsers();
                    break;
            }
            return _statusCode;
        }


        /// <summary>
        /// Get API response
        /// </summary>
        /// <returns>response</returns>
        public int GetAPIResponseStatusCodeForAPIReqresAPIPOSTCreateUser()
        {
            int _statusCode = 0;
            try
            {
                // Get the responce
                var response = RestAPIHelper.GetResponseTLSTargetprocess();
                // Get the response status code
                _statusCode = RestAPIHelper.GetResponseStatusCode(response);
                // Get the responce
                var responsedata = RestAPIHelper.GetResponse();
                if (response.IsSuccessful)
                {
                    //Deserialize the Json response data
                    _reqresApipostCreateUserResponseProperties = new JsonDeserializer().
                    Deserialize<ReqresApipostCreateUserResponseProperties>(responsedata);
                }
                else
                {
                    responseDesription = response.Content;
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e);
            }
            finally
            {
                // Write value to extent report
                new CustomReport().AddStatusToReport(Reqres.APITests.Utils.CustomReport.ReportStatus.Info,
                   string.Format("Response Status Code: {0}", _statusCode));
            }
            return _statusCode;
        }
        

        /// <summary>
        /// Get API response
        /// </summary>
        /// <returns>response</returns>
        public int GetAPIResponseStatusCodeForAPIReqresAPIPATCHSingleUsers()
        {
            int _statusCode = 0;
            try
            {
                // Get the responce
                var response = RestAPIHelper.GetResponseTLSTargetprocess();
                // Get the response status code
                _statusCode = RestAPIHelper.GetResponseStatusCode(response);
                // Get the responce
                var responsedata = RestAPIHelper.GetResponse();
                if (response.IsSuccessful)
                {
                    //Deserialize the Json response data
                    _reqresApipostCreateUserResponseProperties = new JsonDeserializer().
                    Deserialize<ReqresApipostCreateUserResponseProperties>(responsedata);
                }
                else
                {
                    responseDesription = response.Content;
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e);
            }
            finally
            {
                // Write value to extent report
                new CustomReport().AddStatusToReport(Reqres.APITests.Utils.CustomReport.ReportStatus.Info,
                   string.Format("Response Status Code: {0}", _statusCode));
            }
            return _statusCode;
        }

        /// <summary>
        /// Get API response
        /// </summary>
        /// <returns>response</returns>
        public int GetAPIResponseStatusCodeForAPIReqresAPIGetListUsers()
        {
            int _statusCode = 0;
            try
            {
                // Get the responce
                var response = RestAPIHelper.GetResponseTLSTargetprocess();
                // Get the response status code
                _statusCode = RestAPIHelper.GetResponseStatusCode(response);
                // Get the responce
                var responsedata = RestAPIHelper.GetResponse();
                if (response.IsSuccessful)
                {
                    //Deserialize the Json response data
                    _reqresAPIGetListUsersResponseProperties = new JsonDeserializer().
                    Deserialize<ReqresAPIGetListUsersResponseProperties>(responsedata);
                }
                else
                {
                    responseDesription = response.Content;
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e);
            }
           
            return _statusCode;
        }


        /// <summary>
        /// Get API response
        /// </summary>
        /// <returns>response</returns>
        public int GetAPIResponseStatusCodeForAPIReqresAPIGetSingleUsers()
        {
            int _statusCode = 0;
            try
            {
                // Get the responce
                var response = RestAPIHelper.GetResponseTLSTargetprocess();
                // Get the response status code
                _statusCode = RestAPIHelper.GetResponseStatusCode(response);
                // Get the responce
                var responsedata = RestAPIHelper.GetResponse();
                if (response.IsSuccessful)
                {
                    //Deserialize the Json response data
                    _reqresApiputUpdateSingleUserResponceProperties = new JsonDeserializer().
                    Deserialize<ReqresApiputUpdateSingleUserResponceProperties>(responsedata);

                    resJob = _reqresApiputUpdateSingleUserResponceProperties.Job;
                    resUserName = _reqresApiputUpdateSingleUserResponceProperties.Name;
                }
                else
                {
                    responseDesription = response.Content;
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e);
            }
            return _statusCode;
        }


        /// <summary>
        /// Get FinAppCRM API response
        /// </summary>
        /// <returns>response</returns>
        public bool GetAPIResponseUserListDeserialize
            (string firstName, string lastName, string email, string userName)
        {
            int _statusCode = 0;
            bool matchStatus = false;
            try
            {
                // Get the response
                var response = RestAPIHelper.GetResponse();
                _statusCode = RestAPIHelper.GetResponseStatusCode(response);
              
                if (response.IsSuccessful)
                {
                    //Deserialize the Json response data
                    _reqresAPIGetListUsersResponseProperties = new JsonDeserializer().Deserialize<ReqresAPIGetListUsersResponseProperties>(response);
                    List<DataItem> DataItems = _reqresAPIGetListUsersResponseProperties.data;
                    if (DataItems != null && DataItems.Count > 0)
                    {
                        foreach (DataItem items in DataItems)
                        {
                            resFirstName = items.first_name;
                            resLastName = items.last_name;
                            resEmail = items.email;
                            resUserName = resFirstName + " " + resLastName;
                            if (resFirstName == firstName && resLastName == lastName && resEmail == email && resUserName.Trim()  == userName.Trim())
                            {
                                matchStatus = true;
                                break;
                            }
                        }

                    }
                }
                else
                {
                    responseDesription = response.Content;
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e);
            }
            
            return matchStatus;
        }


        /// <summary>
        /// Get FinAppCRM API response
        /// </summary>
        /// <returns>response</returns>
        public bool GetAPIResponseSingleUserDeserialize(string name, string job)
        {
            int _statusCode = 0;
            bool matchStatus = false;
            try
            {
                // Get the response
                var response = RestAPIHelper.GetResponse();
                _statusCode = RestAPIHelper.GetResponseStatusCode(response);

                if (response.IsSuccessful)
                {
                    //Deserialize the Json response data
                    _reqresApiputUpdateSingleUserResponceProperties = new JsonDeserializer().
                        Deserialize<ReqresApiputUpdateSingleUserResponceProperties>(response);
                            resJob = _reqresApiputUpdateSingleUserResponceProperties.Job;
                    resUserName = _reqresApiputUpdateSingleUserResponceProperties.Name.Trim();
                            if (resUserName == name && resJob == job)
                            {
                                matchStatus = true;
                            }
                    }
                else
                {
                    responseDesription = response.Content;
                }
                
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e);
            }
            return matchStatus;

        }


        public void POSTRequestForReqresAPIPOSTCreateUser()
        {
            _reqresApipostCreateUserRequestProperties = new ReqresApipostCreateUserRequestProperties();
            _reqresApipostCreateUserRequestProperties.Job = new RandomJobTitlePicker().getJobTitlePicker();
            _reqresApipostCreateUserRequestProperties.Name = new RandomNameGenerator().GenerateName(8);

            //Post request with subscription key
            RestAPIHelper.ReqresAPICreateUserPostRequest(_reqresApipostCreateUserRequestProperties);
        }

        public void PUTRequestForReqresAPIPUTSingleUser(string name, string job)
        {
            try
            {
                _reqresApipostCreateUserRequestProperties = new ReqresApipostCreateUserRequestProperties();
                _reqresApipostCreateUserRequestProperties.Job = job;
                _reqresApipostCreateUserRequestProperties.Name = name;

                RestAPIHelper.ReqresAPIPUTSingleUsePostRequest(_reqresApipostCreateUserRequestProperties);
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e);
            }
        }

        public void PATCHRequestForReqresAPIPATCHSingleUser(string name, string job)
        {
            try
            {
                _reqresApipostCreateUserRequestProperties = new ReqresApipostCreateUserRequestProperties();
                _reqresApipostCreateUserRequestProperties.Job = job;
                _reqresApipostCreateUserRequestProperties.Name = name;

                RestAPIHelper.ReqresAPIPATCHSingleUsePostRequest(_reqresApipostCreateUserRequestProperties);
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e);
            }
        }
        
    }
}
