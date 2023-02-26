using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace Reqres.APITests.Utils
{
    /// <summary>
    /// This is Extent report creation class
    /// Reports will be saved in TestResults folder
    /// </summary>
    public class CustomReport
    {
        public static ExtentReports report = new ExtentReports();
        public static ExtentTest test;
        public static string repotPath = "";
        public static long testRunId;
        public static bool isRemoteExecution = false;

        public enum ReportStatus
        {
            Pass,
            Info,
            Skip,
            Warning,
            Error,
            Debug,
            Fatal,
            AttachScreenshot,
            Fail
        }

        /// <summary>
        /// load config for first time
        /// </summary>
        static CustomReport()
        {
            //create test reults directory
            String Folderpath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).
                GetDirectories("TestResults")[0].FullName;
            string path = "";
            if (!Directory.Exists(Folderpath))
            {
                Directory.CreateDirectory(Folderpath);
            }

            ExtentHtmlReporter htmlrep;
            //get TestResults folder path and create report
            repotPath = GetCustomReportHTMLFilePath();
            htmlrep = new ExtentHtmlReporter(repotPath);
            // Get extent app config file path
            path = GetExtentAppConfigFilePath();

            htmlrep.LoadConfig(path);
            report.AttachReporter(htmlrep);
            //Display environment details
            report.AddSystemInfo("Environment", ConfigurationManager.AppSettings["Environment"]);
            report.AddSystemInfo("Browser", ConfigurationManager.AppSettings["Browser"]);
            report.AddSystemInfo("App Server", ConfigurationManager.AppSettings["AppServerName"]);
            report.AddSystemInfo("DB Server", ConfigurationManager.AppSettings["DBServerName"]);
            report.AddSystemInfo("Product", ConfigurationManager.AppSettings["Product"]);
            report.AddSystemInfo("MachineName", Environment.MachineName);
            report.AddSystemInfo("OS", Environment.OSVersion.ToString());
        }

        ///// <summary>
        ///// This method gets the latest TFS test runId via TFS API
        ///// </summary>
        //public static void GetTFSTestRunId()
        //{
        //    if (isRemoteExecution)
        //        testRunId = RestAPI.ExceuteTFSAPIToGetTestRunId();
        //}

        /// <summary>
        /// Get the html path for custom report
        /// </summary>
        /// <returns>html report path</returns>
        private static String GetCustomReportHTMLFilePath()
        {
            //get xml file path
            var reportFilePath = Path.Combine(new string[] {
                                                   Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).
                                                   GetDirectories("TestResults")[0].FullName,
                                                   ConfigurationManager.AppSettings[AutomationConfigurationManagerResource.Product_Key].ToUpper()+
                                                   AutomationConfigurationManagerResource.TextSeparator+
                                                   ConfigurationManager.AppSettings[AutomationConfigurationManagerResource.AppServerName_Key].ToUpper()+
                                                   AutomationConfigurationManagerResource.TextSeparator+ Logger.GetApplicationStartDateTime(),
                                                   AutomationConfigurationManagerResource.Reports_Path,
                                                   ConfigurationManager.AppSettings[AutomationConfigurationManagerResource.Product_Key].ToUpper()+
                                                   AutomationConfigurationManagerResource.TextSeparator+
                                                   ConfigurationManager.AppSettings[AutomationConfigurationManagerResource.AppServerName_Key].ToUpper()+
                                                   AutomationConfigurationManagerResource.TextSeparator+
                                                   DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".html"});
            return reportFilePath;
        }

        /// <summary>
        /// Get the extent config XML File Path.
        /// </summary>
        /// <returns>Extent config TestData xml file path.</returns>
        private static String GetExtentAppConfigFilePath()
        {
            // get xml file path
            return Path.Combine(
                new string[] {
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    AutomationConfigurationManagerResource.ExtentReportAppConfigFileName + ".xml" });
        }

        /// <summary>
        /// Adds environment details to report dashboard
        /// </summary>
        /// <param name="type"></param>
        /// <param name="param"></param>
        public void addSysInfo(string type, string param)
        {
            report.AddSystemInfo(type, param);
        }

        /// <summary>
        /// Creats new test
        /// should be called befoe sceanrio
        /// </summary>
        /// <param name="name"></param>
        public void CreateTest(string name)
        {
            test = report.CreateTest(name);
        }

        /// <summary>
        /// Saves extent report,Should be called after Test run
        /// </summary>
        public void SaveReport()
        {
            report.Flush();
        }

        /// <summary>
        /// Adds status to the report
        /// </summary>
        /// <param name="status">ReportStatus</param>
        public void AddStatusToReport(ReportStatus status, string message, string customMessage = "")
        {
            switch (status)
            {
                case ReportStatus.Pass:
                    test.Pass(message);
                    break;

                case ReportStatus.Info:
                    test.Info(message);
                    break;

                case ReportStatus.Skip:
                    test.Skip(message);
                    break;

                case ReportStatus.Warning:
                    test.Warning(message);
                    break;

                case ReportStatus.Error:
                    test.Error(message);
                    // Check if custom message exist and write to Extent report
                    if (!string.IsNullOrWhiteSpace(customMessage))
                    {
                        test.Error(customMessage);
                    }
                    break;

                case ReportStatus.Debug:
                    test.Debug(message);
                    // Check if custom message exist and write to Extent report
                    if (!string.IsNullOrWhiteSpace(customMessage))
                    {
                        test.Debug(customMessage);
                    }
                    break;

                case ReportStatus.Fatal:
                    test.Fatal(message);
                    // Check if custom message exist and write to Extent report
                    if (!string.IsNullOrWhiteSpace(customMessage))
                    {
                        test.Fatal(customMessage);
                    }
                    break;

                case ReportStatus.AttachScreenshot:
                    test.AddScreenCaptureFromPath(message);
                    break;

                case ReportStatus.Fail:
                    test.Fail(message);
                    break;

                default:
                    throw new Exception("Unsupported argument " + status);
            }
        }

        /// <summary>
        /// Creates bug column in report
        /// </summary>
        /// <param name="ex">This is exception type.</param>
        /// <param name="screenshotPath">This is screen shot.</param>
        /// <param name="customExceptionMessage">This is customized exception name.</param>
        public void AddFailStatusToReport(Exception ex, string screenshotPath, string customExceptionMessage = "")
        {
            if (!string.IsNullOrWhiteSpace(customExceptionMessage))
            {
                test.Error(customExceptionMessage);
            }
            if (ex.InnerException != null)
            {
                test.Error(ex.InnerException);
            }
            else if (ex.Message.ToString() != null)
            {
                test.Error(ex.Message);
            }
            test.AddScreenCaptureFromPath(screenshotPath);
        }
    }
}
