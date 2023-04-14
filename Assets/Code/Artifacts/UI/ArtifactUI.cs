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

            this.CreateTextLine(this.Artifact.Name, this.Name);

            this.UpdateDescription();
        }

        public void UpdateDescription() {
            foreach (Transform child in this.Effects) Destroy(child.gameObject);

            List<string> effects = this.Artifact.Description;
            List<Line> lines = effects.Select(effect => this.CreateTextLine(effect, this.Effects)).ToList();
            float totalHeight = lines.Sum(line => line.Height);

            for (int i = 1; i < lines.Count; i++) {
                lines[i].Transform.localPosition = new Vector3(
                    0,
                    lines[i - 1].Transform.localPosition.y - lines[i - 1].Height / 2 - lines[i].Height / 2,
                    0.025f
                );
            }
            foreach (Line line in lines) {
                Vector3 localPosition = line.Transform.localPosition;
                localPosition = new Vector3(0, localPosition.y + totalHeight / 2 - lines[0].Height / 2, 0.025f);
                line.Transform.localPosition = localPosition;
            }
        }

        private Line CreateTextLine(string s, Transform parent) {
            Text text = Instantiate(this.TextPrefab, parent);
            text.Initialize(s);
            float scale = 1;
            if (text.Width > this.MaxTextWidth) {
                scale = this.MaxTextWidth / text.Width;
                text.transform.localScale = new Vector3(scale, scale, scale);
            }

            return new Line {
                Height = text.Height * scale,
                Transform = text.transform
            };
        }
        private struct Line {
            public float Height;
            public Transform Transform;
        }
    }
}
