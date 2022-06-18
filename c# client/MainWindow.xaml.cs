using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using Grpc.Net.Client;
using System.Windows.Controls.DataVisualization.Charting;
using System.IO;

namespace Waterlevel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window //fehlt: achsenbeschriftungen
    {
        public RequestHandler oRequestHandler;
        public List<ClientRequest> lClientRequest;
        public static MainWindow _instance;

        public MainWindow()
        {
            _instance = this; //create an instance to get the request data async
            InitializeComponent();
            oRequestHandler= new RequestHandler(this);
            lClientRequest= new List<ClientRequest>();
            this.DataContext= oRequestHandler;
            oRequestHandler.doRequest(); //start gRPC Request
            
        }

        public void requestData()
        {
            lClientRequest = oRequestHandler.GetClientData(); //put the data into list and load in chart
            LoadLineChart();
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void LoadLineChart()
        {
            var list = new List<KeyValuePair<DateTime, double>>();
            for (int i = 0; i < lClientRequest.Count; i++)
            {
                list.Add(new KeyValuePair<DateTime, double>(lClientRequest[i].dateTime, lClientRequest[i].value));
            }
            ((LineSeries)WaterLevelChart.Series[0]).ItemsSource = list;
        }

        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            oRequestHandler.doRequest();
        }

        private void btn_table_Click(object sender, RoutedEventArgs e)
        {
            WaterLevelChart.Visibility = Visibility.Hidden;
            btn_graph.Visibility = Visibility.Visible;
            btn_table.Visibility = Visibility.Hidden;
            lbx_data.Visibility = Visibility.Visible;
            lbx_data.ItemsSource = lClientRequest;
            lbx_data.Items.Refresh();
        }
        private void btn_graph_Click(object sender, RoutedEventArgs e)
        {
            WaterLevelChart.Visibility = Visibility.Visible;
            btn_graph.Visibility = Visibility.Hidden;
            btn_table.Visibility = Visibility.Visible;
            lbx_data.Visibility = Visibility.Hidden;
        }

        private void btn_download_Click(object sender, RoutedEventArgs e)
        {
            exportToCSV(lClientRequest);
        }
        public static void exportToCSV(List<ClientRequest> lClientRequest)
        {
            try
            {
                string headerline = string.Join(";", lClientRequest[0].GetType().GetProperties().Select(p => p.Name));
                var dataLines = from val in lClientRequest
                                let dataLine = string.Join(";", val.GetType().GetProperties().Select(p => p.GetValue(val)))
                                select dataLine;
                var csvData = new List<string>();
                csvData.Add(headerline);
                csvData.AddRange(dataLines);

                string csvFilePath = @"C:\Users\Nancy\Documents\HTW\Semester4\Verteilte Systeme und Komponenten\Waterlevellist.csv";
                File.WriteAllLines(csvFilePath, csvData);
                MessageBox.Show("Die Datei wurde erfolgreich exportiert.");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error: ", ex);
            }
        }
    }
}
