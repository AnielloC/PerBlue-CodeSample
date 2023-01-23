using Assets._Scripts.Levels;
using Assets._Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Managers")]
        // LevelController reference
        [SerializeField] private LevelControllerBase levelCon;

        // Camera manager reference
        [SerializeField] private CameraManager camManager;

        [Space]
        // Player sprite renderer
        [SerializeField] private SpriteRenderer sprite;

        // Animator controller for the player
        [SerializeField] private Animator animator;

        // The position to move the player to
        private Vector2 targetPos;

        // If the player character is currently moving
        private bool moving = false;

        // If the player character should be moving
        private bool canMove = true;

        // Start is called before the first frame update
        void Start()
        {
            MouseManager.Instance.OnGroundClicked.AddListener(UpdateMovePosition);
            camManager.OnCameraStoppedMoving.AddListener(TransitionToNewSegment);

            TransitionToNewSegment(1);
        }

        // Update is called once per frame
        void Update()
        {
            if (canMove)
            {
                MovePlayer();
            }
        }

        // Update the animater in fixed intervals
        private void FixedUpdate()
        {
            animator.SetBool("Moving", moving);
            animator.SetBool("SceneChange", GameManager.Instance.curGameState == GameManager.GameState.TRANSITION);
        }

        // Move the player gameobject
        private void MovePlayer()
        {
            // Check distance to make sure we still have distance to move
            if (Vector2.Distance(transform.localPosition, targetPos) > 0.1f)
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetPos, Time.deltaTime * 10);
                moving = true;
            }
            else StopPlayer();
        }

        /// <summary>
        /// Move the player to the next segment
        /// </summary>
        /// <param name="next">Segment number</param>
        private void TransitionToNewSegment(int next)
        {
            // Bring player to the edge of the next segment and start the transition movement
            canMove = false;
            transform.localPosition = levelCon.LevelSegments.Find($"Targets{levelCon.curLevelSegment}/EnterTarget").position;
            GameManager.Instance.SetGameState(GameManager.GameState.TRANSITION);
            UpdateMovePosition(levelCon.LevelSegments.Find($"Targets{levelCon.curLevelSegment}/StartTarget").position);
            canMove = true;
        }

        // Reset movement variables when the player is stopped
        private void StopPlayer()
        {
            moving = false;

            // If coming out of a transition state, that means the player is in a new level segment
            if (GameManager.Instance.curGameState == GameManager.GameState.TRANSITION)
            {
                levelCon.StartNextSegment();
                GameManager.Instance.SetGameState(GameManager.GameState.TEXT);
            }
        }

        /// <summary>
        /// Set target position based on the parameter value
        /// </summary>
        /// <param name="target">Position to move player to</param>
        public void UpdateMovePosition(Vector2 target)
        {
            targetPos = new Vector2(target.x, transform.localPosition.y);
            sprite.flipX = targetPos.x < transform.localPosition.x;
        }

        /// <summary>
        /// Update the color the player sprite
        /// </summary>
        /// <param name="img">Image with the color to use</param>
        public void SetPlayerColor(Image img)
        {
            sprite.color = img.color;
        }
    }
}
