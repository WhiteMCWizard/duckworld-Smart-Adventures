using UnityEngine;
using UnityEngine.UI;

namespace CinemaDirector
{
    [ExecuteInEditMode]
    public class FadeToWhite : MonoBehaviour
    {
        [SerializeField] private Color from = Color.clear;
        [SerializeField] private Color to = Color.white;
        [SerializeField] private float duration = 1f;

        private void Awake()
        {
            var rawImage = GetComponent<RawImage>();
            if (rawImage == null)
            {
                rawImage = gameObject.AddComponent<RawImage>();
                transform.position = Vector3.zero;
                transform.localScale = new Vector3(100f, 100f, 100f);
                rawImage.texture = new Texture2D(1, 1);
                rawImage.enabled = false;
                rawImage.rectTransform.anchoredPosition = Vector2.zero;
                rawImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
                rawImage.color = Color.clear;
            }
        }

        public void Trigger()
        {
            var rawImage = GetComponent<RawImage>();
            if (rawImage != null)
            {
                rawImage.enabled = true;
                rawImage.rectTransform.anchoredPosition = Vector2.zero;
                rawImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
                rawImage.color = from;
            }
        }

        public void ReverseTrigger()
        {
            End();
        }

        public void UpdateTime(float time, float deltaTime)
        {
            float transition = time / duration;
            FadeToColor(from, to, transition);
        }

        public void SetTime(float time, float deltaTime)
        {
            var rawImage = GetComponent<RawImage>();
            if (rawImage != null)
            {
                if (time >= 0f && time <= duration)
                {
                    rawImage.enabled = true;
                    UpdateTime(time, deltaTime);
                }
                else if (rawImage.enabled)
                {
                    rawImage.enabled = false;
                }
            }
        }

        public void End()
        {
            var rawImage = GetComponent<RawImage>();
            if (rawImage != null)
            {
                rawImage.enabled = false;
            }
        }
        public void ReverseEnd()
        {
            var rawImage = GetComponent<RawImage>();
            if (rawImage != null)
            {
                rawImage.enabled = true;
                rawImage.rectTransform.anchoredPosition = Vector2.zero;
                rawImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
                rawImage.color = to;
            }
        }

        public void Stop()
        {
            var rawImage = GetComponent<RawImage>();
            if (rawImage != null)
            {
                rawImage.enabled = false;
            }
        }

        private void FadeToColor(Color from, Color to, float transition)
        {
            var rawImage = GetComponent<RawImage>();
            if (rawImage != null)
            {
                rawImage.color = Color.Lerp(from, to, transition);
            }
        }
    }
}


