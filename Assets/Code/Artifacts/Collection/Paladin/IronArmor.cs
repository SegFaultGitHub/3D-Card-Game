using System.Collections.Generic;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Effects;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Artifacts.Collection {
    public class IronArmor : Artifact {
        public override void Initialize() {
            this.Name = "Iron Armor";
            this.Description = new List<string> {
                $"{IronArmorCallback.VALUE}{SpriteEffectMapping.Get(Effect.Shield)}{SpriteEffectMapping.Turn}"
            };
        }

        public override void Equip(Character character) {
            base.Equip(character);
            character.AddCallback(new IronArmorCallback());
        }

        private class IronArmorCallback : OnTurnEnds {
            private const int PRIORITY = 0;
            public const int VALUE = 2;

            public IronArmorCallback() : base(PRIORITY) { }

            public override IEnumerable<CardEffect.CardEffectValues> Run(Character character) {
                return new List<CardEffect.CardEffectValues> {
                    CardEffect.RunEffect(null, CallbackType.Shield, character, character, VALUE, short.MaxValue)
                };
            }

            public override void Run(SimulationCharacter character) {
                throw new System.NotImplementedException();
            }
        }
    }
}
