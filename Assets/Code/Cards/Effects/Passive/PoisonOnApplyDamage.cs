using System.Collections.Generic;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Cards.Effects.Passive {
    public class PoisonOnDamage : CardEffect {
        private const int PRIORITY = 3;
        private readonly int? Duration;
        private readonly int Value;

        public PoisonOnDamage(int poison, int? duration = null) {
            this.Value = poison;
            this.Duration = duration;
        }

        public override void UpdateDescription(Player player = null) {
            int value = player == null ? this.Value : player.Compute(null, CallbackType.Poison, player, null, this.Value, PRIORITY);
            this.Description = $"{SpriteEffectMapping.Get(Effect.Damage)} "
                               + $"{SpriteEffectMapping.Arrow} "
                               + $"{value}{SpriteEffectMapping.Get(Effect.Poison)}";

            if (this.Duration != null) this.Description += TurnsString(this.Duration.Value);
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> _, Character from, Character to) {
            to.AddTempCallback(new Callback(this.Value), this.Duration);
            return new List<CardEffectValues>();
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            to.AddTempCallback(new Callback(this.Value), this.Duration);
        }

        private class Callback : OnApply {
            private readonly int Poison;

            public Callback(int poison) : base(PRIORITY, CallbackType.Damage) => this.Poison = poison;

            public override int Run(List<CardEffectValues> list, Character from, Character to, int value) {
                CardEffectValues values = RunEffect(list, CallbackType.Poison, from, to, this.Poison, this.Priority);
                list?.Add(values);
                return value;
            }

            public override int Run(SimulationCharacter from, SimulationCharacter to, int value) {
                RunEffect(CallbackType.Poison, from, to, this.Poison, this.Priority);
                return value;
            }
        }
    }
}
