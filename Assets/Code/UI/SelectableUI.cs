using Code.Characters;
using UnityEngine;

namespace Code.UI {
    public class SelectableUI : MonoBehaviour {
        [field: SerializeField] private GameObject ExtendedCollider;

        [field: SerializeField] public float DeselectSize { get; set; }
        [field: SerializeField] public float SelectSize { get; set; }
        [field: SerializeField] private float DepthOffset;
        public Vector3 InitialPosition { private get; set; }
        public Vector3 InitialAngle { private get; set; }
        private LTDescr ScaleTween, MoveTween, RotateTween;

        public virtual void Initialize(Player player = null) {
            this.transform.localScale = new Vector3(this.DeselectSize, this.DeselectSize, this.DeselectSize);
        }

        public virtual void Select(bool rotate = true) {
            this.ExtendedCollider.SetActive(true);
            this.Scale(this.SelectSize);
            this.Move(this.InitialPosition + new Vector3(0, 0, this.DepthOffset));
            if (rotate) this.Rotate(Vector3.zero);
        }

        public virtual void Deselect(bool rotate = true) {
            this.ExtendedCollider.gameObject.SetActive(false);
            this.Scale(this.DeselectSize);
            this.Move(this.InitialPosition);
            if (rotate) this.Rotate(this.InitialAngle);
        }

        protected LTDescr Scale(float scale, float duration = 0.25f) {
            if (this.ScaleTween != null) LeanTween.cancel(this.ScaleTween.id);
            this.ScaleTween = LeanTween.scale(this.gameObject, new Vector3(scale, scale, scale), duration)
                .setEaseOutBack()
                .setOnComplete(() => this.ScaleTween = null);
            return this.ScaleTween;
        }

        public LTDescr Move(Vector3 position) {
            if (this.MoveTween != null) LeanTween.cancel(this.MoveTween.id);
            this.MoveTween = LeanTween.moveLocal(this.gameObject, position, 0.25f)
                .setEaseOutBack()
                .setOnComplete(() => this.MoveTween = null);
            return this.MoveTween;
        }

        public LTDescr Rotate(Vector3 angles) {
            if (this.RotateTween != null) LeanTween.cancel(this.RotateTween.id);
            this.RotateTween = LeanTween.rotateLocal(this.gameObject, angles, 0.25f)
                .setEaseOutBack()
                .setOnComplete(() => this.RotateTween = null);
            return this.RotateTween;
        }

        public LTDescr Hide() {
            return this.Scale(0, 0.1f);
        }

        public LTDescr Show() {
            return this.Scale(this.DeselectSize, 0.1f);
        }
    }
}
