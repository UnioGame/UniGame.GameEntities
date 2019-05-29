using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using Newtonsoft.Json;

namespace Samples
{
    public class ClanQueries
    {
        private const int QUERY_LIMIT = 10;
        // TO DO отвязать от фаербейса, инкапсулировать запросы в объекты

        /// <summary>
        /// В будущем запрос будет возвращать неполные данные кланов в целях снижения нагрузки
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public async Task<List<ClanData>> GetClans(string prefix)
        {
            var query = FirebaseDatabase.DefaultInstance.RootReference.Child("Clans/").OrderByChild("ClanName").StartAt(prefix);
            var data = await query.GetValueAsync();
            var result = new List<ClanData>();
            foreach(var clanSnapshot in data.Children)
            {
                var clan = JsonConvert.DeserializeObject<ClanData>(clanSnapshot.GetRawJsonValue());
                result.Add(clan);
            }
            return result;
        }
    }
}
