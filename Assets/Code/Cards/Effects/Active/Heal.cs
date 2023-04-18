using System.Collections.Generic;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Cards.Effects.Active {
    public class Heal : CardEffect {
        private readonly int Value;
        private readonly bool Self;

        public Heal(int value, bool self = false) {
            this.Value = value;
            this.Self = self;
        }

        public override void UpdateDescription(Player player = null) {
            int value = player == null ? this.Value : player.Compute(null, CallbackType.Heal, player, null, this.Value, short.MaxValue);
            this.Description = new[] {
                $"Heals {value}{SpriteEffectMapping.Heart}"
            };
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> sideEffects, Character from, Character to) {
            return new List<CardEffectValues> {
                RunEffect(sideEffects, CallbackType.Heal, from, this.Self ? from : to, this.Value, short.MaxValue)
            };
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            RunEffect(CallbackType.Heal, from, to, this.Value, short.MaxValue);
        }
    }
}
