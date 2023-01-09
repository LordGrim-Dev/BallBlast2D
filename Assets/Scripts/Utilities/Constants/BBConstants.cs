
namespace Game.Common
{
    public class BBConstants
    {
        public const float k_RESPAWN_COOLDOWN_TIME = 7f;

        public const float k_MAX_FIRE_COOLDOWN = 0.065f;

        public const float k_MAX_XVELOCITY = 6.25f;

        public const float k_MAX_SPAWN_TIME_GAP = 3.5f;

        public const int k_MAX_PARENT_BALLS_ON_SCREEN = 1;

        public const uint k_SPLIT_BALL_ID = 1002;
        public const uint k_PARENT_BALL_ID = 1001;

        public const string CONFIG_FILE_NAME = "ball_blast_level_data";
        
        public const string CONFIG_PLAYR_PRGRSS = "ball_blast_p_prog.json";

        public const string K_UI_PREFAB_PATH = "Prefabs/UI/";
    }

    public enum Direction : uint
    {
        eNone = 0,
        eLeft = 1,
        eRight,
        eTop,
        eBottom
    }


    public enum BallSize : uint
    {
        eLevel_0 = 0,
        eLevel_1 = 1,
        eLevel_2 = 2,
        eLevel_3 = 3
    }
}