using System.Collections;
using System.Collections.Generic;
using Channels;
using Narrative;
using UnityEngine;
using UnityEngine.UIElements;
using System.Text;

namespace UI
{
    public class RadioMessagesView : MonoBehaviour, ISubscriber<Narration>
    {
        public NarrationChannel radioMessages;
        public AudioSource audioSource;

        private RadioMessagesController radioMessagesController;
        private Stack<Narration> narrations = new();
        private Coroutine revealCoroutine;

        private void OnEnable()
        {
            var ui = GetComponent<UIDocument>();
            radioMessagesController = new (ui.rootVisualElement);
            radioMessagesController.Clear();
            radioMessages.AddSubscriber(this);
        }

        private void OnDisable()
        {
            radioMessages.RemoveSubscriber(this);
        }

        public void OnReceive(Narration narration)
        {
            narrations.Push(narration);
        }

        private IEnumerator Start()
        {
            while (true)
            {
                while (narrations.Count == 0)
                {
                    yield return new WaitForFixedUpdate();
                }

                var narration = narrations.Pop();
                while (narration != null)
                {
                    // radioMessagesController.Show(narration.text);
                    StartCoroutine(
                        radioMessagesController.RevealText(
                            narration.text.ToCharArray(),
                            narration.audioClip != null ? narration.audioClip.length : narration.defaultDuration
                        ));

                    if (narration.audioClip != null)
                    {
                        audioSource.PlayOneShot(narration.audioClip);
                    }
                    yield return new WaitForSeconds(narration.defaultDuration);
                    while (audioSource.isPlaying)
                    {
                        yield return new WaitForFixedUpdate();
                    }

                    if (narration.next != null)
                    {
                        yield return new WaitForSeconds(narration.delayUntilNext);
                        narration = narration.next;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    internal class RadioMessagesController
    {
        private readonly Label label;
        public RadioMessagesController(VisualElement root)
        {
            label = root.Q<Label>("RadioMessages");
        }

        public void Show(string text)
        {
            label.text = text;
        }

        public void Clear()
        {
            label.text = "";
        }

        public IEnumerator RevealText(char[] textToReveal, float audioClipLength)
        {
            int count = 0;
            float wait = audioClipLength / (float)textToReveal.Length * 0.85f;
            StringBuilder displayText = new StringBuilder(textToReveal.Length);

            while (count < textToReveal.Length)
            {
                displayText.Append(textToReveal[count]);
                label.text = displayText.ToString();
                count++;

                yield return new WaitForSeconds(wait);
            }

            yield return new WaitForSeconds(1);

            Clear();
        }
    }
}