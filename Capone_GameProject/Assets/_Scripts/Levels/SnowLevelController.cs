using Assets._Scripts.Data;
using Assets._Scripts.Managers;
using UnityEngine;

namespace Assets._Scripts.Levels
{
    public class SnowLevelController : LevelControllerBase
    {
        // Reference to the raycast controller
        [SerializeField] protected MouseManager mouseManager;

        // Text controller displaying the text box
        [SerializeField] private TextManager textManager;

        // Ground raycast target that allows the player to move
        [SerializeField] private GameObject groundRaycast;

        [Space]
        // Array for text used in each segment of the level
        [SerializeField] private LevelSegmentText[] levelText;

        // The number of times the player has clicked to move
        private int timesMoved = 0;

        // Setup the text and values for the current level segment the player is at
        public override void StartNextSegment()
        {
            groundRaycast.SetActive(true);

            switch (curLevelSegment)
            {
                case 1:
                    textManager.SetDisplayText(levelText[0].SegmentText);
                    textManager.OnTextFinished.AddListener(SetStageOne);
                    break;

                case 2:
                    textManager.SetDisplayText(levelText[1].SegmentText);
                    textManager.OnTextFinished.AddListener(SetStageTwo);
                    break;

                case 3:
                    textManager.SetDisplayText(levelText[2].SegmentText);
                    break;

                default:
                    Debug.LogError("curLevelSegment has exceeded the number of segments in this level!");
                    break;
            }
        }

        // Check if the game should end. If not, let the player move on
        public void CheckForGameEnd()
        {
            curLevelSegment++;
            if (levelSegments.childCount >= curLevelSegment)
            {
                UiManager.Instance.ShowNextArrow();
            }
        }

        // Make sure the player has moved x times before continuing to the next level
        private void CheckTimesMoved(Vector2 pos)
        {
            timesMoved++;
            if (timesMoved >= 3)
            {
                mouseManager.OnGroundClicked.RemoveListener(CheckTimesMoved);
                textManager.SetDisplayText(levelText[3].SegmentText);
                CheckForGameEnd();
            }
        }

        // Setup segement one
        private void SetStageOne()
        {
            mouseManager.OnGroundClicked.AddListener(CheckTimesMoved);
            timesMoved = 0;

            textManager.OnTextFinished.RemoveListener(SetStageOne);
        }

        // Setup segment two
        private void SetStageTwo()
        {
            UiManager.Instance.ShowColorChangeScreen();
            textManager.OnTextFinished.RemoveListener(SetStageTwo);
        }

        // Finish segment two
        public void EndStageTwo()
        {
            textManager.SetDisplayText(levelText[4].SegmentText);
            CheckForGameEnd();
        }
    }
}
