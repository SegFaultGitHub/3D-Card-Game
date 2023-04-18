using System.Collections.Generic;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;
using Unity.VisualScripting;

namespace Code.Cards.Effects.Passive {
    public class ShieldOverTime : CardEffect {
        private const int PRIORITY = 1;
        private readonly int? Duration;
        private readonly int Value;

        public ShieldOverTime(int value, int? duration = null) {
            this.Value = value;
            this.Duration = duration;
        }

        public override void UpdateDescription(Player player = null) {
            int value = player == null ? this.Value : player.Compute(null, CallbackType.Shield, player, null, this.Value, PRIORITY);
            this.Description = new[] {
                $"Gains {value}{SpriteEffectMapping.Get(Effect.Shield)} each turn"
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

        private class Callback : OnTurnEnds {
            private readonly int Value;

            public Callback(int value) : base(PRIORITY) => this.Value = value;

            public override IEnumerable<CardEffectValues> Run(Character character) {
                return new List<CardEffectValues> {
                    RunEffect(null, CallbackType.Shield, character, character, this.Value, short.MaxValue)
                };
            }

            public override void Run(SimulationCharacter character) {
                RunEffect(CallbackType.Shield, character, character, this.Value, short.MaxValue);
            }
        }
    }
}
