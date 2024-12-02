using LeagueBL.DTO;
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
    public class TeamManager
    {
        private ITeamRepository repo;

        public TeamManager(ITeamRepository repo)
        {
            this.repo = repo;
        }
        public void RegistreerTeam(int stamnummer, string naam, string bijnaam)
        {
            try
            {
                Team t = new Team(stamnummer, naam);
                if (!string.IsNullOrWhiteSpace(bijnaam)) t.ZetBijnaam(bijnaam);
                if (!repo.BestaatTeam(t))
                {
                    repo.SchrijfTeamInDB(t);
                }
                else
                {
                    throw new TeamManagerException("registreer team - team bestaat al");
                }
            }
            catch (TeamManagerException) { throw; }
            catch (Exception ex)
            {
                throw new TeamManagerException("Registreer team", ex);
            }
        }
        public Team SelecteerTeam(int stamnummer)
        {
            try
            {
                Team t = repo.SelecteerTeam(stamnummer);
                if (t == null) throw new TeamManagerException("selecteerteam - team bestaat niet");
                return t;
            }
            catch (TeamManagerException) { throw; }
            catch (Exception ex) { throw new TeamManagerException("selecteerteam", ex); }
        }
        public IReadOnlyList<TeamInfo> SelecteerTeams()
        {
            try
            {
                return repo.SelecteerTeams();
            }
            catch (Exception ex) { throw new TeamManagerException("SelecteerTeams", ex); }
        }
    }
}
