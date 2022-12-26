
namespace Game.Common
{
    public class BBConstants
    {
        public const float KMAX_FIRE_COOLDOWN = 5f;

        public const float k_MAX_XVELOCITY = 4.25f;

        public const float k_MAX_SPAWN_TIME_GAP = 3.5f;

        public const int kMAX_BALLS_ON_SCREEN = 1;

    }

    public enum Direction
    {
        eNone = 0,
        eLeft = 1,
        eRight,
        eTop,
        eBottom
    }


    public enum BallSize
    {
        eLevel = 0,
        eLevel1 = 1,
        eLevel2
    }
}