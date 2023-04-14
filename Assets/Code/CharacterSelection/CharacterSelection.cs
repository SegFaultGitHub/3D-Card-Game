using Code.Extensions;
using Code.Singleton;
using Code.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.CharacterSelection {
    public class CharacterSelection : WithRaycast {
        [field: SerializeField] private Transform CameraTransform;
        [field: SerializeField] private Transform SpotLight;
        [field: SerializeField] private LayerMask Layer;
        private SelectedCharacter Selected;
        private LTDescr SpotLightTween, CameraMoveTween, CameraForwardTween;

        private bool Complete;
        [field: SerializeField] private string Scene;
        [field: SerializeField] private FadeScreen FadeScreen;

        protected override void Update() {
            if (this.Complete) return;

            base.Update();

            this.GatherInput();

            Hit<SelectedCharacter> hit = this.Raycast<SelectedCharacter>(this.Layer);
            if (this.Input.Choose && hit.Obj != null) {
                this.SelectCharacter();
                this.Complete = true;
            }
            if (hit.Obj == null || hit.Obj == this.Selected)
                return;
            this.Selected = hit.Obj;
            this.MoveSpotLight();
        }

        private void MoveSpotLight() {
            if (this.SpotLightTween != null) LeanTween.cancel(this.SpotLightTween.id);

            Vector3 forward = this.SpotLight.forward;
            this.SpotLight.LookAt(this.Selected.LightFocus);
            Vector3 targetForward = this.SpotLight.forward;
            this.SpotLight.forward = forward;

            this.SpotLightTween = LeanTween.value(this.SpotLight.gameObject, forward, targetForward, 0.3f)
                .setOnUpdateVector3(v => this.SpotLight.forward = v)
                .setEaseOutBack()
                .setOnComplete(() => this.SpotLightTween = null);
        }

        private void SelectCharacter() {
            if (this.CameraMoveTween != null) LeanTween.cancel(this.CameraMoveTween.id);
            if (this.CameraForwardTween != null) LeanTween.cancel(this.CameraForwardTween.id);

            Vector3 forward = this.CameraTransform.forward;
            Vector3 position = this.CameraTransform.position;

            this.CameraTransform.position = this.Selected.CameraPosition.position;
            this.CameraTransform.LookAt(this.Selected.LightFocus);

            Vector3 targetPosition = this.CameraTransform.position;
            Vector3 targetForward = this.CameraTransform.forward;

            this.CameraTransform.forward = forward;
            this.CameraTransform.position = position;

            const float transitionDuration = 0.75f;

            this.Selected.Dance();
            Static.Player = this.Selected.Player;

            this.CameraForwardTween = LeanTween.value(this.CameraTransform.gameObject, forward, targetForward, transitionDuration)
                .setOnUpdateVector3(v => this.CameraTransform.forward = v)
                .setEaseOutQuad()
                .setOnComplete(() => this.CameraForwardTween = null);
            this.CameraMoveTween = LeanTween.move(this.CameraTransform.gameObject, targetPosition, transitionDuration)
                .setEaseOutQuad()
                .setOnComplete(() => this.CameraMoveTween = null);

            this.InSeconds(transitionDuration, () => this.FadeScreen.Fade(1f).setOnComplete(() => SceneManager.LoadScene(this.Scene)));
        }

        #region Input
        private class _Input {
            public bool Choose;
        }
        private _Input Input = new();

        protected override void OnEnable() {
            base.OnEnable();
            this.InputActions.CharacterSelection.Enable();
        }

        protected override void OnDisable() {
            base.OnDisable();
            this.InputActions.CharacterSelection.Disable();
        }

        private void GatherInput() {
            this.Input = new _Input { Choose = this.InputActions.CharacterSelection.Choose.WasReleasedThisFrame() };
        }
        #endregion
    }
}
