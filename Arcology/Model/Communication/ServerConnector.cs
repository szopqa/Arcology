using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Windows;
using Newtonsoft.Json;
using RestSharp;

namespace Arcology.Model.Communication {

	public class ServerConnector : IConnectionProvider {

		//Login and token fields. Used to execute all actions
		public static string login = "michalszopa@outlook.com";
		public static string token = "AF5C53F85FE0178F2B8DE9C39FCB5644";

		private const string WebServiceAddress = "http://arcology.prime.future-processing.com";

		/// <summary>
		/// Executes GET method on web service
		/// </summary>
		/// <returns>Action result</returns>
		public Result Describe() {

			var encodedLogin = HttpUtility.UrlEncode(login);
			string baseUrl = $"{WebServiceAddress}/describe?login={encodedLogin}&token={token}";

			var client = new RestClient(baseUrl);

			var request = new RestRequest(Method.GET);
			request.AddHeader("cache-control", "no-cache");
			request.AddHeader("content-type", "application/json");

			IRestResponse response = client.Execute(request);

			return JsonConvert.DeserializeObject<Result>(response.Content);
		}

		/// <summary>
		/// Executes action on web service which depends on enum Action value
		/// </summary>
		public HttpStatusCode ExecuteAction(string parameter, Actions chosenAction) {

			string baseUrl = $"{WebServiceAddress}/execute";
			var client = new RestClient(baseUrl);
			var request = new RestRequest(Method.POST);
			request.AddHeader("cache-control", "no-cache");
			request.AddHeader("content-type", "application/json");

			var executor = new Executor(request);

			var actionsToExecute = new Dictionary<Actions, MakeRequest> {
				[Actions.Restart] = executor.Restart,
				[Actions.Produce] = executor.Produce,
				[Actions.ImportFood] = executor.ImportFood,
				[Actions.Propaganda] = executor.Propaganda,
				[Actions.Clean] = executor.Clean,
				[Actions.BuildArcology] = executor.BuildArcology,
				[Actions.ExpandPopulationCapacity] = executor.ExpandPopulationCapacity,
				[Actions.ExpandFoodCapacity] = executor.ExpandFoodCapacity,
				[Actions.WeAreReady] = executor.WeAreReady
			};

			//Executing action chosen as a method argument
			request = actionsToExecute[chosenAction].Invoke(login, token, parameter);
			

			IRestResponse response = client.Execute(request);
			return response.StatusCode;

		}
	}

}