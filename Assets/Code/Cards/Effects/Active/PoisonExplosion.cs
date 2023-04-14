using System.Collections.Generic;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Cards.Effects.Active {
    public class PoisonExplosion : CardEffect {
        private readonly int Ratio;

        public PoisonExplosion(int ratio) => this.Ratio = ratio;

        public override void UpdateDescription(Player _ = null) {
            this.Description = $"{BlueText(this.Ratio * 100)}{{%}}{SpriteEffectMapping.Get(Effect.Poison)} "
                               + $" {SpriteEffectMapping.Arrow} "
                               + $"{SpriteEffectMapping.Get(Effect.Damage)}";
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> sideEffects, Character from, Character to) {
            int damage = to.Stats.Poison * this.Ratio;
            to.Stats.Poison = 0;
            return new List<CardEffectValues> { RunEffect(sideEffects, CallbackType.Damage, from, to, damage, 0) };
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            int damage = to.Stats.Poison * this.Ratio;
            to.Stats.Poison = 0;
            RunEffect(CallbackType.Damage, from, to, damage, short.MaxValue);
        }
    }
}
