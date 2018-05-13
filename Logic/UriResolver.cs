using System;
using System.Net;
using System.Net.Sockets;

namespace KTClient.Logic
{
    class UriResolver
    {
        private Uri uri;

        public UriResolver()
        {
            this.uri = null;
        }

        public UriResolver(Uri uri)
        {
            this.uri = uri;
        }

        public UriResolver(string stringUri)
        {
            this.setUri(stringUri);
        }

        public IPAddress[] getIPAddresses()
        {
            if (this.uri != null)
            {
                IPAddress[] iPAddresses;
                if (this.uri.HostNameType == UriHostNameType.Dns)
                {
                    try
                    {
                        iPAddresses = Dns.GetHostAddresses(this.uri.Host);
                    }
                    catch (SocketException)
                    {
                        iPAddresses = null;
                    }
                }
                else
                {
                    iPAddresses = new IPAddress[1];
                    iPAddresses[0] = IPAddress.Parse(this.uri.Host);
                }
                return iPAddresses;
            } else
            {
                return null;
            }
        } 

        private string preprocessUri(string preUri)
        {
            if (!preUri.StartsWith("http://"))
            {
                preUri = "http://" + preUri;
            }
            return preUri;
        }

        public Uri getUri()
        {
            return this.uri;
        }

        public void setUri(Uri uri)
        {
            this.uri = uri;
        }

        public void setUri(string stringUri)
        {
            stringUri = this.preprocessUri(stringUri);
            try
            {
                this.uri = new Uri(stringUri);
            }
            catch (System.UriFormatException)
            {
                this.uri = null;
            }
        }
    }
}
