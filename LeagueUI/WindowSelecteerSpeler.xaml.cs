using LeagueBL.DTO;
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
using System.Windows.Shapes;

namespace LeagueUI
{
    /// <summary>
    /// Interaction logic for WindowSelecteerSpeler.xaml
    /// </summary>
    public partial class WindowSelecteerSpeler : Window
    {
        public SpelerInfo? SelectedSpeler=null;
        public WindowSelecteerSpeler(List<SpelerInfo> spelers)
        {
            InitializeComponent();
            SpelersListBox.ItemsSource = spelers;
        }

        private void SpelersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedSpeler = SpelersListBox.SelectedItem as SpelerInfo;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
