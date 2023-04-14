using UnityEngine;

namespace Code.UI {
    public class EffectText : Text {
        protected override void Awake() {
            base.Awake();
            this.transform.localScale *= 0;
            LeanTween.scale(this.gameObject, Vector3.one, 0.2f).setEaseOutBack();
            LeanTween.scale(this.gameObject, Vector3.zero, 0.2f).setDelay(0.5f).setEaseInBack().setDestroyOnComplete(true);
        }
    }
}
