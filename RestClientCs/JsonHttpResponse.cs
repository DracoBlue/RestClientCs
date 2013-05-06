using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RestClientCs
{
    public class JsonHttpResponse : BaseHttpResponse, HttpResponse
    {
        protected JObject rawJsonObject = null;
        protected JArray rawJsonArray = null;

        public JsonHttpResponse(HttpWebResponse webResponse) : base(webResponse)
        {
            if (this.rawResponseString.Length > 0 && this.rawResponseString.Substring(0, 1) == "{")
            {
                this.rawJsonObject = JObject.Parse(this.rawResponseString);
            }
            else if (this.rawResponseString.Length > 0 && this.rawResponseString.Substring(0, 1) == "[")
            {
                this.rawJsonArray = JArray.Parse(this.rawResponseString);
            }
            else if (this.rawResponseString == "null")
            {
                this.rawJsonObject = new JObject();
            }
            else
            {
                if ((int)webResponse.StatusCode >= 400 && (int)webResponse.StatusCode < 600)
                {
                    this.rawJsonObject = new JObject();
                }
                else
                {
                    throw new JsonHttpResponseException("Response is no valid JsonObject or JsonArray");
                }
            }
        }

        public JsonHttpResponse(string rawResponseString)
            : base(rawResponseString)
        {
            this.rawResponseString = rawResponseString;

            if (this.rawResponseString.Substring(0, 1) == "{")
            {
                this.rawJsonObject = JObject.Parse(this.rawResponseString);
            }
            else if (this.rawResponseString.Substring(0, 1) == "[")
            {
                this.rawJsonArray = JArray.Parse(this.rawResponseString);
            }
            else if (this.rawResponseString == "null")
            {
                this.rawJsonObject = new JObject();
            }
            else
            {
                throw new JsonHttpResponseException("Response is no valid JsonObject or JsonArray: " + rawResponseString);
            }
        }

        public JObject getValue()
        {
            return this.rawJsonObject;
        }

        public HttpResponse[] getValues()
        {
            HttpResponse[] values = new HttpResponse[this.rawJsonArray.Count];
            int pos = 0;
            foreach (JObject rawObject in this.rawJsonArray)
            {
                values[pos] = new JsonHttpResponse(rawObject.ToString());
                pos++;
            }

            return values;
        }

        public HttpAgent getLink(string linkName)
        {
            if (this.rawJsonObject.Value<JArray>("links") != null)
            {
                foreach (JObject link in this.rawJsonObject.Value<JArray>("links"))
                {

                    if (link.Value<string>("rel") == linkName)
                    {
                        if (link.Value<string>("type") != null)
                        {
                            WebHeaderCollection headers = new WebHeaderCollection();
                            headers.Add(HttpResponseHeader.ContentType, link.Value<string>("type"));
                            return new HttpAgent(link.Value<string>("href"), headers);
                        }
                        return new HttpAgent(link.Value<string>("href"));
                    }
                }
            }

            throw new JsonHttpResponseException("Link not found!");
        }

        public bool hasLink(string linkName)
        {
            try
            {
                this.getLink(linkName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int getLinksCount()
        {
            try
            {
                JArray links = this.rawJsonObject.Value<JArray>("links");
                return links.Count;
            }
            catch
            {
                return 0;
            }
        }
    }
}

