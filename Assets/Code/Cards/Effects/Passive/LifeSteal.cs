using System.Collections.Generic;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;
using Unity.VisualScripting;

namespace Code.Cards.Effects.Passive {
    public class LifeSteal : CardEffect {
        private const int PRIORITY = 0;
        private readonly int? Duration;
        private readonly float Ratio;

        public LifeSteal(float ratio, int? duration = null) {
            this.Ratio = ratio;
            this.Duration = duration;
        }

        public override void UpdateDescription(Player _ = null) {
            this.Description = new List<string> {
                $"{SpriteEffectMapping.Get(Effect.Damage)} "
                + $"{SpriteEffectMapping.Arrow} "
                + $"{(int)(this.Ratio * 100)}{{%}}{SpriteEffectMapping.Get(Effect.Heal)}"
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

        private class Callback : OnApplied {
            private readonly float Ratio;

            public Callback(float ratio) : base(PRIORITY, CallbackType.Damage) => this.Ratio = ratio;

            public override int Run(List<CardEffectValues> list, Character from, Character _, int value) {
                CardEffectValues values = RunEffect(list, CallbackType.Heal, from, from, (int)(value * this.Ratio), this.Priority);
                list?.Add(values);
                return value;
            }

            public override int Run(SimulationCharacter from, SimulationCharacter _, int value) {
                RunEffect(CallbackType.Heal, from, from, (int)(value * this.Ratio), this.Priority);
                return value;
            }
        }
    }
}
