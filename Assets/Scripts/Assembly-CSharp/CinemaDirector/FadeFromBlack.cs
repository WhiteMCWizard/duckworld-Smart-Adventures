using UnityEngine;
using UnityEngine.UI;

namespace CinemaDirector
{
    [CutsceneItem("Transitions", "Fade from Black", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
    public class FadeFromBlack : CinemaGlobalAction
    {
        private Color From = Color.black;

        private Color To = Color.clear;

        private Image image;

        private void Awake()
        {
            image = base.gameObject.GetComponent<Image>();
            if (image == null)
            {
                image = base.gameObject.AddComponent<Image>();
                base.gameObject.transform.position = Vector3.zero;
                base.gameObject.transform.localScale = new Vector3(100f, 100f, 100f);
                image.enabled = false;
                image.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
                image.color = Color.clear;
            }
        }

        public override void Trigger()
        {
            if (image != null)
            {
                image.enabled = true;
                image.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
                image.color = From;
            }
        }

        public override void ReverseTrigger()
        {
            End();
        }

        public override void UpdateTime(float time, float deltaTime)
        {
            float transition = time / base.Duration;
            FadeToColor(From, To, transition);
        }

        public override void SetTime(float time, float deltaTime)
        {
            if (image != null)
            {
                if (time >= 0f && time <= base.Duration)
                {
                    image.enabled = true;
                    UpdateTime(time, deltaTime);
                }
                else if (image.enabled)
                {
                    image.enabled = false;
                }
            }
        }

        public override void End()
        {
            if (image != null)
            {
                image.enabled = false;
            }
        }

        public override void ReverseEnd()
        {
            if (image != null)
            {
                image.enabled = true;
                image.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
                image.color = To;
            }
        }

        public override void Stop()
        {
            if (image != null)
            {
                image.enabled = false;
            }
        }

        private void FadeToColor(Color from, Color to, float transition)
        {
            if (image != null)
            {
                image.color = Color.Lerp(from, to, transition);
            }
        }
    }
}
