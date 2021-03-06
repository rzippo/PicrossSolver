﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using PicrossSolverLibrary;

namespace PicrossSolverWPF
{
    /// <summary>
    /// Interaction logic for PicrossBoardView.xaml
    /// </summary>
    public partial class PicrossBoardView : UserControl
    {
        public PicrossBoard Board
        {
            get { return (PicrossBoard)GetValue(BoardProperty); }
            set {
                SetValue(BoardProperty, value);
                ReloadCellsView();
            }
        }

        public static readonly DependencyProperty BoardProperty =
            DependencyProperty.Register(
                "Board", 
                typeof(PicrossBoard), 
                typeof(PicrossBoardView),
                new PropertyMetadata(
                    PicrossBoard.GetEmpty()
                )
            );

        public ObservableCollection<PicrossCellViewModel> CellsView
        {
            get { return (ObservableCollection<PicrossCellViewModel>)GetValue(CellsViewProperty); }
            set { SetValue(CellsViewProperty, value); }
        }

        public static readonly DependencyProperty CellsViewProperty =
            DependencyProperty.Register(
                "CellsView",
                typeof(ObservableCollection<PicrossCellViewModel>),
                typeof(PicrossBoardView),
                new PropertyMetadata(new ObservableCollection<PicrossCellViewModel>())
                );

        private PicrossPuzzle puzzle;
        public PicrossPuzzle Puzzle {
            get => puzzle;
            set
            {
                puzzle = value;
                Board = new PicrossBoard(puzzle);
            }
        }

        public PicrossBoardView()
        {
            InitializeComponent();
            ReloadCellsView();
        }

        private void ReloadCellsView()
        {
            CellsView.Clear();
            for (int rowIndex = 0; rowIndex < Board.RowCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < Board.ColumnCount; columnIndex++)
                {
                    CellsView.Add(new PicrossCellViewModel(Board.Matrix[rowIndex, columnIndex]));
                }
            }
        }
    }
}
