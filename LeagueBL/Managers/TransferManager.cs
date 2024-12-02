using LeagueBL.Exceptions;
using LeagueBL.Interfaces;
using LeagueBL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueBL.Managers
{
    public class TransferManager
    {
        private ITransferRepository repo;

        public TransferManager(ITransferRepository repo)
        {
            this.repo = repo;
        }
        public Transfer RegistreerTransfer(Speler speler, Team nieuwTeam,int prijs)
        {
            if (speler == null) throw new TransferManagerException("Registreer transfer - speler is null");
            Transfer transfer = null;
            try
            {
                //speler stopt
                if (nieuwTeam == null)
                {
                    //zou ook in methode verwijderteam kunnen
                    if (speler.Team == null) throw new TransferManagerException("Registreertransfer - team is null");
                    transfer = new Transfer(speler, speler.Team);
                    speler.VerwijderTeam();
                }
                else if (speler.Team == null) //nieuwe speler
                {
                    speler.ZetTeam(nieuwTeam);
                    transfer = new Transfer(speler, nieuwTeam, prijs);
                }
                else
                {
                    transfer = new Transfer(speler, nieuwTeam, speler.Team, prijs);
                }
                return repo.SchrijfTransferInDB(transfer);
            }
            catch (TransferManagerException e) { throw; }
            catch (Exception e) { throw new TransferManagerException("RegistreerTransfer", e); }
        }
    }
}
