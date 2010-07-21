using System.Windows;
using Microsoft.Win32;

namespace BehaveN.GUI
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var d = new OpenFileDialog();

            if (d.ShowDialog().GetValueOrDefault())
            {
                string fileName = d.FileName;
                MessageBox.Show("You selected " + fileName + "!");
            }
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
