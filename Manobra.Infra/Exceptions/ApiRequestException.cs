using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Manobra.Infra.Exceptions
{
    public class ApiRequestException : WebException
    {
        public HttpStatusCode Code { get; private set; }
        public string Url { get; private set; }
        public string Reason { get; private set; }

        public ApiRequestException(HttpStatusCode code, string url, string reason)
            : base(GetFormattedMessage(code, url, reason))
        {
            Code = code;
            Url = url;
            Reason = reason;
        }

        private static string GetFormattedMessage(HttpStatusCode code, string url, string reason)
        {
            return string.Format(
                "Status: {0} - {1}, Url: {2}, Message: {3}",
                code,
                (int)code,
                url,
                reason);
        }
    }
}
