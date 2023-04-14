using System.Collections.Generic;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Cards.Effects.Passive {
    public class DamageUp : CardEffect {
        private const int PRIORITY = 0;
        private readonly int? Duration;
        private readonly int Value;

        public DamageUp(int value, int? duration = null) {
            this.Value = value;
            this.Duration = duration;
        }

        public override void UpdateDescription(Player _ = null) {
            this.Description = $"{BlueText(this.Value.ToString())}{SpriteEffectMapping.Get(Effect.Damage, Modifier.Plus)}";
            if (this.Duration != null) this.Description += TurnsString(this.Duration.Value);
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> _1, Character from, Character to) {
            to.AddCallback(new DamageUpCallback(this.Value), this.Duration);
            return new List<CardEffectValues>();
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            to.AddCallback(new DamageUpCallback(this.Value), this.Duration);
        }

        private class DamageUpCallback : OnCompute {
            private readonly int Value;

            public DamageUpCallback(int value) : base(PRIORITY, CallbackType.Damage) => this.Value = value;

            public override int Run(List<CardEffectValues> _, Character from, Character to, int value) {
                return value + this.Value;
            }

            public override int Run(SimulationCharacter from, SimulationCharacter to, int value) {
                return value + this.Value;
            }
        }
    }
}
