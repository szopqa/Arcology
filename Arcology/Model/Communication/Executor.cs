using System;
using RestSharp;

namespace Arcology.Model.Communication {

	//Delegate for all methods written below
	public delegate RestRequest MakeRequest ( string login, string token, string parameter );


	public class Executor {


		private IRestRequest _request;
		
		//Constructor
		public Executor ( IRestRequest request ) {
			_request = request;
		}


		/*Methods returning json commands containing Command, Login, Token 
		 * and Parameter if needed for each action chosen to execute*/
		 
		public RestRequest Restart ( string login, string token, string parameter ) {

			return ( RestRequest ) _request.AddParameter("application/json",
														"{\"Command\": \"Restart\", \"Login\": \"" + login + "\", \"Token\": \"" + token + "\"}",
														ParameterType.RequestBody);
		}

		public RestRequest ImportFood ( string login, string token, string parameter ) {

			return ( RestRequest ) _request.AddParameter("application/json",
														"{ \"Command\":\"Import food\", \"Login\":\"" + login + "\", \"Token\":\"" 
														+ token + "\",\"Parameter\":\"" + parameter + "\"}",
														ParameterType.RequestBody);
		}

		public RestRequest Propaganda ( string login, string token, string parameter ) {

			return ( RestRequest ) _request.AddParameter("application/json",
														"{\"Command\": \"Propaganda\", \"Login\": \"" + login + "\", \"Token\": \""
														+ token + "\", \"Parameter\": \"" + parameter + "\"}",
														ParameterType.RequestBody);
		}

		public RestRequest Produce ( string login, string token, string parameter ) {

			return ( RestRequest ) _request.AddParameter("application/json",
														"{\"Command\": \"Produce\", \"Login\": \"" + login + "\", \"Token\": \"" + token + "\"}",
														ParameterType.RequestBody);
		}

		public RestRequest Clean ( string login, string token, string parameter ) {

			return ( RestRequest ) _request.AddParameter("application/json",
														"{\"Command\": \"Clean\", \"Login\": \"" + login + "\", \"Token\": \""
														+ token + "\", \"Parameter\": \"" + parameter + "\"}",
														ParameterType.RequestBody);
		}

		public RestRequest BuildArcology ( string login, string token, string parameter ) {

			return ( RestRequest ) _request.AddParameter("application/json",
														"{\"Command\": \"Build Arcology\", \"Login\": \"" + login + "\", \"Token\": \""
														+ token + "\", \"Parameter\": \"" + parameter + "\"}",
														ParameterType.RequestBody);
		}

		public RestRequest ExpandPopulationCapacity ( string login, string token, string parameter ) {

			return ( RestRequest ) _request.AddParameter("application/json",
														"{\"Command\": \"Expand Population Capacity\", \"Login\": \"" + login + "\", \"Token\": \""
														+ token + "\", \"Parameter\": \"" + parameter + "\"}",
														ParameterType.RequestBody);
		}

		public RestRequest ExpandFoodCapacity ( string login, string token, string parameter ) {

			return ( RestRequest ) _request.AddParameter("application/json",
														"{\"Command\": \"Expand Food Capacity\", \"Login\": \"" + login + "\", \"Token\": \""
														+ token + "\", \"Parameter\": \"" + parameter + "\"}",
														ParameterType.RequestBody);
		}

		public RestRequest WeAreReady ( string login, string token, string parameter ) {

			return ( RestRequest ) _request.AddParameter("application/json",
														"{\"Command\": \"We are ready\", \"Login\": \"" + login + "\", \"Token\": \""
														+ token + "\", \"Parameter\": \"" + parameter + "\"}",
														ParameterType.RequestBody);
		}


	}
}
