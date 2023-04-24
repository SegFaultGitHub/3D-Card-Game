using System.Collections.Generic;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Cards.Effects.Active {
    public class IncreasePoison : CardEffect {
        private readonly float Ratio;

        public IncreasePoison(float ratio) => this.Ratio = ratio;

        public override void UpdateDescription(Player player = null) {
            this.Description = new List<string> {
                $"Increase {SpriteEffectMapping.Get(Effect.Poison)} by {(int)(this.Ratio * 100)}{{%}}"
            };
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> sideEffects, Character from, Character to) {
            int poison = (int)(to.Stats.Poison * this.Ratio);
            return new List<CardEffectValues> { RunEffect(sideEffects, CallbackType.Poison, from, to, poison, 1) };
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            int poison = (int)(to.Stats.Poison * this.Ratio);
            RunEffect(CallbackType.Poison, from, to, poison, 1);
        }
    }
}
