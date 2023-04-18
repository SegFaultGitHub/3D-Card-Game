using System.Collections.Generic;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;
using Unity.VisualScripting;

namespace Code.Cards.Effects.Passive {
    public class DamageOnApplyPoison : CardEffect {
        private const int PRIORITY = 3;
        private readonly int? Duration;
        private readonly int Value;

        public DamageOnApplyPoison(int damage, int? duration = null) {
            this.Value = damage;
            this.Duration = duration;
        }

        public override void UpdateDescription(Player player = null) {
            int value = player == null ? this.Value : player.Compute(null, CallbackType.Damage, player, null, this.Value, PRIORITY);
            this.Description = new[] {
                $"Deals {value}{SpriteEffectMapping.Get(Effect.Damage)} when applying {SpriteEffectMapping.Get(Effect.Poison)}"
            };
            if (this.Duration != null) this.Description.AddRange(TurnsString(this.Duration.Value));
            // int value = player == null ? this.Value : player.Compute(null, CallbackType.Damage, player, null, this.Value, PRIORITY);
            // this.Description = $"{SpriteEffectMapping.Get(Effect.Poison)} "
            //                    + $"{SpriteEffectMapping.Arrow} "
            //                    + $"{value}{SpriteEffectMapping.Get(Effect.Damage)}";
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
            private readonly int Damage;

            public Callback(int damage) : base(PRIORITY, CallbackType.Poison) => this.Damage = damage;

            public override int Run(List<CardEffectValues> sideEffects, Character from, Character to, int value) {
                CardEffectValues values = RunEffect(sideEffects, CallbackType.Damage, from, to, this.Damage, this.Priority);
                sideEffects?.Add(values);
                return value;
            }

            public override int Run(SimulationCharacter from, SimulationCharacter to, int value) {
                RunEffect(CallbackType.Damage, from, to, this.Damage, this.Priority);
                return value;
            }
        }
    }
}
