using System.Collections.Generic;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Cards.Effects.Passive {
    public class PoisonUp : CardEffect {
        private const int PRIORITY = 0;
        private readonly int? Duration;
        private readonly int Value;

        public PoisonUp(int value, int? duration = null) {
            this.Value = value;
            this.Duration = duration;
        }

        public override void UpdateDescription(Player _ = null) {
            this.Description = $"{BlueText(this.Value.ToString())}{SpriteEffectMapping.Get(Effect.Poison, Modifier.Plus)}";
            if (this.Duration != null) this.Description += TurnsString(this.Duration.Value);
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> _1, Character from, Character to) {
            to.AddCallback(new PoisonUpCallback(this.Value), this.Duration);
            return new List<CardEffectValues>();
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            to.AddCallback(new PoisonUpCallback(this.Value), this.Duration);
        }

        private class PoisonUpCallback : OnCompute {
            private readonly int Value;

            public PoisonUpCallback(int value) : base(PRIORITY, CallbackType.Poison) => this.Value = value;

            public override int Run(List<CardEffectValues> _, Character from, Character to, int value) {
                return value + this.Value;
            }

            public override int Run(SimulationCharacter from, SimulationCharacter to, int value) {
                return value + this.Value;
            }
        }
    }
}
