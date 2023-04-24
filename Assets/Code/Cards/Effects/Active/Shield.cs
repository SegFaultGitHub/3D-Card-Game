using System.Collections.Generic;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Cards.Effects.Active {
    public class Shield : CardEffect {
        private readonly int Value;
        private readonly bool Self;

        public Shield(int value, bool self = false) {
            this.Value = value;
            this.Self = self;
        }

        public override void UpdateDescription(Player player = null) {
            int value = player == null ? this.Value : player.Compute(null, CallbackType.Shield, player, null, this.Value, short.MaxValue);
            this.Description = new List<string> {
                $"Gains {value}{SpriteEffectMapping.Get(Effect.Shield)}"
            };
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> sideEffects, Character from, Character to) {
            return new List<CardEffectValues> {
                RunEffect(sideEffects, CallbackType.Shield, from, this.Self ? from : to, this.Value, short.MaxValue)
            };
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            RunEffect(CallbackType.Shield, from, to, this.Value, short.MaxValue);
        }
    }
}
