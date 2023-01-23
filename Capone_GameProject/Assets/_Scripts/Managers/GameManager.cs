using Assets._Scripts.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._Scripts.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        // States the game can be in
        public enum GameState
        {
            NORMAL,
            TEXT,
            PAUSED,
            TRANSITION
        }

        // What the current game state is
        public GameState curGameState { get; private set; } = GameState.NORMAL;

        // Event to call when the game state is changed
        public UnityEvent<GameState> OnGameStateChanged { get; set; } = new UnityEvent<GameState>();

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// Set the gamestate to the given state
        /// </summary>
        /// <param name="newState">State to change to</param>
        public void SetGameState(GameState newState)
        {
            curGameState = newState;

            if (newState == GameState.PAUSED)
            {
                Time.timeScale = 0f;
            }
            else Time.timeScale = 1.0f;

            OnGameStateChanged?.Invoke(newState);
        }

        // Close application
        public void QuitGame()
        {
            Application.Quit();
        }

        protected override void OnDestroy()
        {
            OnGameStateChanged.RemoveAllListeners();
            base.OnDestroy();
        }
    }
}
