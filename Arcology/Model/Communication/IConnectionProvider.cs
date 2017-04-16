using System.Net;

namespace Arcology.Model.Communication {
	interface IConnectionProvider {
		Result Describe ();
		HttpStatusCode ExecuteAction ( string parameter, Actions action );
	}
}

