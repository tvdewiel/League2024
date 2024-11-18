using LeagueBL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueBL.Model
{
    public class Team
    {
        public Team(int stamnummer, string naam)
        {
            ZetStamnummer(stamnummer);
            ZetNaam(naam);
        }

        public int Stamnummer {  get; private set; }
        public string Naam {  get;private set; }
        public string Bijnaam { get;private set; }
        private List<Speler> spelers = new List<Speler>();
        public IReadOnlyList<Speler> Spelers => spelers;
        internal void VoegSpelerToe(Speler speler)
        {
            if (speler==null) throw new TeamException("VoegSpelerToe");
            if (spelers.Contains(speler)) throw new TeamException("VoegSpelerToe");
            spelers.Add(speler);
            if (speler.Team!=this)
                speler.ZetTeam(this);
        }
        internal void VerwijderSpeler(Speler speler)
        {
            if (speler == null) throw new TeamException("VerwijderSpeler");
            if (!spelers.Contains(speler)) throw new TeamException("VerwijderSpeler");
            spelers.Remove(speler);
            if (speler.Team == this) speler.VerwijderTeam();
        }
        public bool HeeftSpeler(Speler speler)
        {
            return spelers.Contains(speler);
        }
        public void ZetNaam(string naam)
        {
            if (string.IsNullOrWhiteSpace(naam)) throw new TeamException("ZetNaam");
            Naam = naam;
        }
        public void ZetStamnummer(int nr)
        {
            if (nr <= 0) throw new TeamException("stamnr <=0");
            Stamnummer = nr;
        }
        public void ZetBijnaam(string naam)
        {
            if (string.IsNullOrWhiteSpace(naam)) throw new TeamException("ZetBijnaam");
            Bijnaam = naam;
        }

        public override bool Equals(object? obj)
        {
            return obj is Team team &&
                   Stamnummer == team.Stamnummer;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Stamnummer);
        }
        public static bool operator ==(Team t1,Team t2)
        {
            if (t1 is null && t2 is null) return true;
            if (t1 is null || t2 is null) return false;
            return t1.Stamnummer==t2.Stamnummer;
        }
        public static bool operator !=(Team t1, Team t2)
        {
            if (t1 is null && t2 is null) return false;
            if (t1 is null || t2 is null) return true;
            return t1.Stamnummer != t2.Stamnummer;
        }
    }
}
