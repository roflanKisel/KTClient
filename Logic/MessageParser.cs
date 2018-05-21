using System;
using System.IO;

namespace KTClient.logic
{
    class MessageParser
    {
        public static string getHeadersFromMessage(string message)
        {
            return message.Substring(0, message.IndexOf("\r\n\r\n"));
        }

        public static string getBodyFromMessage(string message)
        {
            return message.Substring(message.IndexOf("\r\n\r\n") + 4);
        }

        public static string getHeaderValue(string headers, string headerName)
        {
            string headerValue = string.Empty;
            try
            {
                if (headers.IndexOf(headerName) != -1)
                {
                    headerValue = headers.Substring(headers.IndexOf(headerName) + headerName.Length + 1);
                    if (headerValue.IndexOf(";") < headerValue.IndexOf("\r\n") && headerValue.IndexOf(";") != -1)
                    {
                        headerValue = headerValue.Substring(0, headerValue.IndexOf(";"));
                    } else
                    {
                        headerValue = headerValue.Substring(0, headerValue.IndexOf("\r\n"));
                    }
                    headerValue = headerValue.Trim();
                }
            } catch (Exception)
            {
                headerValue = string.Empty;
            }
            return headerValue;
        }
    }
}
