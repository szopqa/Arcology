using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcology.Model.Communication {

	/// <summary>
	/// All actions, used to invoke proper method 
	/// </summary>
	public enum Actions {

		Start,
		Restart,
		Produce,
		ImportFood,
		Propaganda,
		Clean,
		BuildArcology,
		ExpandPopulationCapacity,
		ExpandFoodCapacity,
		WeAreReady

	}
}
