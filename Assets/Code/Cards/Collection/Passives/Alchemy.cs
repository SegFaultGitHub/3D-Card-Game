using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Passive;

namespace Code.Cards.Collection.Passives {
    public class Alchemy : Card {
        public override void Initialize() {
            this.Name = "Alchemy";
            this.CardEffects = new List<CardEffect> { new PoisonUp(1) };
        }
    }
}
