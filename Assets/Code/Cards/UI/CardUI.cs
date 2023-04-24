using System.Collections.Generic;
using System.Linq;
using Code.Cards.Collection;
using Code.Characters;
using Code.UI;
using Unity.VisualScripting;
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
            icon.transform.localScale *= 0.8f;

            this.CreateText(this.Card.Name, this.Name, allowMultiline: false, maxTextWidth: this.MaxTextWidth);
            Text cost = this.CreateText($"{this.Card.Cost}{{actionPoint}}", this.CostFrame, allowMultiline: false, maxTextWidth: null);
            cost.transform.localScale *= 0.7f;

            this.UpdateDescription(player);
        }

        public void UpdateDescription(Player player) {
            foreach (Transform child in this.Effects) Destroy(child.gameObject);

            List<string> effects = this.Card.Description(player).SelectMany(s => s).ToList();
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

        public override void Select(bool rotate = true) {
            base.Select(rotate);
            this.TargetPosition = null;
        }

        public override void Deselect(bool rotate = true) {
            base.Deselect(rotate);
            this.TargetPosition = null;
        }

        private Text CreateText(string s, Transform parent, bool allowMultiline, float? maxTextWidth) {
            Text text = Instantiate(this.TextPrefab, parent);
            text.Initialize(s, allowMultiLine: allowMultiline, maxWidth: maxTextWidth);
            return text;
        }
    }
}
