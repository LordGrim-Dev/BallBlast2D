using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BallBlast.config
{
    public class LocalisationDetailes
    {
        [JsonProperty("en")]
        Dictionary<string, string> mLocalisationDetails_en;

        [JsonIgnore]
        public Dictionary<string, string> LocalisationDetails_en { get => mLocalisationDetails_en; }

    }
}