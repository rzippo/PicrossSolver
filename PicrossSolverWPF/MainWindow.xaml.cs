using System;
using System.Collections.Generic;
using System.IO;
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
using Newtonsoft.Json;
using PicrossSolverLibrary;


namespace PicrossSolverWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PicrossCellView[][] ViewMatrix { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void JsonPathSelector_OnClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Json files(*.json)|*.json",
                Multiselect = false
            };

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                JsonPath.Text = openFileDialog.FileName;
            }
        }

        private void Load_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (FileStream fs = new FileStream(JsonPath.Text, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string json = sr.ReadToEnd();
                        PicrossPuzzle puzzle = JsonConvert.DeserializeObject<PicrossPuzzle>(json);
                        BoardView.Puzzle = puzzle;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error while opening json");
            }
        }

        private void Solve_Click(object sender, RoutedEventArgs e)
        {
            BoardView.Board.Solve();
            MessageBox.Show("Solve completed");
        }
    }
}
