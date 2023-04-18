using System.Collections.Generic;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Effects;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Artifacts.Collection {
    public class CursedRoot : Artifact {
        public override void Initialize() {
            this.Name = "Cursed Root";
            this.Description = new List<string> {
                $"{SpriteEffectMapping.Get(Effect.Poison)} {SpriteEffectMapping.Arrow} 100{{%}}{SpriteEffectMapping.Get(Effect.Damage)}"
            };
        }

        public override void Equip(Character character) {
            base.Equip(character);
            character.AddCallback(new CursedRootCallback());
        }
        
        private class CursedRootCallback : OnApplied {
            private const int PRIORITY = 0;

            public CursedRootCallback() : base(PRIORITY, CallbackType.Poison) { }

            public override int Run(List<CardEffect.CardEffectValues> list, Character from, Character to, int value) {
                CardEffect.CardEffectValues values = CardEffect.RunEffect(list, CallbackType.Damage, from, to, value, this.Priority);
                list?.Add(values);
                return value;
            }

            public override int Run(SimulationCharacter from, SimulationCharacter to, int value) {
                CardEffect.RunEffect(CallbackType.Damage, from, to, value, this.Priority);
                return value;
            }
        }
    }
}
