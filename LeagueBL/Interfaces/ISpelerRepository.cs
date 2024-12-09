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
        bool BestaatSpeler(int id);
        void SchrijfSpelerInDB(Speler s);
        Speler SelecteerSpeler(int id);
        IReadOnlyList<SpelerInfo> SelecteerSpelers(int? id, string naam);
        void UpdateSpeler(Speler speler);
    }
}
