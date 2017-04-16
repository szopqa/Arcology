using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Arcology.Pages {
	/// <summary>
	/// Interaction logic for SimulationPage.xaml
	/// </summary>
	public partial class SimulationPage : Page {

		//Constructor
		public SimulationPage () {
			InitializeComponent();
			this.SliderValueLbl.Content = "0";
		}
		

		/// <summary>
		/// Shows current Parameter value in label
		/// Shows predicted values
		/// </summary>
		public void SliderValueChanged ( object sender, RoutedPropertyChangedEventArgs<double> e ) {

			//Showing current slider value inside label
			int parameter = (int) this.ParameterSlider.Value;
			this.SliderValueLbl.Content = parameter;

			//Hides labels if slider value equals 0
			if (ParameterSlider.Value == 0) {

				PredictedFoodCapLbl.Visibility = Visibility.Hidden;
				PredictedIntegrityLbl.Visibility = Visibility.Hidden;
				PredictedPeopleCapLbl.Visibility = Visibility.Hidden;
				PredictedPopulationLbl.Visibility = Visibility.Hidden;
			}
			else {
				PredictedFoodCapLbl.Visibility = Visibility.Visible;
				PredictedIntegrityLbl.Visibility = Visibility.Visible;
				PredictedPeopleCapLbl.Visibility = Visibility.Visible;
				PredictedPopulationLbl.Visibility = Visibility.Visible;
			}											 

			//Getting current values from labels
			double foodCapacityValue = double.Parse(this.FoodCapacityLbl.Content.ToString());
			double peopleCapacityValue = double.Parse(this.PopulationCapacityLbl.Content.ToString());
			double integrityValue = double.Parse(this.IntegrityLbl.Content.ToString());
			double populationValue = double.Parse(this.PopulationLbl.Content.ToString());


			//Showing predicted values in labels
			this.PredictedFoodCapLbl.Content = (parameter + foodCapacityValue).ToString();
			this.PredictedIntegrityLbl.Content = (parameter + integrityValue).ToString();
			this.PredictedPeopleCapLbl.Content = (parameter + peopleCapacityValue).ToString();
			this.PredictedPopulationLbl.Content = (parameter + populationValue).ToString();

		}
		

	}
}
