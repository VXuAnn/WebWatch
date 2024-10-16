using System.Text.Json;
using Microsoft.AspNetCore.Http; // Ensure you're using this for ISession

namespace ShopBanHang.Helpers
{
    public static class SessionExtensions // Make this static
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
