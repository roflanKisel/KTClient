using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTClient.logic.entities
{
    class HttpHeaders
    {
        public static string Host = "Host";
        public static string UserAgent = "User-Agent";
        public static string ContentLength = "Content-Length";
        public static string Connection = "Connection";

        public static string DefaultHttpVersion = "HTTP/1.1";
        public static string DefaultUserAgent = "KTClient";
        public static string DefaultConnection = "keep-alive";
    }
}
