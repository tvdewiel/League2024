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
    public class SpelerRepositoryADO : ISpelerRepository
    {
        private string connectionString;

        public SpelerRepositoryADO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool BestaatSpeler(Speler s)
        {
            string sql = "SELECT count(*) FROM speler WHERE naam=@naam";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@naam", s.Naam);
                int n = (int)cmd.ExecuteScalar();
                if (n > 0) return true; else return false;
            }
        }

        public bool BestaatSpeler(int id)
        {
            string sql = "SELECT count(*) FROM speler WHERE id=@id";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@id",id);
                int n = (int)cmd.ExecuteScalar();
                if (n > 0) return true; else return false;
            }
        }

        public void SchrijfSpelerInDB(Speler s)
        {
            string sql = "INSERT INTO speler(naam,lengte,gewicht) output INSERTED.ID VALUES(@naam,@lengte,@gewicht)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@naam", s.Naam);
                cmd.Parameters.Add(new SqlParameter("@lengte", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@gewicht", SqlDbType.Int));
                if (s.Lengte == null) cmd.Parameters["@lengte"].Value = DBNull.Value;
                else cmd.Parameters["@lengte"].Value = s.Lengte;
                if (s.Gewicht == null) cmd.Parameters["@gewicht"].Value = DBNull.Value;
                else cmd.Parameters["@gewicht"].Value = s.Gewicht;
                int newID = (int)cmd.ExecuteScalar();
                s.ZetId(newID);
            }
        }

        public Speler SelecteerSpeler(int id)
        {
            string sql = "select t1.id spelerid,t1.naam spelernaam,t1.rugnummer spelerrugnummer,t1.lengte spelerlengte,t1.gewicht spelergewicht,\r\n       t2.naam teamnaam,t2.stamnummer,t2.bijnaam,t3.id,t3.naam,t3.rugnummer,t3.lengte,t3.gewicht \r\nfrom speler t1 left join team t2 on t1.teamId=t2.stamnummer\r\nleft join speler t3 on t3.teamId=t2.stamnummer\r\nwhere t1.id=@id";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue ("@id", id);
                IDataReader reader=cmd.ExecuteReader();
                Speler speler = null;
                Team team = null;
                while (reader.Read())
                {
                    if (speler == null)
                    {
                        //maak speler
                        int? lengte = null;
                        int? gewicht = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("spelerlengte"))) lengte = (int?)reader["spelerlengte"];
                        if (!reader.IsDBNull(reader.GetOrdinal("spelergewicht"))) gewicht = (int?)reader["spelergewicht"];
                        speler=new Speler(id,(string)reader["spelernaam"],lengte, gewicht);
                        if (!reader.IsDBNull(reader.GetOrdinal("spelerrugnummer"))) speler.ZetRugnummer((int)reader["spelerrugnummer"]);                        
                        if (reader.IsDBNull(reader.GetOrdinal("stamnummer"))) return speler;
                    }
                    if (team == null)
                    {
                        //maak team                       
                        team = new Team((int)reader["stamnummer"], (string)reader["teamnaam"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("bijnaam"))) 
                            team.ZetBijnaam((string)reader["bijnaam"]);
                        speler.ZetTeam(team);
                    }
                    if (((int)reader["id"])!=id)
                    {
                        int? lengte = null;
                        int? gewicht = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("spelerlengte"))) lengte = (int?)reader["spelerlengte"];
                        if (!reader.IsDBNull(reader.GetOrdinal("spelergewicht"))) gewicht = (int?)reader["spelergewicht"];
                        Speler sTeam = new Speler((int)reader["id"], (string)reader["spelernaam"], lengte, gewicht);
                        if (!reader.IsDBNull(reader.GetOrdinal("spelerrugnummer"))) sTeam.ZetRugnummer((int)reader["spelerrugnummer"]);
                        sTeam.ZetTeam(team);
                    }
                }
                return speler;
            }
            
        }

        public IReadOnlyList<SpelerInfo> SelecteerSpelers(int? id, string naam)
        {
            string sql = "SELECT t1.id,t1.naam,t1.rugnummer,t1.lengte,t1.gewicht,\r\n      case \r\n\t  when t2.stamnummer is null then null\r\n\t  when t2.bijnaam is null then concat(t2.naam,' - ',t2.stamnummer)\r\n\t     else concat(t2.naam,' (',t2.bijnaam,') - ',t2.stamnummer)\r\n     end teamnaam\r\n  FROM [Speler] t1 left join team t2 on t1.teamId=t2.stamnummer";
            if (id.HasValue) sql += " WHERE t1.id=@id";
            else sql += " WHERE t1.naam=@naam";
            List<SpelerInfo> spelers = new List<SpelerInfo>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                if (id.HasValue)
                {
                    cmd.Parameters.AddWithValue("@id", id);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@naam", naam);
                }
                cmd.CommandText = sql;
                conn.Open();

                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string team = null;
                    if (!reader.IsDBNull(reader.GetOrdinal("teamnaam"))) team = (string)reader["teamnaam"];
                    int? lengte = null;
                    if (!reader.IsDBNull(reader.GetOrdinal("lengte"))) lengte = (int)reader["lengte"];
                    int? gewicht = null;
                    if (!reader.IsDBNull(reader.GetOrdinal("gewicht"))) gewicht = (int)reader["gewicht"];
                    int? rugnr = null;
                    if (!reader.IsDBNull(reader.GetOrdinal("rugnummer"))) rugnr = (int)reader["rugnummer"];
                    SpelerInfo speler = new SpelerInfo((int)reader["id"], (string)reader["naam"], rugnr, lengte, gewicht, team);
                    spelers.Add(speler);
                }
                reader.Close();
                return spelers;
            }
        }

        public void UpdateSpeler(Speler speler)
        {
            string sql = "UPDATE speler SET naam=@naam,rugnummer=@rugnummer,lengte=@lengte,gewicht=@gewicht WHERE id=@id";
            using(SqlConnection conn=new SqlConnection(connectionString))
            using(SqlCommand cmd=conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@id",speler.Id);
                cmd.Parameters.AddWithValue("@naam",speler.Naam);
                if (speler.Lengte != null) cmd.Parameters.AddWithValue("@lengte",speler.Lengte);
                else cmd.Parameters.AddWithValue("@lengte",DBNull.Value);
                if (speler.Gewicht != null) cmd.Parameters.AddWithValue("@gewicht",speler.Gewicht);
                else cmd.Parameters.AddWithValue("@gewicht",DBNull.Value);
                if (speler.Rugnummer != null) cmd.Parameters.AddWithValue("@rugnummer",speler.Rugnummer);
                else cmd.Parameters.AddWithValue("@rugnummer",DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
