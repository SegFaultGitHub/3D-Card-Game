using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;

namespace Code.Cards.Collection.Actives {
    public class Protection : Card {
        public override void Initialize() {
            this.Name = "Protection";
            this.CardEffects = new List<CardEffect> { new Shield(3) };
        }
    }
}
