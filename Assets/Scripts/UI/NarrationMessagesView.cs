using System.Collections;
using System.Collections.Generic;
using System.Text;
using Channels;
using Narrative;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class NarrationMessagesView : MonoBehaviour, ISubscriber<Narration>
    {
        public NarrationChannel narrationChannel;
        public AudioSource audioSource;
        public bool gradualReveal;

        private NarrationMessagesController narrationMessagesController;
        private Stack<Narration> narrations = new();

        private void OnEnable()
        {
            var ui = GetComponent<UIDocument>();
            narrationMessagesController = new(ui.rootVisualElement);
            narrationMessagesController.Clear();
            narrationChannel.AddSubscriber(this);
        }

        private void OnDisable()
        {
            narrationChannel.RemoveSubscriber(this);
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
                    var delay = narration.defaultDuration;

                    if (narration.audioClip != null)
                    {
                        delay = Mathf.Max(delay, narration.audioClip.length);
                        audioSource.PlayOneShot(narration.audioClip);
                    }

                    if (gradualReveal)
                    {
                        yield return narrationMessagesController.RevealText(
                            narration.text.ToCharArray(),
                            delay
                        );
                    }
                    else
                    {
                        narrationMessagesController.Show(narration.text);
                        yield return new WaitForSeconds(delay);
                    }

                    yield return new WaitForSeconds(narration.delayUntilNext);
                    if (narration.next == null) break;
                    narration = narration.next;
                }
                narrationMessagesController.Clear();
            }
        }
    }

    internal class NarrationMessagesController
    {
        private readonly Label label;

        public NarrationMessagesController(VisualElement root)
        {
            label = root.Q<Label>();
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
            float wait = audioClipLength / (float) textToReveal.Length * 0.85f;
            StringBuilder displayText = new StringBuilder(textToReveal.Length);

            while (count < textToReveal.Length)
            {
                displayText.Append(textToReveal[count]);
                label.text = displayText.ToString();
                count++;

                yield return new WaitForSeconds(wait);
            }
        }
    }
}