using Reqres.APITests.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using TechTalk.SpecFlow;

namespace Reqres.APITests.Tests.TestDefinitions
{
    [Binding]
    public sealed class CustomReportWriter
    {
        private static Logger log = Logger.GetInstance(typeof(CustomReportWriter));

        /// <summary>
        /// create test in runtime and attach to report
        /// </summary>
        [BeforeScenario]
        public static void BeforeScenario()
        {
            new CustomReport().CreateTest(ScenarioContext.Current.ScenarioInfo.Title);
        }

        /// <summary>
        /// Add stepinfor to the report
        /// </summary>
        [BeforeStep]
        public static void BeforeStep()
        {
            new CustomReport().
            AddStatusToReport(Reqres.APITests.Utils.CustomReport.ReportStatus.Info,
            "<font color='#22a1c4'>Executing-></font>" + GetScenarioStepText());
        }

        /// <summary>
        /// load extent config
        /// </summary>
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            //call static constructor to load extent report config
            CustomReport cr = new CustomReport();
            //Get the latest TFS Test run Id if execution is in TFS
            // CustomReport.GetTFSTestRunId();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            try
            {
                new CustomReport().SaveReport();
            }
            catch (Exception exp)
            {
                Logger.GetInstance(typeof(Logger)).LogException("Logger", "AfterTestRun", exp);
            }
        }

        /// <summary>
        /// attach scenario status to report
        /// </summary>
        [AfterScenario]
        public static void AfterScenario()
        {
            //check If current scenario status is error
            var status = ScenarioContext.Current.TestError;
            if (status == null)
            {
                var executionStatus = ScenarioExecutionStatus().ToString();
                //if scenario status is not error and status is "OK" then mark test as pass
                if (executionStatus.Equals("OK"))
                {
                    new CustomReport().AddStatusToReport
                    (CustomReport.ReportStatus.Pass,
                   "<b><font color='green'>Scenario Passed</font></b> " + ScenarioContext.Current.ScenarioInfo.Title.ToString());
                }
                //if status is not pass or error then add that as warning
                else
                {
                    new CustomReport().AddStatusToReport
                    (CustomReport.ReportStatus.Warning,
                    ScenarioContext.Current.ScenarioInfo.Title.ToString());
                }
            }
            else
            {
                new CustomReport().AddStatusToReport
                (CustomReport.ReportStatus.Error, "<font color='red'>Scenario Failed</font>"
                + ScenarioContext.Current.ScenarioInfo.Title.ToString());
            }
        }

        [AfterStep]
        public static void AfterStep()
            {
            var status = ScenarioContext.Current.TestError;
            var step = GetScenarioStepText();
            if (status != null)
            {
                var messageToLog = status.Message.ToString();
                if (messageToLog.Contains(">"))
                {
                    messageToLog = messageToLog.Replace(">", "&gt;");
                }
                if (messageToLog.Contains("<"))
                {
                    messageToLog = messageToLog.Replace("<", "&lt;");
                }
                new CustomReport().AddStatusToReport
               (CustomReport.ReportStatus.Error, "<b><font color='red'>Failed</font></b> ->" + step.ToString() +
               "|<b> Reason</b> :-" + messageToLog);
                //atach innerException
                var innerException = status.InnerException;
                if (innerException != null)
                {
                    new CustomReport().AddStatusToReport
                  (CustomReport.ReportStatus.Error, "<b><font color='red'>InnerExeption</font></b> ->"
                  + "<textarea class='code-block'>" +
                    status.InnerException + "</textarea>");
                }
                if (status.StackTrace != null)
                {
                    new CustomReport().AddStatusToReport
                    (CustomReport.ReportStatus.Error, "<b><font color='red'>StackTrace</font></b> ->"
                    + "<textarea class='code-block'>" +
                    status.StackTrace + "</textarea>");
                }
            }
            else
            {
                string executionStatus = "";
                if (ScenarioExecutionStatus() != null)
                {
                    executionStatus = ScenarioExecutionStatus().ToString();
                }
                else
                {
                    executionStatus = "Unable to get scenario status";
                }

                //if scenario status is not error and status is "OK" then mark test as pass
                if (executionStatus.Equals("OK"))
                {
                    new CustomReport().AddStatusToReport
                    (CustomReport.ReportStatus.Pass,
                    "<b><font color='green'>Passed</font></b> -> " + GetScenarioStepText());
                }
                else
                {
                    new CustomReport().AddStatusToReport
                    (CustomReport.ReportStatus.Warning,
                   GetScenarioStepText());
                }
            }
        }

        private static string GetScenarioStepText()
        {
            int currentPositionText = 0;
            try
            {
                var frames = new StackTrace(true).GetFrames();
                if (frames != null)
                {
                    var featureFileFrame = frames.FirstOrDefault(f =>
                                                                 f.GetFileName() != null &&
                                                                 f.GetFileName().EndsWith(".feature"));

                    if (featureFileFrame != null)
                    {
                        var lines = File.ReadAllLines(featureFileFrame.GetFileName());
                        const int frameSize = 20;
                        int currentLine = featureFileFrame.GetFileLineNumber() - 1;
                        int minLine = Math.Max(0, currentLine - frameSize);
                        int maxLine = Math.Min(lines.Length - 1, currentLine + frameSize);

                        for (int lineNo = currentLine - 1; lineNo >= minLine; lineNo--)
                        {
                            if (lines[lineNo].TrimStart().StartsWith("Scenario:"))
                            {
                                minLine = lineNo + 1;
                                break;
                            }
                        }

                        for (int lineNo = currentLine + 1; lineNo <= maxLine; lineNo++)
                        {
                            if (lines[lineNo].TrimStart().StartsWith("Scenario:"))
                            {
                                maxLine = lineNo - 1;
                                break;
                            }
                        }

                        for (int lineNo = minLine; lineNo <= maxLine; lineNo++)
                        {
                            if (lineNo == currentLine)
                            {
                                currentPositionText = lineNo - minLine;
                                return String.Format("->" + lines[lineNo]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogException("report writer", "error getting executung stepname", ex);
            }

            return String.Format("(Unable to detect current step)");
        }

        public static object ScenarioExecutionStatus()
        {
            object result = null;
            try
            {
                string element = "TestStatus";
                var pInfo = typeof(ScenarioContext).GetProperty(element, BindingFlags.Instance | BindingFlags.NonPublic);
                var getter = pInfo.GetGetMethod(nonPublic: true);
                result = getter.Invoke(ScenarioContext.Current, null);
            }
            catch (Exception ex)
            {
                log.LogException("report writer", "error getting ScenarioExecutionStatus", ex);
            }
            return result;
        }
    }
}
