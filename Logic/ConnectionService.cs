using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;
using System;

namespace KTClient.Logic
{
    class ConnectionService
    {
        public static string sendData(Uri uri, IPAddress[] ipAddresses, string sendString)
        {
            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipAddresses, uri.Port);

            DataObject sendDataObject = new DataObject(sendString); // data that we send to server
            DataObject receiveDataObject = new DataObject(); // data that we receive from server

            socket.Send(sendDataObject.getBuffer());

            bool flag = true; // just so we know we are still reading

            while (flag)
            {
                // read the header byte by byte, until \r\n\r\n
                byte[] buffer = new byte[1];
                socket.Receive(buffer, 0, 1, 0);
                receiveDataObject.appendStringRepresentation(buffer);

                // if headers ended
                if (receiveDataObject.getStringRepresentation().Contains("\r\n\r\n"))
                {
                    readBody(receiveDataObject, socket);
                    flag = false;
                }
            }
            socket.Close();
            return receiveDataObject.getStringRepresentation();
        }

        private static void readBody(DataObject response, Socket socket)
        {
            // header is received, parsing content length
            // regular expression
            Regex regex = new Regex("\\\r\nContent-Length: (.*?)\\\r\n");
            Match match = regex.Match(response.getStringRepresentation());

            if (match.Success)
            {
                int contentLength = int.Parse(match.Groups[1].ToString()); // get content length using regex
                byte[] bodyBuff;
                // read the body
                for (int i = 0; i < contentLength; i++)
                {
                    bodyBuff = new byte[1];
                    socket.Receive(bodyBuff, 0, 1, 0);
                    response.appendStringRepresentation(bodyBuff);
                }
            }
        }
    }
}
