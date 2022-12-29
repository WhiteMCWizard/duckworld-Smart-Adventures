using UnityEngine;
using UnityEngine.UI;

namespace CinemaDirector
{
    [ExecuteInEditMode]
    public class FadeFromWhite : CinemaGlobalAction
    {
        private Color From = Color.white;

        private Color To = Color.clear;

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

        public override void Trigger()
        {
            var rawImage = GetComponent<RawImage>();
            if (rawImage != null)
            {
                rawImage.enabled = true;
                rawImage.rectTransform.anchoredPosition = Vector2.zero;
                rawImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
                rawImage.color = From;
            }
        }

        public override void ReverseTrigger()
        {
            End();
        }

        public override void UpdateTime(float time, float deltaTime)
        {
            float transition = time / duration;
            FadeToColor(From, To, transition);
        }

        public override void SetTime(float time, float deltaTime)
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

        public override void End()
        {
            var rawImage = GetComponent<RawImage>();
            if (rawImage != null)
            {
                rawImage.enabled = false;
            }
        }

        public override void ReverseEnd()
        {
            var rawImage = GetComponent<RawImage>();
            if (rawImage != null)
            {
                rawImage.enabled = true;
                rawImage.rectTransform.anchoredPosition = Vector2.zero;
                rawImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
                rawImage.color = To;
            }
        }
        public override void Stop()
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
