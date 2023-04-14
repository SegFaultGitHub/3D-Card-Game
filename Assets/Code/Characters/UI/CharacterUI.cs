using TMPro;
using UnityEngine;

namespace Code.Characters.UI {
    public class CharacterUI : MonoBehaviour {
        [field: SerializeField] private Character Character;
        [field: SerializeField] private TMP_Text Name;
        [field: SerializeField] private Bar HealthBar;
        [field: SerializeField] private Bar ActionPointsBar;


        [field: SerializeField] private float TextOffset;
        [field: SerializeField] private float BarOffset;


        private void Awake() {
            this.SetName();

            this.Arrange();
        }

        private void Update() {
            this.HealthBar.UpdateRatio(this.Character.Stats.HealthRatio);
            this.ActionPointsBar.UpdateRatio(this.Character.Stats.ActionPointsRatio);
        }

        private void SetName() {
            this.Name.SetText(this.Character.Name);
        }

        private void Arrange() {
            float height = 0;
            this.Name.ForceMeshUpdate();
            RectTransform r;

            // Name
            height += this.TextOffset;
            r = (RectTransform)this.Name.transform;
            r.sizeDelta = new Vector2(r.sizeDelta.x, this.Name.textBounds.size.y);
            r.anchoredPosition = new Vector2(r.anchoredPosition.x, -height);
            height += r.sizeDelta.y;
            height += this.TextOffset;

            // Health bar
            r = (RectTransform)this.HealthBar.transform;
            r.anchoredPosition = new Vector2(r.anchoredPosition.x, -height);
            height += r.sizeDelta.y;
            height += this.BarOffset;

            // Action points bar
            r = (RectTransform)this.ActionPointsBar.transform;
            r.anchoredPosition = new Vector2(r.anchoredPosition.x, -height);
            height += r.sizeDelta.y;
            height += this.BarOffset;

            r = (RectTransform)this.transform;
            r.sizeDelta = new Vector2(r.sizeDelta.x, height);
        }
    }
}
