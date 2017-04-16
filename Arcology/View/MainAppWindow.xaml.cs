using Arcology.Model;
using Arcology.Model.Communication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
	/// Interaction logic for MainAppWindow.xaml
	/// </summary>
	public partial class MainAppWindow : Page {


		public static SimulationPage SimulationPage = new SimulationPage();

		//private objects fields
		private ServerConnector connector;
		private LogSaver _logSaver;
		private Result _actionResult;


		//Constructor
		public MainAppWindow (Result lastSessionResults) {

			InitializeComponent();

			connector = new ServerConnector();
			_logSaver = new LogSaver();
			_actionResult = new Result();

			this._actionResult = lastSessionResults;

		}



		private void MainAppwindowOnload ( object sender, RoutedEventArgs e ) {

			//Showing SimulationPage on MainWindow's frame
			SimulationFrame.NavigationService.Navigate(SimulationPage);

			//Shows result from last session of simulation
			UpdateResults(this._actionResult);

		}



		/// <summary>
		/// Method executed after each Action method
		/// Updates page and checks if simulation can be continued 
		/// </summary>
		/// <param name="actionResult">Result's object returned by Describe function</param>
		public void UpdateResults(Result actionResult ) {

			//Adding 1st turn to logs
			AddFirstRoundToLogs(actionResult);
			
			//Updating labes
			UpdateResultLabels(actionResult);

			//Checking if arcology has survived
			CheckIfSimulationIsFinished(actionResult);
			
			//Checking if isTerminated value is true
			CheckIfIsTerminated(actionResult);		
			
			//Resets slider value whenever Action is executed
			SimulationPage.ParameterSlider.Value = 0;

		}



		#region privateMethods




		/// <summary>
		/// Updates info such as score etc. in simulation window after each action 
		/// </summary>
		private void UpdateResultLabels(Result actionResult) {

			SimulationPage.TurnLbl.Content = actionResult.Turn;
			SimulationPage.ScoreLbl.Content = actionResult.EventScore;

			SimulationPage.FoodQuantityLbl.Content = String.Format("{0:0.00}",actionResult.FoodQuantity);
			SimulationPage.PopulationLbl.Content = String.Format("{0:0.00}", actionResult.Population);
			SimulationPage.WasteLbl.Content = String.Format("{0:0.00}", actionResult.Waste);
			SimulationPage.ProductionLbl.Content = String.Format("{0:0.00}", actionResult.Production);
			SimulationPage.FoodCapacityLbl.Content = String.Format("{0:0.00}", actionResult.FoodCapacity);
			SimulationPage.PopulationCapacityLbl.Content = String.Format("{0:0.00}", actionResult.PopulationCapacity);
			SimulationPage.IntegrityLbl.Content = String.Format("{0:0.00}", actionResult.ArcologyIntegrity);
			SimulationPage.SocialCapitalLbl.Content = String.Format("{0:0.00}", actionResult.SocialCapital);
			try {

				SimulationPage.NehoRunes.Content = string.Join(" | ", actionResult.NehoRunes.ToArray());

			}
			catch ( ArgumentNullException ) {
				SimulationPage.NehoRunes.Content = "";
			}


			//Adding new events to EventList
			UpdateEventsList(actionResult);

		}

		/// <summary>
		/// Adds First round to logs with "Start" Action
		/// </summary>
		private void AddFirstRoundToLogs(Result result) {
			if ( result.Turn == 1 ) {
				result.Action = Actions.Start;
				_logSaver.AddActionToLogsList(result);
			}

		}


		/// <summary>
		/// Adds new events to list. Selecting only those without "Changed" word 
		/// </summary>
		private void UpdateEventsList ( Result result ) {

			SimulationPage.EventsList.Items.Clear();
				
			foreach ( var newEvent in result.Events ) {

				//Getting rid of useless events
				if (!newEvent.Contains("Changed")) {
					
					//Adding new Events at the top of the ListBox
					SimulationPage.EventsList.Items.Insert(0,newEvent);

				}


			}

		}



		/// <summary>
		/// Ends simulation when isTerminated equals true
		/// Saves logs to file
		/// </summary>
		private void CheckIfIsTerminated(Result actionResult) {

			if (actionResult.IsTerminated && actionResult.TotalScore == 0) {
				MessageBox.Show("Arcology has collapsed!\n Click Restart button!");

				_logSaver.SaveLogsToXmlFile();
			}

		}



		/// <summary>
		/// Saves result. Shows score
		/// </summary>
		private void CheckIfSimulationIsFinished(Result actionResult) {

			if (actionResult.TotalScore > 0) {

				_logSaver.SaveLogsToXmlFile();

				MessageBox.Show("You did it! Your Scores : \n" +
				                "Arcology score : " + actionResult.ExperimentScore + "\n" +
				                "Event score : " + actionResult.EventScore + "\n" +
				                "Total score : " + actionResult.TotalScore);
				
				
			}

		}



		/// <summary>
		/// Gets current slider value. 
		/// Returns null when user didnt set the parameter value on slider
		/// Actions thet require parameter wont be executed
		/// </summary>
		private string getSliderValue() {

			int roundedSliderValue = (int) SimulationPage.ParameterSlider.Value;

			if (roundedSliderValue == 0) {
				MessageBox.Show("Select parameter value on slider!");
				return null;
			}

			return roundedSliderValue.ToString();

		}


		/// <summary>
		/// Shows error dialog when return code is 500
		/// </summary>
		private void ShowConnectionErrorDialog () {

			UpdateResultLabels(this._actionResult);
			MessageBox.Show("Connection error!");

		}
		
		
		
		#endregion

		
		
		
		
		#region ArcologyActions
		
		/*Click action for each button executing proper Action*/


		private void ImportFoodClick ( object sender, RoutedEventArgs e ) {

			if (getSliderValue() != null) {
				
				var statusCode = connector.ExecuteAction(getSliderValue(), Actions.ImportFood);
				this._actionResult = connector.Describe();

				if (statusCode == HttpStatusCode.OK || this._actionResult.IsTerminated == true ) {

					_actionResult.Action = Actions.ImportFood;
					_actionResult.Parameter = getSliderValue();

					_logSaver.AddActionToLogsList(this._actionResult);
				
					UpdateResults(_actionResult);

				}
				else {
				
					ShowConnectionErrorDialog();
				
				}

			}

				
			
		}


		private void ProduceClick ( object sender, RoutedEventArgs e ) {

			var statusCode = connector.ExecuteAction("", Actions.Produce);
			this._actionResult = connector.Describe();

			if (statusCode == HttpStatusCode.OK || this._actionResult.IsTerminated == true) {

				_actionResult.Action = Actions.Produce;

				_logSaver.AddActionToLogsList(this._actionResult);

				UpdateResults(_actionResult);

			}
			else {
				
				ShowConnectionErrorDialog();
			
			}

		}

		private void PropagandaClick ( object sender, RoutedEventArgs e ) {

			if (getSliderValue() != null) {

				var statusCode = connector.ExecuteAction(getSliderValue(), Actions.Propaganda);
				this._actionResult = connector.Describe();

				if (statusCode == HttpStatusCode.OK || this._actionResult.IsTerminated == true) {

					_actionResult.Action = Actions.Propaganda;
					_actionResult.Parameter = getSliderValue();

					_logSaver.AddActionToLogsList(this._actionResult);

					UpdateResults(_actionResult);
				}
				else {

					ShowConnectionErrorDialog();

				}
			}
		}

		private void CleanClick ( object sender, RoutedEventArgs e ) {

			if (getSliderValue() != null) {

				var statusCode = connector.ExecuteAction(getSliderValue(), Actions.Clean);
				this._actionResult = connector.Describe();

				if (statusCode == HttpStatusCode.OK || this._actionResult.IsTerminated == true) {

					_actionResult.Action = Actions.Clean;
					_actionResult.Parameter = getSliderValue();

					_logSaver.AddActionToLogsList(this._actionResult);

					UpdateResults(_actionResult);

				}
				else {

					ShowConnectionErrorDialog();

				}

			}
		}

		private void BuildArcologyClick ( object sender, RoutedEventArgs e ) {

			if (getSliderValue() != null) {

				var statusCode = connector.ExecuteAction(getSliderValue(), Actions.BuildArcology);
				this._actionResult = connector.Describe();

				if (statusCode == HttpStatusCode.OK || this._actionResult.IsTerminated == true) {

					_actionResult.Action = Actions.BuildArcology;
					_actionResult.Parameter = getSliderValue();

					_logSaver.AddActionToLogsList(this._actionResult);

					UpdateResults(_actionResult);
				}
				else {

					ShowConnectionErrorDialog();

				}

			}
		}

		private void ExpandFoodClick ( object sender, RoutedEventArgs e ) {

			if (getSliderValue() != null) {

				var statusCode = connector.ExecuteAction(getSliderValue(), Actions.ExpandFoodCapacity);
				this._actionResult = connector.Describe();

				if (statusCode == HttpStatusCode.OK || this._actionResult.IsTerminated == true) {

					_actionResult.Action = Actions.ExpandFoodCapacity;
					_actionResult.Parameter = getSliderValue();

					_logSaver.AddActionToLogsList(this._actionResult);

					UpdateResults(_actionResult);
				}
				else {

					ShowConnectionErrorDialog();

				}
			}

		}

		private void ExpandPopulationClick ( object sender, RoutedEventArgs e ) {

			if (getSliderValue() != null) {

				var statusCode = connector.ExecuteAction(getSliderValue(), Actions.ExpandPopulationCapacity);
				this._actionResult = connector.Describe();

				if (statusCode == HttpStatusCode.OK || this._actionResult.IsTerminated == true) {

					_actionResult.Action = Actions.ExpandPopulationCapacity;
					_actionResult.Parameter = getSliderValue();

					_logSaver.AddActionToLogsList(this._actionResult);

					UpdateResults(_actionResult);
				}
				else {

					ShowConnectionErrorDialog();

				}
			}

		}

		private void WeAreReadyClick ( object sender, RoutedEventArgs e ) {

			if (getSliderValue() != null) {

				var statusCode = connector.ExecuteAction(getSliderValue(), Actions.WeAreReady);
				this._actionResult = connector.Describe();

				if (statusCode == HttpStatusCode.OK || this._actionResult.IsTerminated == true) {

					_actionResult.Action = Actions.WeAreReady;
					_actionResult.Parameter = getSliderValue();

					_logSaver.AddActionToLogsList(this._actionResult);

					UpdateResults(_actionResult);
				}
				else {

					ShowConnectionErrorDialog();

				}

			}
		}


		private void RestartBtn_OnClick(object sender, RoutedEventArgs e) {

			_logSaver.SaveLogsToXmlFile();
			var statusCode = connector.ExecuteAction("", Actions.Restart);

			if (statusCode == HttpStatusCode.OK) {

				//Cleans all previous logs saved
				_logSaver.CleanLogsList();
				//Cleans all previous events 
				SimulationPage.EventsList.Items.Clear();

				//Adding Start action as first element in logs file
				this._actionResult = connector.Describe();
				

				UpdateResults(_actionResult);

			}
			else {
				ShowConnectionErrorDialog();
			}

		}

		#endregion

		
	}
}
