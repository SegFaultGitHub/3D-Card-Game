using System.Collections.Generic;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Effects;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Artifacts.Collection.Necromancer {
    public class RottenTooth : Artifact {
        public override void Initialize() {
            this.Name = "Rotten Tooth";
            this.Description = new List<string> { $"Increase {SpriteEffectMapping.Get(Effect.Poison)} by {Callback.VALUE}" };
        }

        public override void Equip(Character character) {
            base.Equip(character);
            character.AddCallback(new Callback());
        }


        private class Callback : OnCompute {
            private const int PRIORITY = 0;
            public const int VALUE = 2;

            public Callback() : base(PRIORITY, CallbackType.Poison) { }

            public override int Run(List<CardEffect.CardEffectValues> _, Character from, Character to, int value) {
                return value + VALUE;
            }

            public override int Run(SimulationCharacter from, SimulationCharacter to, int value) {
                return value + VALUE;
            }
        }
    }
}
