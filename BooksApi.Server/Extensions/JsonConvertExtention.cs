using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server.Extensions
{
    static class JsonConvertExtention
    {

            public static string ToJson(this object? obj, Formatting form)
            {
                return JsonConvert.SerializeObject(obj, form);
            }



            public static T? FromJson<T>(this string obj)
            {
                var deserializer = JsonConvert.DeserializeObject<T>(obj);
                return deserializer;
            }

    }
}
