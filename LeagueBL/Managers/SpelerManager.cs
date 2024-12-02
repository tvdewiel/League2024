using LeagueBL.DTO;
using LeagueBL.Exceptions;
using LeagueBL.Interfaces;
using LeagueBL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LeagueBL.Managers
{
    public class SpelerManager
    {
        private ISpelerRepository repo;

        public SpelerManager(ISpelerRepository repo)
        {
            this.repo = repo;
        }

        public Speler RegistreerSpeler(string naam,int? lengte,int? gewicht)
        {
            try
            {
                Speler s = new Speler(naam, lengte, gewicht);
                if (!repo.BestaatSpeler(s))
                {
                    repo.SchrijfSpelerInDB(s);
                    return s;
                }
                else
                {
                    throw new SpelerManagerException("RegistreerSpeler - speler bestaat al");
                }
            }
            catch(SpelerManagerException) { throw; }
            catch (Exception ex) { throw new SpelerManagerException("RegistreerSpeler", ex); }
        }
        public IReadOnlyList<SpelerInfo> SelecteerSpelers(int? id,string naam)
        {
            if ((id == null) && (string.IsNullOrWhiteSpace(naam))) throw new SpelerManagerException("SelecteerSpelers - no valid input");
            try
            {
                return repo.SelecteerSpelers(id, naam);
            }
            catch(SpelerManagerException) { throw; }
            catch(Exception ex) { throw new SpelerManagerException("SelecteerSpelers", ex); }
        }
    }
}
