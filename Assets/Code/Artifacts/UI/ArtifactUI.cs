using System.Collections.Generic;
using System.Linq;
using Code.Characters;
using Code.UI;
using UnityEngine;

namespace Code.Artifacts.UI {
    public class ArtifactUI : SelectableUI {
        [field: SerializeField] private Transform Background;
        [field: SerializeField] private Transform Name;
        [field: SerializeField] private Transform IconFrame;
        [field: SerializeField] private Transform Effects;
        [field: SerializeField] private Text TextPrefab;
        private Vector2 Size;

        [field: SerializeField] public Artifact Artifact { get; set; }
        private float MaxTextWidth => this.Size.x * 0.9f;

        private void Awake() {
            this.Size = this.Background.localScale;
            this.Background.gameObject.SetActive(false);
        }

        public override void Initialize(Player player = null) {
            base.Initialize(player);
            this.Artifact.Initialize();

            GameObject icon = Instantiate(this.Artifact.Icon, this.IconFrame);
            icon.layer = this.gameObject.layer;
            icon.transform.localPosition = new Vector3(0, 0, 0.025f);

            this.CreateText(this.Artifact.Name, this.Name, allowMultiline: false, maxTextWidth: this.MaxTextWidth);

            this.UpdateDescription();
        }

        public void UpdateDescription() {
            foreach (Transform child in this.Effects) Destroy(child.gameObject);

            List<string> effects = this.Artifact.Description;
            List<Text> texts = effects.Select(
                    effect => this.CreateText(effect, this.Effects, allowMultiline: true, maxTextWidth: this.MaxTextWidth)
                )
                .ToList();

            if (texts.Count <= 1)
                return;

            float totalHeight = texts.Sum(text => text.Height);
            texts[0].transform.localPosition = new Vector3(0, totalHeight / 2 - texts[0].Height / 2, 0);
            for (int i = 1; i < texts.Count; i++) {
                texts[i].transform.localPosition = new Vector3(
                    0,
                    texts[i - 1].transform.localPosition.y - texts[i - 1].Height / 2 - texts[i].Height / 2,
                    0
                );
            }
        }

        private Text CreateText(string s, Transform parent, bool allowMultiline, float? maxTextWidth) {
            Text text = Instantiate(this.TextPrefab, parent);
            text.Initialize(s, allowMultiLine: allowMultiline, maxWidth: maxTextWidth);
            return text;
        }
    }
}
