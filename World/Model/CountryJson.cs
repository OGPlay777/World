using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace World.Model
{
    

    public partial class CountryJson
    {
        [JsonProperty("name")]
        public Name Name { get; set; }

        [JsonProperty("cca2")]
        public string Cca2 { get; set; }
    }

    public partial class Name
    {
        [JsonProperty("common")]
        public string Common { get; set; }

        [JsonProperty("official")]
        public string Official { get; set; }

        [JsonProperty("nativeName")]
        public Dictionary<string, NativeName> NativeName { get; set; }
    }

    public partial class NativeName
    {
        [JsonProperty("official")]
        public string Official { get; set; }

        [JsonProperty("common")]
        public string Common { get; set; }
    }


    

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
