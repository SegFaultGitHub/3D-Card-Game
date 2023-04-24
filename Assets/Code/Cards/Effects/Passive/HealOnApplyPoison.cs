using System.Collections.Generic;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;
using Unity.VisualScripting;

namespace Code.Cards.Effects.Passive {
    public class HealOnApplyPoison : CardEffect {
        private const int PRIORITY = 2;
        private readonly int? Duration;
        private readonly int Value;

        public HealOnApplyPoison(int heal, int? duration = null) {
            this.Value = heal;
            this.Duration = duration;
        }

        public override void UpdateDescription(Player player = null) {
            int value = player == null ? this.Value : player.Compute(null, CallbackType.Heal, player, null, this.Value, PRIORITY);
            this.Description = new List<string> {
                $"Heals {value}{SpriteEffectMapping.Heart} when applying {SpriteEffectMapping.Get(Effect.Poison)}"
            };

            if (this.Duration != null) this.Description.AddRange(TurnsString(this.Duration.Value));
            // int value = player == null ? this.Value : player.Compute(null, CallbackType.Heal, player, null, this.Value, PRIORITY);
            // this.Description = new List<string> {
            //     $"{SpriteEffectMapping.Get(Effect.Poison)} "
            //     + $"{SpriteEffectMapping.Arrow} "
            //     + $"{value}{SpriteEffectMapping.Get(Effect.Heal)}"
            // };
            //
            // if (this.Duration != null) this.Description[0] += TurnsString(this.Duration.Value);
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> _1, Character from, Character to) {
            to.AddTempCallback(new Callback(this.Value), this.Duration);
            return new List<CardEffectValues>();
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            to.AddTempCallback(new Callback(this.Value), this.Duration);
        }

        private class Callback : OnApply {
            private readonly int Heal;

            public Callback(int heal) : base(PRIORITY, CallbackType.Poison) => this.Heal = heal;

            public override int Run(List<CardEffectValues> sideEffects, Character from, Character to, int value) {
                CardEffectValues values = RunEffect(sideEffects, CallbackType.Heal, from, from, this.Heal, this.Priority);
                sideEffects?.Add(values);
                return value;
            }

            public override int Run(SimulationCharacter from, SimulationCharacter to, int value) {
                RunEffect(CallbackType.Damage, from, to, this.Heal, this.Priority);
                return value;
            }
        }
    }
}
