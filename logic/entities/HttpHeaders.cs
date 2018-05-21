using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTClient.logic.entities
{
    class HttpHeaders
    {
        public readonly static string Host = "Host";
        public readonly static string UserAgent = "User-Agent";
        public readonly static string ContentLength = "Content-Length";
        public readonly static string Connection = "Connection";
        public readonly static string TransferEncoding = "Transfer-Encoding";
        public readonly static string ContentType = "Content-Type";

        public static string DefaultHttpVersion = "HTTP/1.1";
        public static string DefaultUserAgent = "KTClient";
        public static string DefaultConnection = "keep-alive";
    }
}
