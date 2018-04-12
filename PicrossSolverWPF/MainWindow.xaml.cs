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
        public PicrossBoard Board { get; set; }
        public PicrossPuzzle Puzzle { get; private set; }

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
                        Puzzle = JsonConvert.DeserializeObject<PicrossPuzzle>(json);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error while opening json");
            }

            Board = new PicrossBoard(Puzzle);
            LoadPicrossGrid();
        }

        private void LoadPicrossGrid()
        {
            BuildMatrix();

            //todo: add visual representation
            // https://stackoverflow.com/questions/25004713/creating-a-grid-filled-with-rectangles

            MessageBox.Show("Ok");
        }

        private void BuildMatrix()
        {
            ViewMatrix = new PicrossCellView[Board.RowCount][];
            for (int rowIndex = 0; rowIndex < Board.ColumnCount; rowIndex++)
            {
                var row = new PicrossCellView[Board.ColumnCount];
                for (int columnIndex = 0; columnIndex < Board.RowCount; columnIndex++)
                {
                    row[columnIndex] = new PicrossCellView(Board.Matrix[rowIndex, columnIndex]);
                }
                ViewMatrix[rowIndex] = row;
            }
        }

        private void Solve_Click(object sender, RoutedEventArgs e)
        {
            Board.DebugSolve();
            MessageBox.Show("Ok");
        }
    }
}
