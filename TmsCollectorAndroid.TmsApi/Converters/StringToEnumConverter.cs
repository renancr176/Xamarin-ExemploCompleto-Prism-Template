using System;
using System.Linq;
using Newtonsoft.Json;
using TmsCollectorAndroid.TmsApi.Extensions;

namespace TmsCollectorAndroid.TmsApi.Converters
{
    public class StringToEnumConverter<TEnum> : JsonConverter<TEnum> where TEnum : Enum
    {
        public override void WriteJson(JsonWriter writer, TEnum value, JsonSerializer serializer)
        {
            writer.WriteValue(value.GetDescription());
        }

        public override TEnum ReadJson(JsonReader reader, Type objectType, TEnum existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var enums = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();

            try
            {
                var result = enums.FirstOrDefault(e => e.GetDescription().ToUpper().Equals(reader.Value.ToString().ToUpper()));

                return ((result.GetDescription().ToUpper().Equals(reader.Value.ToString().ToUpper()))
                    ? result
                    : (TEnum) (object) -1);
            }
            catch (Exception ) { }

            return enums.FirstOrDefault();
        }
    }
}