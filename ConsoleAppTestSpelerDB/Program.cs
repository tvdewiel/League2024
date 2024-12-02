using LeagueBL.Interfaces;
using LeagueBL.Managers;
using LeagueBL.Model;
using LeagueDL;

namespace ConsoleAppTestSpelerDB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            string conn = @"Data Source=NB21-6CDPYD3\SQLEXPRESS;Initial Catalog=League2024_1F;Integrated Security=True;Trust Server Certificate=True";
            ISpelerRepository repo = new SpelerRepositoryADO(conn);
            SpelerManager sm = new SpelerManager(repo);
            //Speler s=sm.RegistreerSpeler("jos", null, 100);
            ITeamRepository repository = new TeamRepositoryADO(conn);
            TeamManager tm = new TeamManager(repository);
            //tm.RegistreerTeam(1000, "Team X", "xsers");
            //tm.RegistreerTeam(100, "Team Y",null);
            //var t = tm.SelecteerTeam(1000);
            //var s=sm.SelecteerSpelers(null,"luke");
            var t = tm.SelecteerTeams();
        }
    }
}
