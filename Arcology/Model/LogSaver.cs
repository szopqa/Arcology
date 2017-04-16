using Arcology.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using Arcology.Model.Communication;

namespace Arcology.Model {
	public class LogSaver {

		//List containing all results objects. Passed to saving method
		private List<Result> LogsSaved = new List<Result>();
		//Number of items in Events list. Created to show only new Events for each action
		private int _currentItemsInList;

		
		/// <summary>
		/// Files are saved to User\AppData\Arcology folder
		/// </summary>
		public string XmlLogsPath { get; set; }

		//Constructor
		public LogSaver() {

			XmlLogsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			if (!Directory.Exists(XmlLogsPath + "\\Arcology")) {
				Directory.CreateDirectory(XmlLogsPath + "\\Arcology");
			}

			_currentItemsInList = 0;

		}


		/// <summary>
		/// Saves all logs to XML file
		/// </summary>
		public void SaveLogsToXmlFile() {
			SaveLogs(this.LogsSaved);
		}


		/// <summary>
		/// Adds result object to List
		/// </summary>
		public void AddActionToLogsList(Result result) {
			Result res = new Result();
			res = result;
			this.LogsSaved.Add(res);
		}


		/// <summary>
		/// Removes all objects from List
		/// </summary>
		public void CleanLogsList() {
			this.LogsSaved.Clear();
		}


		/// <summary>
		/// Creates xml logs file in User\AppData\Arcology folder
		/// </summary>
		private void CreateFileIfItDoesntExist() {

			if (!File.Exists(XmlLogsPath + "\\Arcology\\Logs.xml")) {

				XmlTextWriter xWrite = new XmlTextWriter(XmlLogsPath + "\\Arcology\\Logs.xml", Encoding.UTF8);
				//Creating main <Logs> node
				xWrite.WriteStartElement("Logs");
				xWrite.WriteEndElement();
				xWrite.Close();

			}

		}


		/// <summary>
		/// Selects events without "Changed" word, 
		/// as they are considered useless by me
		/// </summary>
		private void GetImportantEvents(Result result) {

			var filteredEvents = new List<string>();

			foreach (string singleEvent in result.Events) {

				if ( !singleEvent.Contains("Changed") ) {
					filteredEvents.Add(singleEvent);
				}

			}

			result.Events = filteredEvents;

			GetOnlyNewEvents(result);
		}

		/// <summary>
		/// Gets only new events, that appeared in each round
		/// </summary>
		private void GetOnlyNewEvents(Result result) {

			var newEvents = new List<string>();

			for (int i = _currentItemsInList; i < result.Events.Count; i++) {
				newEvents.Add(result.Events[i]);
			}

			this._currentItemsInList = result.Events.Count;

			
			result.Events = newEvents;

		}


		/// <summary>
		/// Handles XML save. Creates proper Nodes and adds items from Result's object List
		/// </summary>
		private void SaveLogs(List<Result> results) {

			CreateFileIfItDoesntExist();	

			XmlDocument xmlLogs = new XmlDocument();
			xmlLogs.Load(XmlLogsPath + "\\Arcology\\Logs.xml");

			
			//Removes previous logs from file
			XmlNode xNode = xmlLogs.SelectSingleNode("Logs");
			xNode?.RemoveAll();


			foreach (Result actionResult in results) {

				//Main node showing all details for each round
				XmlNode xmlAction = xmlLogs.CreateElement("Action");

				//Child nodes
				XmlNode xmlTurn = xmlLogs.CreateElement("Turn");
				XmlNode xmlActionUsed = xmlLogs.CreateElement("ActionUsed");
				XmlNode xmlParameter = xmlLogs.CreateElement("Prarameter");
				XmlNode xmlEventScore = xmlLogs.CreateElement("EventScore");
				XmlNode xmlArcologyScore = xmlLogs.CreateElement("ArcologyScore");
				XmlNode xmlTotalScore = xmlLogs.CreateElement("TotalScore");
				XmlNode xmlRunes = xmlLogs.CreateElement("Runes");
				XmlNode xmlFoodQuantity = xmlLogs.CreateElement("FoodQuantity");
				XmlNode xmlWaste = xmlLogs.CreateElement("Waste");
				XmlNode xmlSocialCapital = xmlLogs.CreateElement("SocialCapital");
				XmlNode xmlProduction = xmlLogs.CreateElement("Production");
				XmlNode xmlFoodCapacity = xmlLogs.CreateElement("FoodCapacity");
				XmlNode xmlIntegrity = xmlLogs.CreateElement("Integrity");
				XmlNode xmlPopulation = xmlLogs.CreateElement("Population");
				XmlNode xmlPopulationCapacity = xmlLogs.CreateElement("PopulationCapacity");
				XmlNode xmlEvents = xmlLogs.CreateElement("NewEvents");


				xmlTurn.InnerText = actionResult.Turn.ToString();
				xmlActionUsed.InnerText = actionResult.Action.ToString();

				if (actionResult.Action != Actions.Produce)
					xmlParameter.InnerText = actionResult.Parameter;

				xmlEventScore.InnerText = actionResult.EventScore.ToString();
				xmlArcologyScore.InnerText = actionResult.ExperimentScore.ToString();
				xmlTotalScore.InnerText = actionResult.TotalScore.ToString();
				try {
					xmlRunes.InnerText = string.Join(" | ", actionResult.NehoRunes.ToArray());
				}
				catch (Exception ex) {
					xmlRunes.InnerText = "";
				}
				xmlFoodQuantity.InnerText = actionResult.FoodQuantity.ToString();
				xmlWaste.InnerText = actionResult.Waste.ToString();
				xmlSocialCapital.InnerText = actionResult.SocialCapital.ToString();
				xmlProduction.InnerText = actionResult.Production.ToString();
				xmlFoodCapacity.InnerText = actionResult.FoodCapacity.ToString();
				xmlIntegrity.InnerText = actionResult.ArcologyIntegrity.ToString();
				xmlPopulation.InnerText = actionResult.Population.ToString();
				xmlPopulationCapacity.InnerText = actionResult.PopulationCapacity.ToString();
				try {
					GetImportantEvents(actionResult);
					xmlEvents.InnerText = string.Join(" | ", actionResult.Events.ToArray());
				}
				catch ( Exception ex ) {
					xmlRunes.InnerText = "";
				}


				xmlAction.AppendChild(xmlTurn);
				xmlAction.AppendChild(xmlActionUsed);
				xmlAction.AppendChild(xmlParameter);
				xmlAction.AppendChild(xmlEventScore);
				xmlAction.AppendChild(xmlArcologyScore);
				xmlAction.AppendChild(xmlTotalScore);
				xmlAction.AppendChild(xmlFoodQuantity);
				xmlAction.AppendChild(xmlFoodCapacity);
				xmlAction.AppendChild(xmlPopulation);
				xmlAction.AppendChild(xmlPopulationCapacity);
				xmlAction.AppendChild(xmlIntegrity);
				xmlAction.AppendChild(xmlProduction);
				xmlAction.AppendChild(xmlSocialCapital);
				xmlAction.AppendChild(xmlWaste);
				xmlAction.AppendChild(xmlRunes);
				xmlAction.AppendChild(xmlEvents);

				try {
					xmlLogs.DocumentElement.AppendChild(xmlAction);
				}
				catch (NullReferenceException) {
					MessageBox.Show("Save failed");
				}
			}


			xmlLogs.Save(XmlLogsPath + "\\Arcology\\Logs.xml");
			this._currentItemsInList = 0;
		}
	}
}