using System.Collections.Generic;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Cards.Effects.Active {
    public class Damage : CardEffect {
        private readonly int Value;

        public Damage(int value) => this.Value = value;

        public override void UpdateDescription(Player player = null) {
            int value = player == null ? this.Value : player.Compute(null, CallbackType.Damage, player, null, this.Value, short.MaxValue);
            if (value > this.Value)
                this.Description = $"{GreenText(value)}{SpriteEffectMapping.Get(Effect.Damage)}";
            else if (value < this.Value)
                this.Description = $"{RedText(value)}{SpriteEffectMapping.Get(Effect.Damage)}";
            else
                this.Description = $"{BlueText(value)}{SpriteEffectMapping.Get(Effect.Damage)}";
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> sideEffects, Character from, Character to) {
            return new List<CardEffectValues> {
                RunEffect(sideEffects, CallbackType.Damage, from, to, this.Value, short.MaxValue)
            };
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            RunEffect(CallbackType.Damage, from, to, this.Value, short.MaxValue);
        }
    }
}
