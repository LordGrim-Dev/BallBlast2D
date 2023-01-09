namespace BallBlast
{
    public interface IBallProperties
    {
        Game.Common.BallSize CurrentBallSizeLevel { get; }
        uint BallID { get; }

        void InitialiseNewBall(uint inBallMaxHitCount, UnityEngine.Vector3 inPos, Game.Common.BallSize inBallSize, float inInitialXVelocity);

        void ResetBallProperties();
    }
}