using UnityEngine;

namespace Chapter1Basic
{
    /// A service, not game state: see the README on why a score is a bad fit here.
    public class ScoreKeeper : MonoBehaviour
    {
        public static ScoreKeeper Instance { get; private set; }

        public int Score { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void Add(int points) => Score += points;

        public void ResetScore() => Score = 0;
    }
}
