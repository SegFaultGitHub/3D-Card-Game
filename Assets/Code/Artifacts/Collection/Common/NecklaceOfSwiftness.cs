using System.Collections.Generic;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Artifacts.Collection {
    public class NecklaceOfSwiftness : Artifact {
        private const int VALUE = 1;

        public override void Initialize() {
            this.Name = "Necklace Of Swiftness";
            this.Description = new List<string> { $"Adds {VALUE}{SpriteEffectMapping.Get(Effect.ActionPoint)}" };
        }

        public override void Equip(Character character) {
            base.Equip(character);
            character.Stats.ActionPoints += VALUE;
        }
    }
}
