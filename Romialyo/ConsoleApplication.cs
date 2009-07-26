using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo
{
    public abstract class ConsoleApplication
    {
        public ConsoleApplication(ConsoleColor foregroundColor, string applicationTitle)
        {
            ApplicationTitle = applicationTitle;
            ForegroundColor = foregroundColor;
        }

        protected readonly ConsoleColor ForegroundColor;
        protected readonly string ApplicationTitle;

        protected abstract ILogger Logger { get; }

        protected abstract void RunInternal();

        public void Run()
        {
            try
            {
                Console.Title = ApplicationTitle;
                WriteMessage("Initializing logs...");
                if (Logger == null)
                {
                    WriteMessage("No logger available! Is this an error?");
                }
                else
                {
                    WriteMessage("Logs initialized. Writing first message in logs.");
                    Logger.Info("Starting " + ApplicationTitle);
                }
            }
            catch (Exception ex)
            {
                ProcessException("Exception before running the MainAction", ex);
            }

            try
            {
                RunInternal();
            }
            catch (Exception ex)
            {
                ProcessException("Exception executing the MainAction", ex);
            }

            SayGoodbye();
        }

        public void WriteMessage(string message)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ForegroundColor;

            Console.WriteLine("-: " + message);

            Console.ForegroundColor = oldColor;
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        protected void SayGoodbye()
        {
            WriteMessage("The application ended... press ENTER to close the Window.");
            Console.ReadLine();
        }

        protected void ProcessException(string contextMessage, Exception ex)
        {
            string message = contextMessage + ": " + ex.ToString();
            WriteMessage(message);
            if (Logger == null)
            {
                WriteMessage("Could NOT log this error because the logger was not available.");
            }
            else
            {
                try
                {
                    Logger.Error(contextMessage, ex);
                    WriteMessage("Error message logged correctly.");
                }
                catch (Exception loggingEx)
                {
                    WriteMessage("Error logging message! The logger failed: " + loggingEx.ToString());
                }
            }
        }

    }
}
