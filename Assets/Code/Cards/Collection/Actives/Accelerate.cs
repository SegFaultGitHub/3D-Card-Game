using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;

namespace Code.Cards.Collection.Actives {
    public class Accelerate : Card {
        public override void Initialize() {
            this.Name = "Accelerate";
            this.CardEffects = new List<CardEffect> { new ActionPoint(3) };
        }
    }
}
