using System;

namespace RestClientCs
{
    public interface HttpResponse
    {
        bool isOk();
        int getLinksCount();
        HttpAgent getLink(string linkName);
        bool hasLink(string linkName);
        string getRawResponseString();
        HttpResponse[] getValues();
    }
}

