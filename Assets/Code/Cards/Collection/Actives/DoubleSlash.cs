using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;

namespace Code.Cards.Collection.Actives {
    public class DoubleSlash : Card {
        public override void Initialize() {
            this.Name = "Double Slash";
            this.CardEffects = new List<CardEffect> {
                new Damage(1),
                new Damage(1)
            };
        }
    }
}
