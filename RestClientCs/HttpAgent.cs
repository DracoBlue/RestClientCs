using System;
using System.Web;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace RestClientCs
{
    public class HttpAgent
    {
        protected string url;
        protected WebHeaderCollection headers = null;

        public delegate void RequestCompleteDelegate(HttpResponse response);

        public HttpAgent(string url)
        {
            this.url = url;
            this.headers = new WebHeaderCollection();
        }

        public HttpAgent(string url, WebHeaderCollection headers)
        {
            this.url = url;
            this.headers = headers;
        }

		public HttpResponse get()
		{
			return this.rawCall("get", null, null);
		}
		
		public void get(RequestCompleteDelegate completeDelegate)
        {
			this.rawCall("get", null, null, completeDelegate);
        }
        
		public HttpResponse get(Dictionary<string, string> parameters)
        {
            return this.rawCall("get", parameters, null);
        }
        
		public void get(Dictionary<string, string> parameters, RequestCompleteDelegate completeDelegate)
		{
			this.rawCall("get", parameters, null, completeDelegate);
		}

		public HttpResponse post(Dictionary<string, string> parameters)
        {
            return this.rawCall("post", null, parameters);
        }
        
		public void post(Dictionary<string, string> parameters, RequestCompleteDelegate completeDelegate)
		{
			this.rawCall("post", null, parameters, completeDelegate);
		}

		public HttpResponse patch(Dictionary<string, string> parameters)
		{
			return this.rawCall("patch", null, parameters);
		}

		public void patch(Dictionary<string, string> parameters, RequestCompleteDelegate completeDelegate)
        {
			this.rawCall("patch", null, parameters, completeDelegate);
        }
        
		public HttpResponse delete()
		{
			return this.rawCall("delete", null, null);
		}

		public void delete(RequestCompleteDelegate completeDelegate)
        {
			this.rawCall("delete", null, null, completeDelegate);
        }

		protected HttpWebRequest createHttpRequest (string verb, Dictionary<string, string> queryParameters, Dictionary<string, string> postParameters)
		{
			string url = this.url;
			
			if (verb == "patch")
			{
				verb = "post";
				
				if (postParameters == null)
				{
					postParameters = new Dictionary<string, string>();
				}
				
				postParameters.Add("_method", "patch");
			}
			
			if (queryParameters != null && verb == "get")
			{
				foreach (var pair in queryParameters)
				{
					url = url + (url.Contains("?") ? "&" : "?") + pair.Key + "=" + pair.Value;
				}
			}
			
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			
			request.Method = verb;
			request.Headers = this.headers;
			request.Accept = "application/json";
			
			if (postParameters != null)
			{
				if (verb == "get")
				{
					throw new Exception("Cannot send POST data with GET request!");
				}
				
				string postData = null;
				
				foreach (var pair in postParameters)
				{
					if (postData != null)
					{
						postData += "&";
					}
					
					postData = postData + pair.Key + "=" + pair.Value;
				}

				if (postData == null)
				{
					postData = "";
				}
				
				byte[] utf8EncodedPostData = Encoding.UTF8.GetBytes(postData);
				request.ContentType = "application/x-www-form-urlencoded";
				request.ContentLength = utf8EncodedPostData.Length;
				using (Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(utf8EncodedPostData, 0, utf8EncodedPostData.Length);
				}
			}

			return request;
		}

		protected HttpResponse getHttpResponseForRequest(HttpWebRequest request, IAsyncResult asyncResult = null)
		{
			try
			{
				if (asyncResult == null)
				{
					HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
					return new JsonHttpResponse(webResponse);                    
				}
				else
				{
					HttpWebResponse webResponse = (HttpWebResponse)request.EndGetResponse(asyncResult);
					return new JsonHttpResponse(webResponse);                    
				}
			}
			catch (WebException webException)
			{
				try
				{
					return new JsonHttpResponse((HttpWebResponse)webException.Response);
				}
				catch (JsonHttpResponseException)
				{
					return new NotOkHttpResponse();
				}
			}
		}
        
		protected void rawCall(string verb, Dictionary<string, string> queryParameters, Dictionary<string, string> postParameters, RequestCompleteDelegate completeDelegate)
        {
			HttpWebRequest request = this.createHttpRequest(verb, queryParameters, postParameters);

            request.BeginGetResponse(asyncResult =>
            {
				completeDelegate(this.getHttpResponseForRequest(request, asyncResult));
            }, null);
        }

		protected HttpResponse rawCall(string verb, Dictionary<string, string> queryParameters, Dictionary<string, string> postParameters)
		{
			HttpWebRequest request = this.createHttpRequest(verb, queryParameters, postParameters);

			return this.getHttpResponseForRequest(request);
		}
    }
}

