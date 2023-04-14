using System.Collections.Generic;
using Code.Cards.Effects;

namespace Code.Cards.Collection.Actives {
    public class PoisonExplosion : Card {
        public override void Initialize() {
            this.Name = "Poison Explosion";
            this.CardEffects = new List<CardEffect> {
                new Effects.Active.PoisonExplosion(2)
            };
        }
    }
}
