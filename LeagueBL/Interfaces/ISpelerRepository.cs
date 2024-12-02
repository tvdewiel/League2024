using LeagueBL.DTO;
using LeagueBL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueBL.Interfaces
{
    public interface ISpelerRepository
    {
        bool BestaatSpeler(Speler s);
        void SchrijfSpelerInDB(Speler s);
        IReadOnlyList<SpelerInfo> SelecteerSpelers(int? id, string naam);
    }
}
