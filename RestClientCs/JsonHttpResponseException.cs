using System;

namespace RestClientCs
{
	public class JsonHttpResponseException : Exception
	{
		public JsonHttpResponseException() : base() { }
		public JsonHttpResponseException(string message) : base(message) { }
		public JsonHttpResponseException(string message, Exception innerException) : base(message, innerException) { }
	}
}

