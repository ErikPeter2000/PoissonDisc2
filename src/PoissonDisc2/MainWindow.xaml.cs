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

namespace PoissonDisc2UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel Logic => DataContext as MainWindowViewModel;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btRender_Click(object sender, RoutedEventArgs e)
        {
            bool success = true;
            success &= int.TryParse(tbxWidth.Text, out int width);
            success &= int.TryParse(tbxHeight.Text, out int height);
            if (!success)
            {
                MessageBox.Show("Invalid input");
                return;
            }
            Logic.Render(width, height);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) => Logic.OnLoad();

        private void btAdd_Click(object sender, RoutedEventArgs e) => Logic.AddPass();

        private void btDelete_Click(object sender, RoutedEventArgs e) => Logic.RemovePass();

        private void btExport_Click(object sender, RoutedEventArgs e) => Logic.ExportBitmap();
    }
}
