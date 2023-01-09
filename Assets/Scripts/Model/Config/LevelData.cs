
using Newtonsoft.Json;

namespace BallBlast.config
{

    [JsonObject(MemberSerialization.OptIn)]
    public class LevelDetails
    {
        [JsonProperty("max_count")]
        private int m_MaxCount;

        [JsonProperty("ball_size_prob")]
        // level-0 to level-4
        // 4 values with probabality dist
        private uint[] m_BallSizeProbability;

        [JsonConstructor]
        public LevelDetails(int maxLevelCount, uint[] ballSizeProbability)
        {
            m_MaxCount = maxLevelCount;
            m_BallSizeProbability = ballSizeProbability;
        }

        [JsonIgnore] public int MaxCount { get => m_MaxCount; }

        [JsonIgnore] public uint[] BallSizeProbability { get => m_BallSizeProbability; }
    }
}