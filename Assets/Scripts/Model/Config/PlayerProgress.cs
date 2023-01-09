using Newtonsoft.Json;

namespace BallBlast.config
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PlayerProgressData
    {
        [JsonProperty("p_level")]
        private int m_PlayerSavedLevel;

        [JsonProperty("p_score")]
        private int m_PlayerScore;
        
        [JsonConstructor]
        public PlayerProgressData(int playerSavedLevel, int playerScore)
        {
            m_PlayerSavedLevel = playerSavedLevel;
            m_PlayerScore = playerScore;
        }

        [JsonIgnore] public int PlayerSavedLevel { get => m_PlayerSavedLevel; }
        [JsonIgnore] public int PlayerScore { get => m_PlayerScore; }
    }
}
