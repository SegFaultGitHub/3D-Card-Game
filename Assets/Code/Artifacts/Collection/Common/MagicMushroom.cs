using System.Collections.Generic;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Artifacts.Collection {
    public class MagicMushroom : Artifact {
        private const float HP_RATIO = 0.1f;
        private const int AP_VALUE = 1;

        public override void Initialize() {
            this.Name = "Magic Mushroom";
            this.Description = new List<string> {
                $"Adds {(int)(HP_RATIO * 100)}{{%}}{SpriteEffectMapping.Heart}",
                $"Adds {AP_VALUE}{SpriteEffectMapping.Get(Effect.ActionPoint)}"
            };
        }

        public override void Equip(Character character) {
            base.Equip(character);
            character.Stats.IncreaseMaxHealth(HP_RATIO);
            character.Stats.ActionPoints += AP_VALUE;
        }
    }
}
