using System.Text;

namespace KTClient.Logic
{
    class DataObject
    {
        private const int DEFAULT_BUFFER_SIZE = 1024;
        private const EncodingMethod DEFAULT_ENCODING_METHOD = EncodingMethod.ASCII;

        private byte[] buffer;
        private string stringRepresentation;
        private EncodingMethod encodingMethod;

        public DataObject()
        {
            buffer = new byte[DEFAULT_BUFFER_SIZE];
            stringRepresentation = string.Empty;
            encodingMethod = DEFAULT_ENCODING_METHOD;
        }

        public DataObject(int bufferSize)
        {
            buffer = new byte[bufferSize];
            stringRepresentation = string.Empty;
            encodingMethod = DEFAULT_ENCODING_METHOD;
        }

        public DataObject(byte[] buffer)
        {
            this.buffer = buffer;
            this.stringRepresentation = this.getEncodedString(buffer);
            encodingMethod = DEFAULT_ENCODING_METHOD;
        }

        public DataObject(string stringRepresentation)
        {
            this.buffer = this.getEncodedBytes(stringRepresentation);
            this.stringRepresentation = stringRepresentation;
            encodingMethod = DEFAULT_ENCODING_METHOD;
        }

        public byte[] getBuffer()
        {
            return this.buffer;
        }

        public string getStringRepresentation()
        {
            return this.stringRepresentation;
        }

        public void setBuffer(byte[] buffer)
        {
            this.buffer = buffer;
            this.stringRepresentation = this.getEncodedString(buffer);
        }

        public void setStringRepresentation(string stringRepresentation)
        {
            this.buffer = this.getEncodedBytes(stringRepresentation);
            this.stringRepresentation = stringRepresentation;
        }

        // append string representation of data object by another string
        public void appendStringRepresentation(string appendedString)
        {
            this.stringRepresentation += appendedString;
            this.buffer = this.getEncodedBytes(this.stringRepresentation);
        }

        // append string representation of data object by array of byte
        public void appendStringRepresentation(byte[] appendedBuffer)
        {
            this.stringRepresentation += this.getEncodedString(appendedBuffer);
            this.buffer = this.getEncodedBytes(this.stringRepresentation);
        }

        public void setEncodingMethod(EncodingMethod encodingMethod)
        {
            this.encodingMethod = encodingMethod;
        }

        private byte[] getEncodedBytes(string str)
        {
            byte[] bytes;
            switch (this.encodingMethod)
            {
                case EncodingMethod.UTF8:
                    bytes = Encoding.UTF8.GetBytes(str);
                    break;
                case EncodingMethod.ASCII:
                    bytes = Encoding.ASCII.GetBytes(str);
                    break;
                case EncodingMethod.Unicode:
                    bytes = Encoding.Unicode.GetBytes(str);
                    break;
                default:
                    bytes = Encoding.ASCII.GetBytes(str);
                    break;
            }
            return bytes;
        }

        private string getEncodedString(byte[] bytes)
        {
            string str;
            switch (this.encodingMethod)
            {
                case EncodingMethod.UTF8:
                    str = Encoding.UTF8.GetString(bytes);
                    break;
                case EncodingMethod.ASCII:
                    str = Encoding.ASCII.GetString(bytes);
                    break;
                case EncodingMethod.Unicode:
                    str = Encoding.Unicode.GetString(bytes);
                    break;
                default:
                    str = Encoding.ASCII.GetString(bytes);
                    break;
            }
            return str;
        }
        
        // clear data in object
        public void clear()
        {
            this.setStringRepresentation("");
        }
    }
}
