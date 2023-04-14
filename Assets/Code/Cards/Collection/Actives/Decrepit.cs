using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;

namespace Code.Cards.Collection.Actives {
    public class Decrepit : Card {
        public override void Initialize() {
            this.Name = "Decrepit";
            this.CardEffects = new List<CardEffect> { new ActionPoint(-2) };
        }
    }
}
