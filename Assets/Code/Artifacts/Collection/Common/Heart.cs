using System.Collections.Generic;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Artifacts.Collection {
    public class Heart : Artifact {
        private const float RATIO = 0.15f;

        public override void Initialize() {
            this.Name = "Heart";
            this.Description = new List<string> { $"Adds {(int) (RATIO * 100)}{{%}}{SpriteEffectMapping.Heart}" };
        }

        public override void Equip(Character character) {
            base.Equip(character);
            character.Stats.IncreaseMaxHealth(RATIO);
        }
    }
}
