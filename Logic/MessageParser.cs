﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTClient.Logic
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
    }
}