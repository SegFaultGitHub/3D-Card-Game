using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;

namespace Code.Cards.Collection.Actives {
    public class Slash : Card {
        public override void Initialize() {
            this.Name = "Slash";
            this.CardEffects = new List<CardEffect> { new Damage(3) };
        }
    }
}
