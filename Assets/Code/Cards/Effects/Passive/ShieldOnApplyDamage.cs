using System.Collections.Generic;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;
using Unity.VisualScripting;

namespace Code.Cards.Effects.Passive {
    public class ShieldOnApplyDamage : CardEffect {
        private const int PRIORITY = 3;
        private readonly int? Duration;
        private readonly int Value;

        public ShieldOnApplyDamage(int shield, int? duration = null) {
            this.Value = shield;
            this.Duration = duration;
        }

        public override void UpdateDescription(Player player = null) {
            int value = player == null ? this.Value : player.Compute(null, CallbackType.Shield, player, null, this.Value, PRIORITY);
            this.Description = new List<string> {
                $"Gains {value}{SpriteEffectMapping.Get(Effect.Shield)} when dealing {SpriteEffectMapping.Get(Effect.Damage)}"
            };
            if (this.Duration != null) this.Description.AddRange(TurnsString(this.Duration.Value));
            // int value = player == null ? this.Value : player.Compute(null, CallbackType.Shield, player, null, this.Value, PRIORITY);
            // this.Description = $"{SpriteEffectMapping.Get(Effect.Damage)} "
            //                    + $"{SpriteEffectMapping.Arrow} "
            //                    + $"{value}{SpriteEffectMapping.Get(Effect.Shield)}";
            //
            // if (this.Duration != null) this.Description += TurnsString(this.Duration.Value);
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> _, Character from, Character to) {
            to.AddTempCallback(new Callback(this.Value), this.Duration);
            return new List<CardEffectValues>();
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            to.AddTempCallback(new Callback(this.Value), this.Duration);
        }

        private class Callback : OnApply {
            private readonly int Shield;

            public Callback(int shield) : base(PRIORITY, CallbackType.Damage) => this.Shield = shield;

            public override int Run(List<CardEffectValues> list, Character from, Character _, int value) {
                CardEffectValues values = RunEffect(list, CallbackType.Shield, from, from, this.Shield, this.Priority);
                list?.Add(values);
                return value;
            }

            public override int Run(SimulationCharacter from, SimulationCharacter _, int value) {
                RunEffect(CallbackType.Shield, from, from, this.Shield, this.Priority);
                return value;
            }
        }
    }
}
