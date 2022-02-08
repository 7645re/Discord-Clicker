using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Collections.Generic;
using discord_clicker.Models;

namespace discord_clicker.Serializer
{
    public static class Serializer
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize<T>(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
        }
        public static string ListObjectToString<Perk>(this List<Perk> list ) {
            Dictionary<string, string> result = new Dictionary<string, string>();
            return "1";
        }
    }
}
