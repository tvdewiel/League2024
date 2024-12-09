using LeagueBL.DTO;
using LeagueBL.Managers;
using LeagueDL;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Interaction logic for WindowUpdateSpeler.xaml
    /// </summary>
    public partial class WindowUpdateSpeler : Window
    {
        private SpelerManager spelerManager;
        public WindowUpdateSpeler()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["LeagueDB2024"].ConnectionString;
            spelerManager = new SpelerManager(new SpelerRepositoryADO(connectionString));
        }

        private void UpdateSpelerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ZoekSpelerButton_Click(object sender, RoutedEventArgs e)
        {
            int? spelerId = null;
            string naam = null;
            if (!string.IsNullOrWhiteSpace(ZoekNaamTextBox.Text)) naam=ZoekNaamTextBox.Text;
            if (!string.IsNullOrWhiteSpace(ZoekSpelerIDTextBox.Text))
            {
                spelerId=int.Parse(ZoekSpelerIDTextBox.Text);
            }
            List<SpelerInfo> spelers= (List<SpelerInfo>)spelerManager.SelecteerSpelers(spelerId, naam);
            if (spelers.Count == 0)
            {
                SpelerIDTextBox.Text = "";
                SpelerNaamTextBox.Text = "";
                TeamTextBox.Text = "";
                RugnummerTextBox.Text = "";
                GewichtTextBox.Text = "";
                LengteTextBox.Text = "";
            }
            if (spelers.Count == 1)
            {
                SpelerIDTextBox.Text = spelers[0].Id.ToString();
                SpelerNaamTextBox.Text = spelers[0].Naam;
                if (spelers[0].Team != null) TeamTextBox.Text = spelers[0].Team.ToString();
                else TeamTextBox.Text = "";
                RugnummerTextBox.Text = spelers[0].Rugnummer.ToString();
                GewichtTextBox.Text = spelers[0].Gewicht.ToString();
                LengteTextBox.Text = spelers[0].Lengte.ToString();
            }
            if (spelers.Count > 1)
            {
                WindowSelecteerSpeler selecteerSpelerWindow=new WindowSelecteerSpeler(spelers);
                if (selecteerSpelerWindow.ShowDialog() == true)
                {
                    SpelerIDTextBox.Text = selecteerSpelerWindow.SelectedSpeler.Id.ToString();
                    SpelerNaamTextBox.Text = selecteerSpelerWindow.SelectedSpeler.Naam;
                    TeamTextBox.Text = selecteerSpelerWindow.SelectedSpeler.Team.ToString();
                    RugnummerTextBox.Text = selecteerSpelerWindow.SelectedSpeler.Rugnummer.ToString();
                    GewichtTextBox.Text = selecteerSpelerWindow.SelectedSpeler.Gewicht.ToString();
                    LengteTextBox.Text = selecteerSpelerWindow.SelectedSpeler.Lengte.ToString();
                }
            }
        }
    }
}
