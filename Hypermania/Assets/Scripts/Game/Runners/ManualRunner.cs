namespace Game.Runners
{
    public class ManualRunner : SingleplayerRunner
    {
        public override void Poll(float deltaTime)
        {
            if (!_initialized)
            {
                return;
            }
            GameLoop();
        }
    }
}
