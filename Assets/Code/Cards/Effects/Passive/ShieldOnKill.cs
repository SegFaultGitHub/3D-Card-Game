using System.Collections.Generic;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;
using Unity.VisualScripting;

namespace Code.Cards.Effects.Passive {
    public class ShieldOnKill : CardEffect {
        private const int PRIORITY = 0;
        private readonly int? Duration;
        private readonly int Value;

        public ShieldOnKill(int value, int? duration = null) {
            this.Value = value;
            this.Duration = duration;
        }

        public override void UpdateDescription(Player player = null) {
            int value = player == null ? this.Value : player.Compute(null, CallbackType.Shield, player, null, this.Value, PRIORITY);
            this.Description = new[] {
                $"Gains {value}{SpriteEffectMapping.Get(Effect.Shield)} on kill"
            };
            if (this.Duration != null) this.Description.AddRange(TurnsString(this.Duration.Value));
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> _, Character from, Character to) {
            to.AddTempCallback(new Callback(this.Value), this.Duration);
            return new List<CardEffectValues>();
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            to.AddTempCallback(new Callback(this.Value), this.Duration);
        }

        private class Callback : OnApplied {
            private readonly int Shield;

            public Callback(int shield) : base(PRIORITY, CallbackType.Damage) {
                this.Shield = shield;
            }

            public override int Run(List<CardEffectValues> list, Character from, Character to, int value) {
                if (!to.Stats.Dead)
                    return value;
                CardEffectValues values = RunEffect(list, CallbackType.Shield, from, from, this.Shield, this.Priority);
                list?.Add(values);
                return value;
            }

            public override int Run(SimulationCharacter _, SimulationCharacter to, int value) {
                if (!to.Stats.Dead)
                    return value;
                RunEffect(CallbackType.Shield, to, to, this.Shield, this.Priority);
                return value;
            }
        }
    }
}
