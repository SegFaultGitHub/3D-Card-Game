using System.Collections.Generic;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;
using Unity.VisualScripting;

namespace Code.Cards.Effects.Passive {
    public class ShieldUp : CardEffect {
        private const int PRIORITY = 0;
        private readonly int? Duration;
        private readonly int Value;

        public ShieldUp(int value, int? duration = null) {
            this.Value = value;
            this.Duration = duration;
        }

        public override void UpdateDescription(Player _ = null) {
            this.Description = new[] {
                $"Increase {SpriteEffectMapping.Get(Effect.Shield)} by {this.Value}"
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

        private class Callback : OnCompute {
            private readonly int Value;

            public Callback(int value) : base(PRIORITY, CallbackType.Shield) => this.Value = value;

            public override int Run(List<CardEffectValues> _, Character from, Character to, int value) {
                return value + this.Value;
            }

            public override int Run(SimulationCharacter from, SimulationCharacter to, int value) {
                return value + this.Value;
            }
        }
    }
}
