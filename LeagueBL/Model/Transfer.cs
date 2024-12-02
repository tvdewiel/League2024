using LeagueBL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueBL.Model
{
    public class Transfer
    {
        public Transfer(Speler speler, Team oudTeam) //stopt
        {
            ZetSpeler(speler);
            ZetOudTeam(oudTeam);
        }
        public Transfer(Speler speler, Team nieuwTeam,int prijs) //nieuw
        {
            ZetSpeler(speler);
            ZetNieuwTeam(nieuwTeam);
            ZetPrijs(prijs);
        }
        public Transfer(Speler speler,Team nieuwTeam, Team oudTeam,int prijs) //klassiek transfer
        {
            ZetSpeler(speler);
            ZetOudTeam(oudTeam);
            ZetNieuwTeam(nieuwTeam);
            ZetPrijs(prijs);
        }
        public int Id { get;private set; }
        public int Prijs { get; private set; }
        public Speler Speler { get; private set; }
        public Team NieuwTeam { get; private set; }
        public Team OudTeam { get; private set; }
        public void ZetId(int id)
        {
            if (id <= 0) throw new SpelerException("Id <=0");
            Id = id;
        }
        public void ZetPrijs(int prijs)
        {
            if (prijs < 0) throw new TransferException("prijs <0");
            Prijs = prijs;
        }
        public void ZetOudTeam(Team team)
        {
            if (team is null) throw new TransferException("ZetOudTeam");
            if (team==NieuwTeam) throw new TransferException("ZetOudTeam");
            OudTeam = team;
        }
        public void VerwijderOudTeam()
        {
            if (NieuwTeam is null) throw new TransferException("VerwijderOudTeam");
            OudTeam = null;
        }
        public void ZetNieuwTeam(Team team)
        {
            if (team is null) throw new TransferException("ZetNieuwTeam");
            if (team == OudTeam) throw new TransferException("ZetNieuwTeam");
            NieuwTeam = team;
        }
        public void VerwijderNieuwTeam()
        {
            if (OudTeam is null) throw new TransferException("VerwijderNieuwTeam");
            NieuwTeam = null;
        }
        public void ZetSpeler(Speler speler)
        {
            if (speler is null) throw new TransferException("ZetSpeler");
            Speler = speler;
        }
    }
}
