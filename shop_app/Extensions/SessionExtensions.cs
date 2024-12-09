using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace shop_app.Extensions
{
    public static class SessionExtensions
    {
        // Сохранение объекта в сессии
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        // Извлечение объекта из сессии
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
