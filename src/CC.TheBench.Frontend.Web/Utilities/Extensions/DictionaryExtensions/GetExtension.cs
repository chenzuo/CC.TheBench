namespace CC.TheBench.Frontend.Web.Utilities.Extensions.DictionaryExtensions
{
    using System.Collections.Generic;

    public static class GetExtension
    {
        public static T Get<T>(this IDictionary<string, object> d, string key)
        {
            object value;
            if (d.TryGetValue(key, out value))
            {
                return (T)value;
            }
            return default(T);
        }
    }
}