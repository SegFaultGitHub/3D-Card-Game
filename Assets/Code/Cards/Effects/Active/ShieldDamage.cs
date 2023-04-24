using System.Collections.Generic;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Cards.Effects.Active {
    public class ShieldDamage : CardEffect {
        private readonly float Ratio;

        public ShieldDamage(float ratio) => this.Ratio = ratio;

        public override void UpdateDescription(Player _ = null) {
            this.Description = new List<string> {
                $"Deals {(int)(this.Ratio * 100)}{{%}}{SpriteEffectMapping.Get(Effect.Shield)} as {SpriteEffectMapping.Get(Effect.Damage)}"
            };
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> sideEffects, Character from, Character to) {
            int damage = (int)(from.Stats.Shield * this.Ratio);
            return new List<CardEffectValues> { RunEffect(sideEffects, CallbackType.Damage, from, to, damage, 0) };
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            int damage = (int)(from.Stats.Shield * this.Ratio);
            RunEffect(CallbackType.Damage, from, to, damage, short.MaxValue);
        }
    }
}
