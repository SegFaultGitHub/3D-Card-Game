using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;

namespace Code.Cards.Collection.Actives {
    public class Overwhelm : Card {
        public override void Initialize() {
            this.Name = "Overwhelm";
            this.CardEffects = new List<CardEffect> {
                new Damage(50),
                new Poison(50),
                new Damage(100),
                new Poison(100)
            };
        }
    }
}
