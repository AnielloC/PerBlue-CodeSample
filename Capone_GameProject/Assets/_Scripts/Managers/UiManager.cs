using Assets._Scripts.Utility;
using UnityEngine;

namespace Assets._Scripts.Managers
{
    public class UiManager : Singleton<UiManager>
    {
        // Parent gameobject of the next arrow UI
        [SerializeField] private GameObject nextArrow;

        // UI screen for changing the penguin color
        [SerializeField] private GameObject colorChangeScreen;

        // Keep track of the state we were previously in
        private GameManager.GameState previousState;

        // Show the arrow that lets the user move to the next level
        public void ShowNextArrow()
        {
            nextArrow.SetActive(true);
        }

        /// <summary>
        /// Toggle paused state of the game
        /// </summary>
        /// <param name="pause">True if pausing</param>
        public void PauseGame(bool pause)
        {
            if (pause)
            {
                previousState = GameManager.Instance.curGameState;
                GameManager.Instance.SetGameState(GameManager.GameState.PAUSED);
            }
            else
            {
                GameManager.Instance.SetGameState(previousState);
            }
        }

        // Show the UI to allow color changing
        public void ShowColorChangeScreen()
        {
            colorChangeScreen.SetActive(true);
        }
    }
}
