using Code.Characters;
using UnityEngine;

namespace Code.CharacterSelection {
    public class SelectedCharacter : MonoBehaviour {
        [field: SerializeField] public Player Player { get; private set; }
        [field: SerializeField] public Transform LightFocus { get; private set; }
        [field: SerializeField] public Transform CameraPosition { get; private set; }
        protected Animator Animator;
        private static readonly int DANCE = Animator.StringToHash("Dance");

        private void Awake() {
            this.Animator = this.GetComponentInChildren<Animator>();
        }

        public void Dance() {
            this.Animator.SetTrigger(DANCE);
        }
    }
}
