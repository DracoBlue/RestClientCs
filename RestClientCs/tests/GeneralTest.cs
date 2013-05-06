using System;
using NUnit.Framework;
using System.Threading;
using System.Collections.Generic;

namespace RestClientCs
{
	[TestFixture()]
	public class GeneralTest
	{	
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
			string serverUrl = Environment.GetEnvironmentVariable("TEST_SERVER_URL");
			HttpAgent httpAgent = new HttpAgent(serverUrl + "?_status=200&message=Message");
			HttpResponse httpResponse = httpAgent.get();
			Assert.IsInstanceOf<JsonHttpResponse>(httpResponse);
			JsonHttpResponse jsonHttpResponse = httpResponse as JsonHttpResponse;
			Assert.AreEqual(jsonHttpResponse.getValue().Value<String>("message"), "Message");
			Assert.AreEqual(httpResponse.isOk(), true);
		}

		[Test()]
		public void NotFoundWithInvalidJson()
		{
			string serverUrl = Environment.GetEnvironmentVariable("TEST_SERVER_URL");
			HttpAgent httpAgent = new HttpAgent(serverUrl + "?_status=404&message=Message");
			HttpResponse httpResponse = httpAgent.get();
			Assert.IsInstanceOf<JsonHttpResponse>(httpResponse);
			Assert.AreEqual(httpResponse.isOk(), false);
		}

		[Test()]
		public void PostAgainstAResourceWhichCannotBePosted()
		{
			string serverUrl = Environment.GetEnvironmentVariable("TEST_SERVER_URL");
			HttpAgent httpAgent = new HttpAgent(serverUrl + "?_status=405&message=Method%20not%20allowed");
			Dictionary<String, String> postParameters = new Dictionary<String, String>();
			HttpResponse httpResponse = httpAgent.post(postParameters);

			Assert.IsInstanceOf<JsonHttpResponse>(httpResponse);
			Assert.AreEqual(httpResponse.isOk(), false);
		}
	}
}

