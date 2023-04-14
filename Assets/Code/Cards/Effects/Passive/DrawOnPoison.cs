using System.Collections.Generic;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Cards.Effects.Passive {
    public class DrawOnPoison : CardEffect {
        private const int PRIORITY = 0;
        private readonly int? Duration;
        private readonly int Value;

        public DrawOnPoison(int damage, int? duration = null) {
            this.Value = damage;
            this.Duration = duration;
        }

        public override void UpdateDescription(Player player = null) {
            this.Description = $"{SpriteEffectMapping.Get(Effect.Poison)} "
                               + $"{SpriteEffectMapping.Arrow} "
                               + $"{BlueText(this.Value.ToString())}{SpriteEffectMapping.Get(Effect.Draw, Modifier.Plus)}";

            if (this.Duration != null) this.Description += TurnsString(this.Duration.Value);
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> _1, Character from, Character to) {
            to.AddCallback(new DrawOnPoisonCallback(this.Value), this.Duration);
            return new List<CardEffectValues>();
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            to.AddCallback(new DrawOnPoisonCallback(this.Value), this.Duration);
        }

        private class DrawOnPoisonCallback : OnApply {
            private readonly int Count;

            public DrawOnPoisonCallback(int count) : base(PRIORITY, CallbackType.Poison) => this.Count = count;

            public override int Run(List<CardEffectValues> sideEffects, Character from, Character to, int value) {
                for (int i = 0; i < this.Count; i++)
                    from.DrawCard();
                return value;
            }

            public override int Run(SimulationCharacter from, SimulationCharacter to, int value) {
                for (int i = 0; i < this.Count; i++)
                    from.DrawCard();
                return value;
            }
        }
    }
}
