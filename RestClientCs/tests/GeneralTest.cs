using System;
using NUnit.Framework;
using System.Threading;
using System.Collections.Generic;

namespace RestClientCs
{
	[TestFixture()]
	public class GeneralTest
	{	

		const string NOT_FOUND_URL = "https://github.com/repos/DracoBlue/RestClientCsNotFound";
		// const string NOT_FOUND_URL = "http://workspaces.local/UrlNotFound.json";
		const string WORKING_READONLY_JSON_URL = "https://api.github.com/repos/DracoBlue/RestClientCs";
		// const string WORKING_READONLY_JSON_URL = "http://workspaces.local/RestClientCsReplyAnything.php";
		const string WORKING_JSON_URL = "https://api.github.com/repos/DracoBlue/RestClientCs";
		// const string WORKING_JSON_URL = "http://workspaces.local/RestClientCs.json";
		const string WORKING_JSON_STRING_KEY = "name";
		const string WORKING_JSON_STRING_VALUE = "RestClientCs";

		[SetUp]
		public void BeforeTest()
		{
			System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
		}


		[Test()]
		public void TestGeneral()
		{
			/*
			HttpAgent httpAgent = new HttpAgent("https://api.github.com/repos/DracoBlue/RestClientCs");
			JsonHttpResponse json = await httpAgent.get(httpResponse => {
				JsonHttpResponse jsonHttpResponse = httpResponse as JsonHttpResponse; 
				Assert.AreEqual(jsonHttpResponse.getValue().GetValue("name"), "RestClientCs");
			});
			*/
		}

		[Test()]
		public void ValidJsonGetRequest()
		{
			HttpAgent httpAgent = new HttpAgent(WORKING_JSON_URL);
			HttpResponse httpResponse = httpAgent.get();
			Assert.IsInstanceOf<JsonHttpResponse>(httpResponse);
			JsonHttpResponse jsonHttpResponse = httpResponse as JsonHttpResponse;
			Assert.AreEqual(jsonHttpResponse.getValue().Value<String>(WORKING_JSON_STRING_KEY), WORKING_JSON_STRING_VALUE);
		}

		[Test()]
		public void NotFoundWithInvalidJson()
		{
			HttpAgent httpAgent = new HttpAgent(NOT_FOUND_URL);
			HttpResponse httpResponse = httpAgent.get();
			Assert.IsInstanceOf<JsonHttpResponse>(httpResponse);
			Assert.AreEqual(httpResponse.isOk(), false);
		}

		[Test()]
		public void PostAgainstAResourceWhichCannotBePosted()
		{
			HttpAgent httpAgent = new HttpAgent(WORKING_READONLY_JSON_URL);
			Dictionary<String, String> postParameters = new Dictionary<String, String>();
			HttpResponse httpResponse = httpAgent.post(postParameters);

			Assert.IsInstanceOf<JsonHttpResponse>(httpResponse);
			Assert.AreEqual(httpResponse.isOk(), false);
		}
	}
}

