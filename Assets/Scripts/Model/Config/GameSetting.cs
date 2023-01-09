using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace BallBlast.config
{
    public class GameSetting
    {
        [JsonProperty("total_lives")]
        private int m_MaxLives;

        [JsonProperty("score_mult")]
        private int m_ScoreMultiplier;

        [JsonProperty("level_data")]
        [JsonRequired]
        private Dictionary<int, LevelDetails> m_LevelData;

        [JsonIgnore]
        public Dictionary<int, LevelDetails> Levels { get => m_LevelData; }

        [JsonIgnore]
        public int MaxLives { get => m_MaxLives; }

        [JsonIgnore]
        public int ScoreMultiplier { get => m_ScoreMultiplier; }
    }
}