using System.Collections.Generic;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Artifacts.Collection {
    public class Heart : Artifact {
        public override void Initialize() {
            this.Name = "Heart";
            this.Description = new List<string> { $"{{+}}15{{%}}{SpriteEffectMapping.Heart}" };
        }

        public override void Equip(Character character) {
            base.Equip(character);
            character.Stats.IncreaseMaxHealth(0.15f);
        }
    }
}
