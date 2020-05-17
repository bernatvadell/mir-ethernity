using Mir.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ICharacterRepository
    {
        Task<IEnumerable<Character>> FindByAccountId(int accountId);
    }
}
