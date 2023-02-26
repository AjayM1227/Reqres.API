using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reqres.APITests.Utils
{
    public abstract class BaseAutomationException : Exception
    {
        /// <summary>
        /// The logger instance to be initialized by each inheriting instance.
        /// </summary>
        public abstract Logger Logger { get; }

        /// <summary>
        /// This is the base exception.
        /// </summary>
        protected BaseAutomationException()
            : base()
        {
            Logger.LogException(String.Empty, String.Empty, this);
        }

        /// <summary>
        /// This is the base excpeiton.
        /// </summary>
        /// <param name="message">A custom message for the exception.</param>
        protected BaseAutomationException(String message)
            : base(message)
        {
            Logger.LogException(String.Empty, String.Empty, this, message);
        }

        /// <summary>
        /// This is the base excpeiton.
        /// </summary>
        /// <param name="message">This is the message.</param>
        /// <param name="innerException">This is the inner exception.</param>
        protected BaseAutomationException(string message
         , Exception innerException)
            : base(message, innerException)
        {
            Logger.LogException(String.Empty, String.Empty, this, message);
        }
    }
}