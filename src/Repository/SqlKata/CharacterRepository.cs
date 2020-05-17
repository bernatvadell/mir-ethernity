using Dapper;
using Mir.Models;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SqlKata
{
    public class CharacterRepository : ICharacterRepository
    {
        private QueryFactory _db;

        public CharacterRepository(QueryFactory db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Character>> FindByAccountId(int accountId)
        {
            var result = _db.Query("user.character")
                .Join("content.gender", "content.gender.id", "user.character.gender_id")
                .Join("content.class", "content.class.id", "user.character.class_id")
                .Join("content.attack_mode", "content.attack_mode.id", "user.character.attack_mode_id")
                .Where(new { account_id = accountId })
                .GetSqlResult();

            return await _db.Connection.QueryAsync<Character, MirGender, MirClass, AttackMode, Character>(
                result.Sql,
                (character, mirGender, mirClass, attackMode) =>
                {
                    character.Gender = mirGender;
                    character.Class = mirClass;
                    character.AttackMode = attackMode;
                    return character;
                },
                param: result.NamedBindings
            );
        }
    }
}
