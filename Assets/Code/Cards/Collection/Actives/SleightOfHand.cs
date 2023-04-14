using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;

namespace Code.Cards.Collection.Actives {
    public class SleightOfHand : Card {
        public override void Initialize() {
            this.Name = "Sleight of Hand";
            this.CardEffects = new List<CardEffect> { new DrawCards(2) };
        }
    }
}
