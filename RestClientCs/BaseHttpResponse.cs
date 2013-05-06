using System;
using System.Web;
using System.Net;
using System.IO;
using System.Text;

namespace RestClientCs
{
    public abstract class BaseHttpResponse
    {
        protected HttpWebResponse webResponse = null;
        protected string rawResponseString = null;

        public BaseHttpResponse(HttpWebResponse webResponse)
        {
            this.webResponse = webResponse;
            this.rawResponseString = this.getRawResponseString(webResponse);
            this.webResponse.Close();
        }

        public BaseHttpResponse(string rawResponseString)
        {
            this.webResponse = null;
            this.rawResponseString = rawResponseString;
        }

        public bool isOk()
        {
            return (this.webResponse != null && this.webResponse.StatusCode == HttpStatusCode.OK) ? true : false;
        }

        
        public string getRawResponseString()
        {
            return this.rawResponseString;
        }

        private string getRawResponseString(HttpWebResponse webResponse)
        {
            string rawString = "";
            Stream receiveStream = webResponse.GetResponseStream();
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, encode);

            Char[] read = new Char[256];

            // Reads 256 characters at a time.    
            int count = readStream.Read(read, 0, 256);

            while (count > 0)
            {
                // Dumps the 256 characters on a string and displays the string to the console.
                String str = new String(read, 0, count);                        
                rawString += str;
                count = readStream.Read(read, 0, 256);
            }

            readStream.Close();

            return rawString;
        }
    }
}

