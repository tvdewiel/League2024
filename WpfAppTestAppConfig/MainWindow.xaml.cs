using LeagueBL.Interfaces;
using LeagueBL.Managers;
using LeagueDL;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppTestAppConfig
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TeamManager teamManager;
        private ITeamRepository teamRepository;
        private string user;
        public MainWindow()
        {
            InitializeComponent();
            user = ConfigurationManager.AppSettings["user"];
            teamRepository = new TeamRepositoryADO(ConfigurationManager.ConnectionStrings[user].ConnectionString);
            teamManager = new TeamManager(teamRepository);
            TeamsComboBox.ItemsSource = teamManager.SelecteerTeams();
            TeamsComboBox.SelectedIndex = 0;
        }
    }
}