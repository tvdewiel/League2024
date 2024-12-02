using LeagueBL.Interfaces;
using LeagueBL.Managers;
using LeagueBL.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueDL
{
    public class TransferRepositoryADO : ITransferRepository
    {
        private string connectionString;

        public TransferRepositoryADO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Transfer SchrijfTransferInDB(Transfer transfer)
        {
            string sqlTransfer = "INSERT INTO transfer(spelerid,prijs,oudteamid,nieuwteamid) output INSERTED.ID VALUES(@spelerid,@prijs,@oudteamid,@nieuwteamid)";
            string sqlSpeler = "UPDATE speler SET teamid=@teamid WHERE id=@id";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            using (SqlCommand cmdSpeler = sqlConnection.CreateCommand())
            using (SqlCommand cmdTransfer=sqlConnection.CreateCommand())
            {
                sqlConnection.Open();
                SqlTransaction tran = sqlConnection.BeginTransaction();
                cmdSpeler.Transaction = tran;
                cmdTransfer.Transaction = tran;
                try
                {
                    //transfer
                    cmdTransfer.CommandText = sqlTransfer;
                    cmdTransfer.Parameters.AddWithValue("@spelerid", transfer.Speler.Id);
                    cmdTransfer.Parameters.AddWithValue("@prijs", transfer.Prijs);
                    if (transfer.OudTeam == null)
                        cmdTransfer.Parameters.AddWithValue("@oudteamid", DBNull.Value);
                    else
                        cmdTransfer.Parameters.AddWithValue("@oudteamid", transfer.OudTeam.Stamnummer);
                    if (transfer.NieuwTeam == null)
                        cmdTransfer.Parameters.AddWithValue("@nieuwteamid", DBNull.Value);
                    else
                        cmdTransfer.Parameters.AddWithValue("@nieuwteamid", transfer.NieuwTeam.Stamnummer);
                    int newID = (int)cmdTransfer.ExecuteScalar();
                    transfer.ZetId(newID);
                    //speler update
                    cmdSpeler.CommandText = sqlSpeler;
                    cmdSpeler.Parameters.AddWithValue("@id", transfer.Speler.Id);
                    if (transfer.Speler.Team == null)
                    {
                        cmdSpeler.Parameters.AddWithValue("@teamid", DBNull.Value);
                    }
                    else
                    {
                        cmdSpeler.Parameters.AddWithValue("@teamid", transfer.Speler.Team.Stamnummer);
                    }
                    cmdSpeler.ExecuteNonQuery();
                    tran.Commit();
                    return transfer;
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
        }
    }
}
