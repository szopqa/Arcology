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
using Arcology.Model;
using Arcology.Model.Communication;
using System.Windows.Media.Animation;

namespace Arcology.Pages {
	/// <summary>
	/// Interaction logic for StartPage.xaml
	/// </summary>
	public partial class StartPage : Page {

		private ServerConnector connector = new ServerConnector();
		

		public StartPage () {
			InitializeComponent();
			this.loginBox.Text = ServerConnector.login.ToString();
			this.passwordBox.Text = ServerConnector.token.ToString();
		}

		/// <summary>
		/// Loads last session. If there is no internet connection application will not start
		/// </summary>
		private void StartButtonClick ( object sender, RoutedEventArgs e ) {

			Result lastSession = new Result();
			
			lastSession = connector.Describe();

			if ( lastSession != null ) {
				this.NavigationService.Navigate(new MainAppWindow(lastSession));
			}
			else {
				MessageBox.Show("Connection Error!");
			}

		}
	}
}
