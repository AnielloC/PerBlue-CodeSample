using UnityEngine;

namespace Assets._Scripts.Levels
{
    public abstract class LevelControllerBase : MonoBehaviour
    {
        // Parent transform for the level segments the player can move to
        [SerializeField] protected Transform levelSegments;

        // Accessor for the level segments parent transform
        public Transform LevelSegments { get { return levelSegments; } }

        // Current level segment the player is on
        public int curLevelSegment { get; protected set; }

        protected virtual void Start()
        {
            curLevelSegment = 1;
        }

        public abstract void StartNextSegment();
    }
}
