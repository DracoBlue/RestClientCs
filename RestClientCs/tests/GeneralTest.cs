using System;
using NUnit.Framework;
using System.Threading;
using System.Collections.Generic;
using System.Configuration;

namespace RestClientCs
{
	[TestFixture()]
	public class GeneralTest
	{	
		protected string serverUrl = null;

		[SetUp]
		public void BeforeTest()
		{
			System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };

			System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			serverUrl = (string) config.AppSettings.Settings["TestServerUrl"].Value;
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
			HttpAgent httpAgent = new HttpAgent(serverUrl + "?_status=404&message=Message");
			HttpResponse httpResponse = httpAgent.get();
			Assert.IsInstanceOf<JsonHttpResponse>(httpResponse);
			Assert.AreEqual(httpResponse.isOk(), false);
		}

		[Test()]
		public void PostAgainstAResourceWhichCannotBePosted()
		{
			HttpAgent httpAgent = new HttpAgent(serverUrl + "?_status=405&message=Method%20not%20allowed");
			Dictionary<String, String> postParameters = new Dictionary<String, String>();
			HttpResponse httpResponse = httpAgent.post(postParameters);

			Assert.IsInstanceOf<JsonHttpResponse>(httpResponse);
			Assert.AreEqual(httpResponse.isOk(), false);
		}
	}
}

