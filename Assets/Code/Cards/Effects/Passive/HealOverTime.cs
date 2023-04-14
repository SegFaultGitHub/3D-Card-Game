using System.Collections.Generic;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Cards.Effects.Passive {
    public class HealOverTime : CardEffect {
        private const int PRIORITY = 1;
        private readonly int? Duration;
        private readonly int Value;

        public HealOverTime(int value, int? duration = null) {
            this.Value = value;
            this.Duration = duration;
        }

        public override void UpdateDescription(Player player = null) {
            int value = player == null ? this.Value : player.Compute(null, CallbackType.Heal, player, null, this.Value, PRIORITY);
            this.Description = $"{BlueText(value)}{SpriteEffectMapping.Get(Effect.Heal)}{SpriteEffectMapping.Turn}";
            if (this.Duration != null) this.Description += TurnsString(this.Duration.Value);
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> _1, Character from, Character to) {
            to.AddCallback(new HealOverTimeCallback(this.Value), this.Duration);
            return new List<CardEffectValues>();
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            to.AddCallback(new HealOverTimeCallback(this.Value), this.Duration);
        }

        private class HealOverTimeCallback : OnTurnStarts {
            private readonly int Value;

            public HealOverTimeCallback(int value) : base(PRIORITY) => this.Value = value;

            public override IEnumerable<CardEffectValues> Run(Character character) {
                return new List<CardEffectValues> { RunEffect(null, CallbackType.Heal, character, character, this.Value, short.MaxValue) };
            }

            public override void Run(SimulationCharacter character) {
                RunEffect(CallbackType.Heal, character, character, this.Value, short.MaxValue);
            }
        }
    }
}
