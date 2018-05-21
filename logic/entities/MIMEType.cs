using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTClient.logic.entities
{
    class MIMEType
    {
        private readonly static Dictionary<String, String> mimeMapping = new Dictionary<String, String>();
        private readonly static String DEFAULT_MIME_TYPE = "application/octet-stream";

        static MIMEType()
        {
            mimeMapping.Add("txt", "text/plain");
            mimeMapping.Add("html", "text/html");
            mimeMapping.Add("js", "application/javascript");
            mimeMapping.Add("css", "text/css");
            mimeMapping.Add("png", "image/png");
            mimeMapping.Add("jpg", "image/jpeg");
        }


        public static String getMIMETypeByExtension(String extension)
        {
            if (mimeMapping.ContainsKey(extension))
            {
                return mimeMapping[extension];
            }
            else
            {
                return DEFAULT_MIME_TYPE;
            }
        }
    }
}
