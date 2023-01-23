using Assets._Scripts.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._Scripts.Managers
{
    public class MouseManager : Singleton<MouseManager>
    {
        // Event for when the ground is clicked
        public UnityEvent<Vector2> OnGroundClicked { get; set; } = new UnityEvent<Vector2>();

        // If this should be checking clicks
        private bool canCheckRaycast = false;

        // Cache Vector to update with mouse pos in Update()
        private Vector3 mousePos;

        // Cache RaycastHit for use in Update()
        private RaycastHit2D hit;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            GameManager.Instance.OnGameStateChanged.AddListener(GameStateChanged);
            GameStateChanged(GameManager.Instance.curGameState);
        }

        /// <summary>
        /// Update if the current game state allows click detection
        /// </summary>
        /// <param name="newState">New game state</param>
        private void GameStateChanged(GameManager.GameState newState)
        {
            if (newState == GameManager.GameState.NORMAL || newState == GameManager.GameState.TEXT)
            {
                canCheckRaycast = true;
            }
            else canCheckRaycast = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (canCheckRaycast && Input.GetMouseButtonDown(0))
            {
                // Give mousePos the z distance from the camera to cast
                mousePos = Input.mousePosition;
                mousePos.z = Mathf.Abs(Camera.main.transform.localPosition.z);

                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos), Camera.main.transform.forward);
                if (hit.collider != null && hit.collider.name == "Floor Raycast")
                {
                    OnGroundClicked?.Invoke(hit.point);
                }
            }
        }

        protected override void OnDestroy()
        {
            OnGroundClicked?.RemoveAllListeners();
        }
    }
}
