using System.Collections.Generic;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Effects;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Artifacts.Collection {
    public class DoubleAxe : Artifact {
        public override void Initialize() {
            this.Name = "Double Axe";
            this.Description = new List<string> { $"{DoubleAxeCallback.VALUE}{SpriteEffectMapping.Get(Effect.Damage, Modifier.Plus)}" };
        }

        public override void Equip(Character character) {
            base.Equip(character);
            character.AddCallback(new DoubleAxeCallback());
        }


        private class DoubleAxeCallback : OnCompute {
            private const int PRIORITY = 0;
            public const int VALUE = 2;

            public DoubleAxeCallback() : base(PRIORITY, CallbackType.Damage) {}

            public override int Run(List<CardEffect.CardEffectValues> _, Character from, Character to, int value) {
                return value + VALUE;
            }

            public override int Run(SimulationCharacter from, SimulationCharacter to, int value) {
                return value + VALUE;
            }
        }
    }
}
