using LeagueBL.DTO;
using LeagueBL.Interfaces;
using LeagueBL.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueDL
{
    public class TeamRepositoryADO : ITeamRepository
    {
        private string connectionString;

        public TeamRepositoryADO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool BestaatTeam(Team t)
        {
            string sql = "SELECT count(*) FROM team WHERE stamnummer=@stamnummer";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@stamnummer", t.Stamnummer);
                int n = (int)cmd.ExecuteScalar();
                if (n > 0) return true; else return false;
            }
        }

        public void SchrijfTeamInDB(Team t)
        {
            string sql = "INSERT INTO team(stamnummer,naam,bijnaam) VALUES(@stamnummer,@naam,@bijnaam)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@naam", t.Naam);
                cmd.Parameters.AddWithValue("@stamnummer", t.Stamnummer);
                cmd.Parameters.Add(new SqlParameter("@bijnaam", SqlDbType.NVarChar));
                if (t.Bijnaam == null) cmd.Parameters["@bijnaam"].Value = DBNull.Value;
                else cmd.Parameters["@bijnaam"].Value = t.Bijnaam;
                cmd.ExecuteNonQuery();
            }
        }

        public Team SelecteerTeam(int stamnummer)
        {
            string sql = "SELECT t1.stamnummer,t1.naam as ploegnaam,t1.bijnaam,t2.id,t2.naam,t2.rugnummer,t2.lengte,t2.gewicht FROM Team t1 left join speler t2 on t1.stamnummer=t2.teamId  where stamnummer=@stamnummer";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@stamnummer", stamnummer);
                Team team = null;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) 
                {
                    if (team == null)
                    {
                        team = new Team(stamnummer, (string)reader["ploegnaam"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("bijnaam"))) team.ZetBijnaam((string)reader["bijnaam"]);
                    }
                    if (!reader.IsDBNull(reader.GetOrdinal("id")))
                    {
                        int? lengte = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("lengte"))) lengte = (int)reader["lengte"];
                        int? gewicht = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("gewicht"))) gewicht = (int)reader["gewicht"];
                        Speler s = new Speler((int)reader["id"], (string)reader["naam"], lengte, gewicht);
                        if (!reader.IsDBNull(reader.GetOrdinal("rugnummer"))) s.ZetRugnummer((int)reader["rugnummer"]);
                        s.ZetTeam(team);
                    }
                }
                return team;
                
            }
        }

        public IReadOnlyList<TeamInfo> SelecteerTeams()
        {
            string sql = "SELECT * from team";
            List<TeamInfo> teams= new List<TeamInfo>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string bijnaam = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("bijnaam"))) bijnaam = (string)reader["bijnaam"];
                        teams.Add(new TeamInfo((int)reader["stamnummer"], (string)reader["naam"], bijnaam));
                    }
                }
            }
            return teams;
        }
    }
}
