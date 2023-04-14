using System.Collections.Generic;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Artifacts.Collection {
    public class NecklaceOfSwiftness : Artifact {
        public override void Initialize() {
            this.Name = "Necklace Of Swiftness";
            this.Description = new List<string> { $"{{+}}1{SpriteEffectMapping.Get(Effect.ActionPoint)}" };
        }

        public override void Equip(Character character) {
            base.Equip(character);
            character.Stats.ActionPoints += 1;
        }
    }
}
