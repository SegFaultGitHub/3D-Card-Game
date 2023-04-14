using UnityEngine;

namespace Code.Characters {
    public class CharacterCustomization : MonoBehaviour {
        [field: SerializeField] private Transform HandSlotLeft;
        [field: SerializeField] private Transform HandSlotRight;
        [field: SerializeField] public Transform Head { get; private set; }
        [field: SerializeField] public Transform LeftArm { get; private set; }
        [field: SerializeField] public Transform RightArm { get; private set; }
        [field: SerializeField] public Transform Torso { get; private set; }


        public void CustomizeHead(GameObject head) {
            Customize(this.Head, head);
        }

        public void CustomizeLeftArm(GameObject leftArm) {
            Customize(this.LeftArm, leftArm);
        }

        public void CustomizeRightArm(GameObject rightArm) {
            Customize(this.RightArm, rightArm);
        }

        public void CustomizeTorso(GameObject torso) {
            Customize(this.Torso, torso);
        }

        public void CustomizeHandSlotLeft(GameObject item) {
            Customize(this.HandSlotLeft, item);
        }

        public void CustomizeHandSlotRight(GameObject item) {
            Customize(this.HandSlotRight, item);
        }

        private static void Customize(Transform parent, GameObject obj) {
            foreach (Transform child in parent) {
                Destroy(child.gameObject);
            }
            Instantiate(obj, parent);
        }
    }
}
