using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;

namespace Code.Cards.Collection.Actives {
    public class Contamination : Card {
        public override void Initialize() {
            this.Name = "Contamination";
            this.CardEffects = new List<CardEffect> {
                new Poison(2),
                new ActionPoint(-1)
            };
        }
    }
}
