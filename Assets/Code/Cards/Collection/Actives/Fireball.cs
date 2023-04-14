using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;

namespace Code.Cards.Collection.Actives {
    public class Fireball : Card {
        public override void Initialize() {
            this.Name = "Fireball";
            this.CardEffects = new List<CardEffect> { new Damage(6) };
        }
    }
}
