using System.Collections.Generic;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Artifacts.Collection {
    public class MagicMushroom : Artifact {
        public override void Initialize() {
            this.Name = "Magic Mushroom";
            this.Description = new List<string> { $"{{+}}10{{%}}{SpriteEffectMapping.Heart}", $"{{+}}1{SpriteEffectMapping.Get(Effect.ActionPoint)}" };
        }

        public override void Equip(Character character) {
            base.Equip(character);
            character.Stats.IncreaseMaxHealth(0.10f);
            character.Stats.ActionPoints += 1;
        }
    }
}
