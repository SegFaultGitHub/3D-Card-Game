using System;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI {
    public class FadeScreen : MonoBehaviour {
        private Image Image;
        [field: SerializeField] private bool UnfadeOnAwake;
        [field: SerializeField, ConditionalField(nameof(UnfadeOnAwake))] private float Duration;


        private void Awake() {
            this.Image = this.GetComponentInChildren<Image>();
            if (this.UnfadeOnAwake) {
                Color color = this.Image.color;
                this.Image.color = new Color(color.r, color.g, color.b, 1);
                this.Unfade(this.Duration);
            }
        }

        public LTDescr Fade(float duration) {
            Color color = this.Image.color;
            return LeanTween.value(0, 1, duration).setOnUpdate(alpha => this.Image.color = new Color(color.r, color.g, color.b, alpha));
        }

        public LTDescr Unfade(float duration) {
            Color color = this.Image.color;
            return LeanTween.value(1, 0, duration).setOnUpdate(alpha => this.Image.color = new Color(color.r, color.g, color.b, alpha));
        }

        public void Fade(float duration, Action action) {
            this.Fade(duration)
                .setOnComplete(
                    () => {
                        action();
                        this.Unfade(duration);
                    }
                );
        }
    }
}
