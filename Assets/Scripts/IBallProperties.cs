namespace BallBlast
{
    public interface IBallProperties
    {
        UnityEngine.Vector2 Position { get; }

        void InitialiseNewBall(uint inBallMaxHitCount, UnityEngine.Vector2 isSplitBall, float inInitialXVelocity, bool inInitialSlowWallSpawnAnim);

        void ResetBallProperties();
    }
}