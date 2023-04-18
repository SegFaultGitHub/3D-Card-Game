using System.Collections.Generic;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;
using Unity.VisualScripting;

namespace Code.Cards.Effects.Passive {
    public class DamageMultiplier : CardEffect {
        private const int PRIORITY = 0;
        private readonly int? Duration;
        private readonly float Ratio;

        public DamageMultiplier(float ratio, int? duration = null) {
            this.Ratio = ratio;
            this.Duration = duration;
        }

        public override void UpdateDescription(Player _ = null) {
            this.Description = new[] {
                $"Increase {SpriteEffectMapping.Get(Effect.Damage)} by {(int)(this.Ratio * 100)}{{%}}"
            };
            if (this.Duration != null) this.Description.AddRange(TurnsString(this.Duration.Value));
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> _, Character from, Character to) {
            to.AddTempCallback(new Callback(this.Ratio), this.Duration);
            return new List<CardEffectValues>();
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            to.AddTempCallback(new Callback(this.Ratio), this.Duration);
        }

        private class Callback : OnCompute {
            private readonly float Ratio;

            public Callback(float ratio) : base(PRIORITY, CallbackType.Damage) => this.Ratio = ratio;

            public override int Run(List<CardEffectValues> _, Character from, Character to, int value) {
                return (int)(value * this.Ratio);
            }

            public override int Run(SimulationCharacter from, SimulationCharacter to, int value) {
                return (int)(value * this.Ratio);
            }
        }
    }
}
