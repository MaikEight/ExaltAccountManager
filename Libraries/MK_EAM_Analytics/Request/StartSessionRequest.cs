using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Linq;

namespace MK_EAM_Analytics.Request
{
    [System.Serializable]
    public sealed class StartSessionRequest
    {
        public string ClientIdHash { get; set; }
        [JsonConverter(typeof(VersionPropertyConverter))]
        public Version ClientVersion { get; set; }
        public int AmountOfAccounts { get; set; }
    }

    public class VersionPropertyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Version.Parse(reader.Value.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                serializer.Serialize(writer, null);
                return;
            }

            writer.WriteValue(value.ToString());
        }
    }
}
