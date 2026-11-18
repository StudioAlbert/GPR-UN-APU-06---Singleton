using Singletons;

namespace Chapter3NullObject
{
    /// A service, not game state: see the README on why a score is a bad fit here.
    public class ScoreKeeper : Singleton<ScoreKeeper>
    {
        public int Score { get; private set; }

        public void Add(int points) => Score += points;

        public void ResetScore() => Score = 0;
    }
}
