using System;

namespace RestClientCs
{
    public class NotOkHttpResponse : HttpResponse
    {
        public NotOkHttpResponse()
        {
        }

        public bool isOk()
        {
            return false;
        }

        public int getLinksCount()
        {
            return 0;
        }

        public HttpAgent getLink(string linkName)
        {
            throw new Exception("Link not found!");
        }

        public bool hasLink(string linkName)
        {
            return false;
        }

        public string getRawResponseString()
        {
            return "";
        }

        public HttpResponse[] getValues()
        {
            HttpResponse[] values = new HttpResponse[0];
            return values;
        }

    }
}

