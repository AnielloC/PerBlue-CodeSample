using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._Scripts.Managers
{
    public class TextManager : MonoBehaviour
    {
        // Event to fire when the text is done displaying
        public UnityEvent OnTextFinished = new UnityEvent();

        // The animator on the text box
        [SerializeField] private Animator textBoxAnim;

        // The text field actually showing the text
        [SerializeField] private TextMeshProUGUI text;

        // Raycast target to force skip dialogue
        [SerializeField] private GameObject boxRaycast;

        [Header("Audio")]
        // Audio source to play our audio clips
        [SerializeField] private AudioSource audioSource;

        // Audio clips for the alphabet letters
        [SerializeField] private AudioClip[] voiceClips;

        // Dictionary to hold the audio clips to be used
        private Dictionary<string, AudioClip> clips;

        // List of current texts to show
        private List<string> textQueue;

        // Cache our stingbuilder for reuse later
        private StringBuilder stringBuilder;

        // Hold reference to the coroutine being used
        private Coroutine currentCoroutine;

        // Track if the text box is closing/closed
        private bool closing = true;

        private void Start()
        {
            // Give the clips to a dictionary so we can extract the value as needed later instead of looping the array
            clips = new Dictionary<string, AudioClip>();
            foreach(AudioClip ac in voiceClips)
            {
                clips.Add(ac.name, ac);
            }
        }

        /// <summary>
        /// Set the list of text to be displayed and start the text box
        /// </summary>
        /// <param name="texts">Text to show the player</param>
        public void SetDisplayText(List<string> texts)
        {
            GameManager.Instance.SetGameState(GameManager.GameState.TEXT);
            textQueue = texts;
            textBoxAnim.Play("Open");
            closing = false;
        }

        // Get the next string to show from the list. If none, close the text box
        public void GetTextToDisplay()
        {
            if (textQueue.Count > 0)
            {
                currentCoroutine = StartCoroutine(GenerateTextForAudio(textQueue[0]));
                textQueue.RemoveAt(0);
                boxRaycast.SetActive(true);
            }
            else
            {
                text.text = "";
                boxRaycast.SetActive(false);

                // Only play the closing anim if we are sure the box not already closing
                if (!closing)
                {
                    textBoxAnim.Play("Close");
                }

                closing = true;
                TextIsFinished();
            }
        }

        /// <summary>
        /// Display the text by letter and play letter audio
        /// </summary>
        /// <param name="s">full text string to display</param>
        private IEnumerator GenerateTextForAudio(string s)
        {
            stringBuilder = new StringBuilder();

            foreach(char c in s)
            {
                stringBuilder.Append(c.ToString());
                text.text = stringBuilder.ToString();
                PlayVoiceClip(c.ToString().ToLower());

                yield return new WaitForSeconds(0.065f);
            }

            yield return new WaitForSeconds(2f);

            GetTextToDisplay();
        }

        /// <summary>
        /// Play audio for the given letter if there is a matching audio clip for it
        /// </summary>
        /// <param name="s">letter to play</param>
        private void PlayVoiceClip(string s)
        {
            if (clips.ContainsKey(s))
            {
                audioSource.PlayOneShot(clips[s]);
            }
        }

        // Text is done displaying, so go back to normal mode
        public void TextIsFinished()
        {
            GameManager.Instance.SetGameState(GameManager.GameState.NORMAL);
            OnTextFinished?.Invoke();
        }

        /// <summary>
        /// Stop showing the current message
        /// </summary>
        /// <param name="stopAll">If true, don't show anymore messages from the current queue</param>
        public void ForceStopText(bool stopAll = false)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            if (stopAll)
            {
                textQueue.Clear();
            }

            text.text = "";
            GetTextToDisplay();
        }

        private void OnDestroy()
        {
            OnTextFinished?.Invoke();
        }
    }
}
