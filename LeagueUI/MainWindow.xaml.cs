﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LeagueUI
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

        private void UpdateSpeler_ButtonClick(object sender, RoutedEventArgs e)
        {
            WindowUpdateSpeler w = new WindowUpdateSpeler();
            w.ShowDialog();
        }

        private void UpdateTeam_ButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}