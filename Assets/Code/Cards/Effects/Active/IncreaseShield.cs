using System.Collections.Generic;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Cards.Effects.Active {
    public class IncreaseShield : CardEffect {
        private readonly float Ratio;

        public IncreaseShield(float ratio) => this.Ratio = ratio;

        public override void UpdateDescription(Player player = null) {
            this.Description = new[] {
                $"Increase {SpriteEffectMapping.Get(Effect.Shield)} by {(int)(this.Ratio * 100)}{{%}}"
            };
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> sideEffects, Character from, Character to) {
            int shield = (int)(to.Stats.Shield * this.Ratio);
            return new List<CardEffectValues> { RunEffect(sideEffects, CallbackType.Shield, from, to, shield, 1) };
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            int shield = (int)(to.Stats.Poison * this.Ratio);
            RunEffect(CallbackType.Shield, from, to, shield, 1);
        }
    }
}
