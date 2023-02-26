using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reqres.APITests.Utils.Exceptions
{
    public class FaultErrorMessages
    {
        public static string GenericPageException => FaultMessageConfigurationHelper.GetMessage("GenericPageException");
        public static string NoSuchWindowException => FaultMessageConfigurationHelper.GetMessage("WindowNotFoundException");
        public static string NoSuchElementException => FaultMessageConfigurationHelper.GetMessage("ElementNotFoundException");
        public static string ElementNotVisibleException => FaultMessageConfigurationHelper.GetMessage("ElementNotVisibleException");
        public static string WebDriverException => FaultMessageConfigurationHelper.GetMessage("WebDriverFailure");
        public static string WebDriverTimeoutException => FaultMessageConfigurationHelper.GetMessage("WebDriverTimeout");
        public static string XPathLookupException => FaultMessageConfigurationHelper.GetMessage("XPathLookupException");
        public static string NotFoundException => FaultMessageConfigurationHelper.GetMessage("NotFoundException");
        public static string InvalidOperationException => FaultMessageConfigurationHelper.GetMessage("InvalidOperationException");
        public static string InvalidCastException => FaultMessageConfigurationHelper.GetMessage("InvalidCastException");
        public static string NullReferenceException => FaultMessageConfigurationHelper.GetMessage("NullReferenceException");
        public static string TimeoutException => FaultMessageConfigurationHelper.GetMessage("TimeoutException");
        public static string NotSupportedException => FaultMessageConfigurationHelper.GetMessage("NotSupportedException");
        public static string AssertFailedException => FaultMessageConfigurationHelper.GetMessage("AssertFailedException");
    }
}