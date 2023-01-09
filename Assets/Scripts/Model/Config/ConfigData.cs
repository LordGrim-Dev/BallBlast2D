using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace BallBlast.config
{
    public class ConfigData
    {

        [JsonProperty(ConfigJsonConstants.kLocalisation)]
        private LocalisationDetailes mLocalisationDetails;


        [JsonProperty(ConfigJsonConstants.kGameSetting)]
        private GameSetting mSettingReference;

        [JsonIgnore]
        public GameSetting GameSetting
        {
            get
            {
                return mSettingReference;
            }
        }

        [JsonIgnore]
        internal LocalisationDetailes LocalisationDetails { get => mLocalisationDetails; }
    }
}