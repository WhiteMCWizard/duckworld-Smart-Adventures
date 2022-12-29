using UnityEngine;
using UnityEngine.UI;

namespace CinemaDirector
{
    [CutsceneItem("Transitions", "Fade to Black", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
    public class FadeToBlack : CinemaGlobalAction
    {
        private Color From = Color.clear;

        private Color To = Color.black;

        private void Awake()
        {
            RawImage component = base.gameObject.GetComponent<RawImage>();
            if (component == null)
            {
                component = base.gameObject.AddComponent<RawImage>();
                base.gameObject.transform.position = Vector3.zero;
                base.gameObject.transform.localScale = new Vector3(100f, 100f, 100f);
                component.texture = Texture2D.whiteTexture;
                component.enabled = false;
                component.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
                component.color = Color.clear;
            }
        }

        public override void Trigger()
        {
            RawImage component = base.gameObject.GetComponent<RawImage>();
            if (component != null)
            {
                component.enabled = true;
                component.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
                component.color = From;
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
            RawImage component = base.gameObject.GetComponent<RawImage>();
            if (component != null)
            {
                if (time >= 0f && time <= base.Duration)
                {
                    component.enabled = true;
                    UpdateTime(time, deltaTime);
                }
                else if (component.enabled)
                {
                    component.enabled = false;
                }
            }
        }

        public override void End()
        {
            RawImage component = base.gameObject.GetComponent<RawImage>();
            if (component != null)
            {
                component.enabled = false;
            }
        }

        public override void ReverseEnd()
        {
            RawImage component = base.gameObject.GetComponent<RawImage>();
            if (component != null)
            {
                component.enabled = true;
                component.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
                component.color = To;
            }
        }

        public override void Stop()
        {
            RawImage component = base.gameObject.GetComponent<RawImage>();
            if (component != null)
            {
                component.enabled = false;
            }
        }
        private void FadeToColor(Color from, Color to, float transition)
        {
            RawImage component = base.gameObject.GetComponent<RawImage>();
            if (component != null)
            {
                component.color = Color.Lerp(from, to, transition);
            }
        }
    }
}

