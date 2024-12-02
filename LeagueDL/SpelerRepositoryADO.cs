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
    }
}
