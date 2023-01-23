using UnityEngine;

namespace Assets._Scripts.Utility
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;

        // Accessor for the instance
        public static T Instance
        {
            get { return instance; }
        }

        protected virtual void Awake()
        {
            // Destroy instance if there already is one, otherwise create instance
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = (T)this;
            }
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}
