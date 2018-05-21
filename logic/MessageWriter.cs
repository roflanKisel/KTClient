using KTClient.logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KTClient.logic.entities;

namespace KTClient.logic
{
    class MessageWriter
    {
        // write response body to file
        public static void writeResponseIntoFile(string headers, string body)
        {
            string contentType = MessageParser.getHeaderValue(headers, HttpHeaders.ContentType);
            try
            {
                if (contentType != string.Empty)
                {
                    switch (contentType)
                    {
                        case "text/plain":
                            File.WriteAllText("..\\..\\resources\\text\\temp-file.txt", body);
                            Console.WriteLine("writed into temp-file.txt");
                            break;
                        case "text/html":
                            File.WriteAllText("..\\..\\resources\\web\\temp-page.html", body);
                            Console.WriteLine("writed into temp-page.html");
                            break;
                        case "image/jpeg":
                            File.WriteAllText("..\\..\\resources\\images\\temp-image.jpg", body);
                            Console.WriteLine("writed into temp-image.jpg");
                            break;
                        case "image/png":
                            File.WriteAllText("..\\..\\resources\\images\\temp-image.png", body);
                            Console.WriteLine("writed into temp-image.png");
                            break;
                        default:
                            File.WriteAllText("..\\..\\resources\\web\\temp-page.html", body);
                            Console.WriteLine("writed into default file");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Error");
                }
            } catch (IOException)
            {
                Console.WriteLine("IO exception");
            }
        }
    }
}
