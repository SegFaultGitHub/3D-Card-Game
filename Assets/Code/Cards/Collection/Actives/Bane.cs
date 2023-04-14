using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;

namespace Code.Cards.Collection.Actives {
    public class Bane : Card {
        public override void Initialize() {
            this.Name = "Bane";
            this.CardEffects = new List<CardEffect> {
                new Damage(2),
                new Poison(1)
            };
        }
    }
}
