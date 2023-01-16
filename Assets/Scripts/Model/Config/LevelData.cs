
using Newtonsoft.Json;

namespace BallBlast.config
{

    [JsonObject(MemberSerialization.OptIn)]
    public class LevelDetails
    {
        [JsonProperty("max_count")]
        private uint m_MaxCount;

        [JsonProperty("ball_size_prob")]
        // level-0 to level-4
        // 4 values with probabality dist
        private uint[] m_BallSizeProbability;

        [JsonConstructor]
        public LevelDetails(uint maxLevelCount, uint[] ballSizeProbability)
        {
            m_MaxCount = maxLevelCount;
            m_BallSizeProbability = ballSizeProbability;
        }

        [JsonIgnore] public uint MaxCount { get => m_MaxCount; }

        [JsonIgnore] public uint[] BallSizeProbability { get => m_BallSizeProbability; }
    }
}