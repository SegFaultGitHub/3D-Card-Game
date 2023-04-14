using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;

namespace Code.Cards.Collection.Actives {
    public class Infection : Card {
        public override void Initialize() {
            this.Name = "Infection";
            this.CardEffects = new List<CardEffect> { new Poison(2) };
        }
    }
}
