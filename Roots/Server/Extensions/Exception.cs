using System.Text;

namespace EDC.Server.Extensions;

public static class ExceptionExtensions
{
    public static string GetAllMessages(this Exception ex)
    {
        var builder = new StringBuilder();
        while (ex != null)
        {
            builder.AppendLine(ex.Message);
            ex = ex.InnerException;
        }
        return builder.ToString().TrimEnd();
    }
}