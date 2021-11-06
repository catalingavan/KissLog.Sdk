using System;

namespace KissLog.RestClient
{
    public class ApiException
    {
        public string ErrorMessage { get; set; }

        internal static ApiException Create(string stringResponse)
        {
            ApiException result = null;

            if(!string.IsNullOrEmpty(stringResponse))
            {
                try
                {
                    result = KissLogConfiguration.JsonSerializer.Deserialize<ApiException>(stringResponse);
                }
                catch
                {
                    // ignored
                }
            }

            if(string.IsNullOrEmpty(result?.ErrorMessage))
            {
                result = new ApiException
                {
                    ErrorMessage = "HTTP exception"
                };
            }

            return result;
        }

        internal static ApiException Create(Exception ex)
        {
            return new ApiException
            {
                ErrorMessage = ex.ToString()
            };
        }
    }
}
