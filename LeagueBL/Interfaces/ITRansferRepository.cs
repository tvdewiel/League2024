using LeagueBL.Managers;
using LeagueBL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueBL.Interfaces
{
    public interface ITransferRepository
    {
        Transfer SchrijfTransferInDB(Transfer transfer);
    }
}
