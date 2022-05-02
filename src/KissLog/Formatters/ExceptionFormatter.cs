using System;
using System.Text;

namespace KissLog.Formatters
{
    public class ExceptionFormatter
    {
        private const string ExceptionLoggedKey = "KissLog-ExceptionLogged";

        public string Format(Exception ex, Logger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (ex == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            FormatException(ex, sb, logger);

            if(KissLogConfiguration.Options.Handlers.AppendExceptionDetails != null)
            {
                string append = KissLogConfiguration.Options.Handlers.AppendExceptionDetails.Invoke(ex);
                if(!string.IsNullOrWhiteSpace(append))
                {
                    sb.AppendLine();
                    sb.AppendLine(append);
                }
            }    

            return sb.ToString().Trim();
        }

        private void FormatException(Exception ex, StringBuilder sb, Logger logger, string header = null)
        {
            string id = $"{ExceptionLoggedKey}-{logger.Id}";

            bool alreadyLogged = ex.Data.Contains(id);
            if (alreadyLogged)
                return;

            logger.DataContainer.Add(ex);
            ex.Data.Add(id, true);

            if (!string.IsNullOrEmpty(header))
                sb.AppendLine(header);

            sb.AppendLine(ex.ToString());

            Exception innerException = ex.InnerException;
            while (innerException != null)
            {
                if (!innerException.Data.Contains(id))
                    innerException.Data.Add(id, true);

                innerException = innerException.InnerException;
            }

            Exception baseException = ex.GetBaseException();
            if (baseException != null)
            {
                FormatException(baseException, sb, logger, "Base Exception:");
            }
        }
    }
}
