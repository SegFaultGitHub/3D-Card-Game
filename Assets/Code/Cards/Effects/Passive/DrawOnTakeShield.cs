using System.Collections.Generic;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;
using Unity.VisualScripting;

namespace Code.Cards.Effects.Passive {
    public class DrawOnTakeShield : CardEffect {
        private const int PRIORITY = -1;
        private readonly int? Duration;
        private readonly int Value;

        public DrawOnTakeShield(int damage, int? duration = null) {
            this.Value = damage;
            this.Duration = duration;
        }

        public override void UpdateDescription(Player player = null) {
            this.Description = new List<string> {
                $"Draws {this.Value}{SpriteEffectMapping.Get(Effect.Draw)} when gaining {SpriteEffectMapping.Get(Effect.Shield)}"
            };
            if (this.Duration != null) this.Description.AddRange(TurnsString(this.Duration.Value));
            // this.Description = new List<string> {
            //     $"{SpriteEffectMapping.Get(Effect.Shield)} "
            //     + $"{SpriteEffectMapping.Arrow} "
            //     + $"{this.Value}{SpriteEffectMapping.Get(Effect.Draw, Modifier.Plus)}"
            // };
            // if (this.Duration != null) this.Description[0] += TurnsString(this.Duration.Value);
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> _, Character from, Character to) {
            to.AddTempCallback(new Callback(this.Value), this.Duration);
            return new List<CardEffectValues>();
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            to.AddTempCallback(new Callback(this.Value), this.Duration);
        }

        private class Callback : OnTake {
            private readonly int Count;

            public Callback(int count) : base(PRIORITY, CallbackType.Shield) => this.Count = count;

            public override int Run(List<CardEffectValues> sideEffects, Character _, Character to, int value) {
                for (int i = 0; i < this.Count; i++)
                    to.DrawCard();
                return value;
            }

            public override int Run(SimulationCharacter _, SimulationCharacter to, int value) {
                for (int i = 0; i < this.Count; i++)
                    to.DrawCard();
                return value;
            }
        }
    }
}
