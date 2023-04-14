using UnityEngine;

namespace Code.Characters.UI {
    public class Bar : MonoBehaviour {
        [field: SerializeField] private RectTransform BarImage;

        public void UpdateRatio(float ratio) {
            ratio = Mathf.Clamp(ratio, 0, 1);
            this.BarImage.localScale = new Vector3(ratio, 1, 1);
        }
    }
}
