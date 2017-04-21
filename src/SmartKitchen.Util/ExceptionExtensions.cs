using System;

namespace HSR.CloudSolutions.SmartKitchen.Util
{
    public static class ExceptionExtensions
    {

        public static string CreateExceptionDialogMessage(this Exception exception)
        {
            Func<Exception, string> exceptionMessage = ex => $"Type: {ex.GetType().Name}\r\nMessage: {ex.Message}";

            var message = $"An unexpected exception occured!\r\n{exceptionMessage(exception)}";
            var innerstException = exception.GetInnerstException();
            if (innerstException != null)
            {
                message += $"\r\n\r\nInnerst exception:\r\n{exceptionMessage(innerstException)}";
            }
            return message;
        }

        public static Exception GetInnerstException(this Exception exception)
        {
            if (exception.InnerException == null)
            {
                return null;
            }
            var ex = exception;
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex;
        }
    }
}
