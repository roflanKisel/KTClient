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
            Regex contentLengthRegex = new Regex("\\\r\nContent-Length: (.*?)\\\r\n");
            Match contentLengthMatch = contentLengthRegex.Match(response.getStringRepresentation());

            Regex transferEncodingChunkedRegex = new Regex("\\\r\nTransfer-Encoding: chunked\\\r\n");
            Match transferEncodingChunkedMatch = transferEncodingChunkedRegex.Match(response.getStringRepresentation());

            if (contentLengthMatch.Success)
            {
                int contentLength = int.Parse(contentLengthMatch.Groups[1].ToString()); // get content length using regex
                byte[] bodyBuff;             
                
                int receivedLength = 0;
                // read the body
                for (int i = 0; i < contentLength; i++)
                {
                    bodyBuff = new byte[1];
                    receivedLength = socket.Receive(bodyBuff, 0, 1, 0);
                    response.appendStringRepresentation(bodyBuff);
                }
            } else if (transferEncodingChunkedMatch.Success)
            {
                byte[] bodyBuff;
                byte[] lengthBuff;
                DataObject currentLength = new DataObject();
                // while there are available bytes of data to receive
                while (socket.Available > 0)
                {
                    // receiving length of chunk
                    lengthBuff = new byte[1];
                    socket.Receive(lengthBuff, 0, 1, 0);
                    currentLength.appendStringRepresentation(lengthBuff);
                    // if length of chunk is known
                    if (currentLength.getStringRepresentation().Contains("\r\n"))
                    {
                        // format length of chunk from hexadecimal to decimal
                        string hexNumberFormat = currentLength.getStringRepresentation().Substring(
                            0, currentLength.getStringRepresentation().IndexOf("\r\n"));
                        int length = Convert.ToInt32(hexNumberFormat, 16);
                        // end of body
                        if (length == 0)
                        {
                            break;
                        }
                        int receivedLength = 0;
                        // receiving chunk
                        for (int i = 0; i < length + 2; i++)
                        {
                            bodyBuff = new byte[1];
                            receivedLength += socket.Receive(bodyBuff, 0, 1, 0);
                            response.appendStringRepresentation(bodyBuff);
                        }
                        // clear length of next chunk
                        currentLength.clear();
                    }
                }
            }
        }
    }
}
