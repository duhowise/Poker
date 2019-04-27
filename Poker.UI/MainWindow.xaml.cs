using System;
using System.Linq;
using System.Windows;
using Microsoft.Win32;

namespace Poker.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ConfigurationPoker _configurationPoker;
        string _xml;

        public MainWindow()
        {
            InitializeComponent();
            SaveButton.Click += SaveButton_Click;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ConnUsername.Text) || !string.IsNullOrWhiteSpace(ServerName.Text) ||
                !string.IsNullOrWhiteSpace(Password.Text))
            {
              // ReSharper disable once StringLiteralTypo
                var ConnectionString = $"Data Source={ServerName.Text},1433;Network Library=DBMSSOCN;Initial Catalog = VotingSystemV2; User ID = {ConnUsername.Text}; Password ={Password.Text};";
                _configurationPoker.SetConnectionString(ConnectionList.SelectedItem.ToString(),ConnectionString);
                _xml = _configurationPoker.RenderXml();
            }
            else
            {
                MessageBox.Show("all fields are required before save");
            }
        }

        private void LoadFileB_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog() {DefaultExt = ".config"};
            if (openFileDialog.ShowDialog() != true) return;
            try
            {
                FileName.Content += openFileDialog.SafeFileName;
                _xml = AppConfig.LoadXml(openFileDialog.FileName);

                _configurationPoker = new ConfigurationPoker(_xml);
                var connectionStringNames = _configurationPoker.GetConnectionStringNames();
                ConnectionList.ItemsSource = connectionStringNames;
                ConnectionList.Text = connectionStringNames.FirstOrDefault() ?? "";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
