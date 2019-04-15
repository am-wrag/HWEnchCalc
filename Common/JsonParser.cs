using Newtonsoft.Json;

namespace HWEnchCalc.Common
{
    public static class JsonParser<T>
    {
        public static string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static T Deserialize(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}