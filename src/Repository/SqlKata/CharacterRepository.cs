using Mir.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SqlKata
{
    public class CharacterRepository : ICharacterRepository
    {
        public Task<IEnumerable<Character>> FindByAccountId(int accountId)
        {
            throw new NotImplementedException();
        }
    }
}
