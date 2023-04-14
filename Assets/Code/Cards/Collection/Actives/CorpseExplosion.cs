using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;

namespace Code.Cards.Collection.Actives {
    public class CorpseExplosion : Card {
        public override void Initialize() {
            this.Name = "Corpse Explosion";
            this.CardEffects = new List<CardEffect> {
                new Damage(5),
                new Poison(2)
            };
        }
    }
}
