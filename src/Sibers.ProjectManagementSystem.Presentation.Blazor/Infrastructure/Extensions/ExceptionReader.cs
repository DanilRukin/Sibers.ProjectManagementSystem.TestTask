using System.Text;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions
{
    public static class ExceptionReader
    {
        public static string ReadErrors(this Exception exception, string delimeter = "; ")
        {
            Exception currentException = exception;
            StringBuilder errors = new StringBuilder();
            while (currentException != null)
            {
                errors.Append(currentException.Message);
                errors.Append(delimeter);
                currentException = currentException.InnerException;
            }
            return errors.ToString();
        }
    }
}
