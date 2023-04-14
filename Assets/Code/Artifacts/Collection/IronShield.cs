using System.Collections.Generic;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Effects;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Artifacts.Collection {
    public class IronShield : Artifact {
        public override void Initialize() {
            this.Name = "Iron Shield";
            this.Description = new List<string> {
                $"{SpriteEffectMapping.Get(Effect.Damage)} "
                + $"{SpriteEffectMapping.Arrow} "
                + $"{(int) (IronShieldCallback.RATIO * 100)}{{%}}{SpriteEffectMapping.Get(Effect.Shield)}"
            };
        }

        public override void Equip(Character character) {
            base.Equip(character);
            character.AddCallback(new IronShieldCallback());
        }

        private class IronShieldCallback : OnApplied {
            private const int PRIORITY = 0;
            public const float RATIO = 0.25f;

            public IronShieldCallback() : base(PRIORITY, CallbackType.Damage) { }

            public override int Run(List<CardEffect.CardEffectValues> list, Character from, Character to, int value) {
                CardEffect.CardEffectValues values = CardEffect.RunEffect(list, CallbackType.Shield, from, from, (int)(value * RATIO), this.Priority);
                list?.Add(values);
                return value;
            }

            public override int Run(SimulationCharacter from, SimulationCharacter to, int value) {
                CardEffect.RunEffect(CallbackType.Poison, from, to, value, this.Priority);
                return value;
            }
        }
    }
}
