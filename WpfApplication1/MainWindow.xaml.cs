using HeyThatsMyFishSolver;
using HeyThatsMyFishWpf.ViewModel;
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

namespace HeyThatsMyFishWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            ListViewItem listViewItem = (ListViewItem)sender;
            MoveScore moveScore = (MoveScore)listViewItem.Content;
            var viewModel = DataContext as MainViewModel;
            viewModel.HighlightMove(moveScore.Move);
        }

        public void ListViewItem_MouseLeave(object sender, MouseEventArgs e)
        {
            ListViewItem listViewItem = (ListViewItem)sender;
            MoveScore moveScore = (MoveScore)listViewItem.Content;
            var viewModel = DataContext as MainViewModel;
            viewModel.UnhighlightMove(moveScore.Move);
        }
    }
}
