using System.Collections.Generic;
using System.Linq;
using Code.Cards.Collection;
using Code.Characters;
using Code.UI;
using UnityEngine;

namespace Code.Cards.UI {
    public class CardUI : SelectableUI {
        [field: SerializeField] private Transform Background;
        [field: SerializeField] private Transform Name;
        [field: SerializeField] private Transform IconFrame;
        [field: SerializeField] private Transform CostFrame;
        [field: SerializeField] private Transform Effects;
        [field: SerializeField] private Text TextPrefab;
        [field: SerializeField] private float Speed;
        private Vector2 Size;

        [field: SerializeField] public Card Card { get; set; }
        private float MaxTextWidth => this.Size.x * 0.9f;

        public Vector3? TargetPosition { private get; set; }
        public bool TargetPositionReached { private get; set; }

        private void Awake() {
            this.Size = this.Background.localScale;
            this.Background.gameObject.SetActive(false);
        }

        private void Update() {
            if (this.TargetPosition != null) {
                if (this.TargetPositionReached) {
                    this.transform.position = this.TargetPosition.Value;
                } else {
                    Vector3 direction = this.TargetPosition.Value - this.transform.position;
                    if (direction.magnitude < this.Speed) {
                        this.TargetPositionReached = true;
                    } else {
                        this.transform.position += direction.normalized * this.Speed;
                    }
                }
            }
        }

        public override void Initialize(Player player = null) {
            base.Initialize(player);
            this.Card.Initialize();

            GameObject icon = Instantiate(this.Card.Icon, this.IconFrame);
            icon.layer = this.gameObject.layer;
            icon.transform.localPosition = new Vector3(0, 0, 0.025f);

            this.CreateTextLine(this.Card.Name, this.Name);
            Line cost = this.CreateTextLine($"{this.Card.Cost}{{actionPoint}}", this.CostFrame);
            cost.Transform.localScale *= 0.9f;

            this.UpdateDescription(player);
        }

        public void UpdateDescription(Player player) {
            foreach (Transform child in this.Effects) Destroy(child.gameObject);

            List<string> effects = this.Card.Description(player).ToList();
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

        public override void Select(bool rotate = true) {
            base.Select(rotate);
            this.TargetPosition = null;
        }

        public override void Deselect(bool rotate = true) {
            base.Deselect(rotate);
            this.TargetPosition = null;
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
