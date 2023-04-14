using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;

namespace Code.Cards.Collection.Actives {
    public class Epidemic : Card {
        public override void Initialize() {
            this.Name = "Epidemic";
            this.CardEffects = new List<CardEffect> {
                new Poison(1),
                new Poison(1),
                new Poison(1)
            };
        }
    }
}
