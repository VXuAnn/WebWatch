﻿using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace ShopBanHang.Areas.Admin.Extensions
{
    public static class SessionExtension
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }
        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
