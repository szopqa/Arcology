using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcology.Model.Communication;

namespace Arcology.Model {
	public class Result {

		public Actions Action { get; set; }
		public string Parameter { get; set; }
		public decimal Turn { get; set; }
		public string ShouldIRestartExperimentAndCry { get; set; }
		public bool IsTerminated { get; set; }
		public decimal EventScore { get; set; }
		public decimal ExperimentScore { get; set; }
		public decimal TotalScore { get; set; }
		public IList<string> NehoRunes { get; set; }
		public decimal FoodQuantity { get; set; }
		public decimal Waste { get; set; }
		public decimal SocialCapital { get; set; }
		public decimal Production { get; set; }
		public decimal FoodCapacity { get; set; }
		public decimal ArcologyIntegrity { get; set; }
		public decimal Population { get; set; }
		public decimal PopulationCapacity { get; set; }
		public IList<string> Events { get; set; }

	}
}
