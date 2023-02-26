using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Configuration;
using System.IO;

namespace Reqres.APITests.Utils
{

    public class Logger
    {
        private readonly ILog _log4NetLogger;
        private static ICollection _configLogs = log4net.Config.XmlConfigurator.Configure();

        //Holds the date and time when the application launches first time.
        private static string _applicationStartDateTime = "";

        /// <summary>
        /// Returns an instance of logger.
        /// </summary>
        /// <returns>An instance of logger.</returns>
        public static Logger GetInstance(Type T)
        {
            if (string.IsNullOrWhiteSpace(_applicationStartDateTime))
            {
                _applicationStartDateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                CreateTestResultFolders();
            }

            var logger = new Logger(T);
            return logger;
        }

        /// <summary>
        /// Logger.
        /// This method configures the path for log file to be saved
        /// </summary>
        private Logger(Type T)
        {
            _log4NetLogger = LogManager.GetLogger("Reqres.APITests");
            var LogFilePath = Path.Combine(new string[] {
                                                   Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).
                                                   GetDirectories("TestResults")[0].FullName,
                                                   ConfigurationManager.AppSettings[AutomationConfigurationManagerResource.Product_Key].ToUpper()+
                                                   AutomationConfigurationManagerResource.TextSeparator+
                                                   ConfigurationManager.AppSettings[AutomationConfigurationManagerResource.AppServerName_Key].ToUpper()+
                                                   AutomationConfigurationManagerResource.TextSeparator
                                                   + _applicationStartDateTime,
                                                   AutomationConfigurationManagerResource.Log_Path,
                                                   AutomationConfigurationManagerResource.LogFile_Name });
            GlobalContext.Properties["LogFileName"] = LogFilePath;
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// This method creates a folder inside TestResults folder of the project
        /// with project name and datetime stamp to hold reports, log
        /// in respective subfolders
        /// </summary>
        public static void CreateTestResultFolders()
        {
            try
            {
                var testResultsDirectoryPath = Path.Combine(new string[] {
                                                   Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).
                                                   GetDirectories("TestResults")[0].FullName,
                                                   ConfigurationManager.AppSettings[AutomationConfigurationManagerResource.Product_Key].ToUpper()+
                                                   AutomationConfigurationManagerResource.TextSeparator+
                                                   ConfigurationManager.AppSettings[AutomationConfigurationManagerResource.AppServerName_Key].ToUpper()+
                                                   AutomationConfigurationManagerResource.TextSeparator
                                                   + _applicationStartDateTime });
                var reportsPath = Path.Combine(new string[] { testResultsDirectoryPath, AutomationConfigurationManagerResource.Reports_Path });
                var logPath = Path.Combine(new string[] { testResultsDirectoryPath, AutomationConfigurationManagerResource.Log_Path });

                Directory.CreateDirectory(testResultsDirectoryPath);
                if (Directory.Exists(testResultsDirectoryPath))
                {
                    Directory.CreateDirectory(reportsPath);
                    Directory.CreateDirectory(logPath);
                }
            }
            catch (Exception exp)
            {
                Logger.GetInstance(typeof(Logger)).LogException("Logger", "CreateTestResultFolders", exp);
            }
        }

        /// <summary>
        /// This method returns the date time stamp of application launch
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationStartDateTime()
        {
            return _applicationStartDateTime;
        }

        /// <summary>
        /// This method logs a message.
        /// </summary>
        /// <param name="message">This is the message.</param>
        private void Log(string message)
        {
            //Log at info level the message
            _log4NetLogger.Debug(message);
        }

        /// <summary>
        /// This method logs a message.
        /// </summary>
        /// <param name="className">The name of the class.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="message">The message.</param>
        public void LogMessage(string message)
        {
            const string logMessageTemplate = "~Message = {0}";
            String logMessage = String.Format(logMessageTemplate, message);

            Log(logMessage);
        }

        /// <summary>
        /// This logs an exception.
        /// </summary>
        /// <param name="className">This is the name of the class.</param>
        /// <param name="methodName">This is the name of the method.</param>
        /// <param name="exception">This is the exception to be logged.</param>
        public void LogException(string className, string methodName, Exception exception)
        {
            LogMessage("~ Exception Message = " + exception.Message + " ~ Inner Exception = " + exception.InnerException + " ~ Stack Trace =" + exception.StackTrace);
        }

        /// <summary>
        /// This logs an exception
        /// </summary>
        /// <param name="className">This is the name of the class</param>
        /// <param name="methodName">This is the name of the method</param>
        /// <param name="exception">This is the exception to be logged</param>
        /// <param name="message"> This is the user Message</param>
        public void LogException(string className, string methodName, Exception exception, string message)
        {
            LogMessage("~ Message = " + message + "~ Exception Message = " + exception.Message + " ~ Inner Exception = " + exception.InnerException + " ~ Stack Trace =" + exception.StackTrace);
        }

        /// <summary>
        /// This method asserts an expression and logs result.
        /// </summary>
        /// <param name="testCaseName">The name of test case.</param>
        /// <param name="scenarioName">The name of the scenario.</param>
        /// <param name="message">Any user message</param>
        /// <param name="assertExpression">This is the assert expression
        /// This expression is expected to be of format ()=> Assert.Fail(),Assert.AreEqual()...
        /// The method is executed and based on result the logging is done.
        /// </param>
        /// if execution passes.</param>
        private void LogAssertion(string testCaseName, string scenarioName, string message, Action assertExpression,
             bool ContinueOnFailure = true)
        {
            const string logMessageTemplate = "~TestCaseName  = {0} ~ ScenarioName ={1} ~ Result = {2} ~ Message= {3}";

            try
            {
                assertExpression.Invoke();
                message = String.Format(logMessageTemplate, testCaseName, scenarioName, "Assert Passed",
                    message);
                Log(message);
            }
            catch (AssertFailedException afe)
            {
                message = String.Format(logMessageTemplate, testCaseName, scenarioName, "~Assert Failed~", " | UserMessage = " + message + "~ Reason = " + afe);
                Log(message);

                //write Fail status to report
                var messageToLog = afe.Message.ToString();
                //dispaly '>' in html file
                if (messageToLog.Contains(">"))
                {
                    messageToLog = messageToLog.Replace(">", "&gt;");
                }
                if (messageToLog.Contains("<"))
                {
                    messageToLog = messageToLog.Replace("<", "&lt;");
                }
                new CustomReport().AddStatusToReport(CustomReport.ReportStatus.Fail, messageToLog);
                // Close webdriver and browser instances if Assert Fails
                if (!ContinueOnFailure)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This method asserts an expression and logs result.
        /// </summary>
        /// <param name="testCaseName">The name of test case.</param>
        /// <param name="scenarioName">The name of the scenario.</param>
        /// <param name="assertExpression">This is the assert expression
        /// This expression is expected to be of format ()=> Assert.Fail(),Assert.AreEqual()...
        /// The method is executed and based on result the logging is done.
        /// </param>
        /// if execution passe.s</param>
        public void LogAssertion(string testCaseName, string scenarioName, Action assertExpression)
        {
            LogAssertion(testCaseName, scenarioName, "", assertExpression);
        }

        /// <summary>
        /// This method asserts an expression and logs result.
        /// </summary>
        /// <param name="testCaseName">The name of test case.</param>
        /// <param name="scenarioName">The name of the scenario.</param>
        /// <param name="message">Any user message.</param>
        /// <param name="exception">This is th exception being used.</param>
        /// <param name="assertExpression">This is the assert expression
        /// This expression is expected to be of format ()=> Assert.Fail(),Assert.AreEqual()...
        /// The method is executed and based on result the logging is done.
        /// </param>
        public void LogAssertion(string testCaseName, string scenarioName, string message, Exception exception, Action assertExpression)
        {
            LogAssertion(testCaseName, scenarioName, "~Message = " + message + "~ Exception = " + exception + " ~ StackTrace = " + exception.StackTrace
                , assertExpression, true);
        }
    }
}