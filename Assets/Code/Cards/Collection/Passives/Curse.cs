using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Passive;

namespace Code.Cards.Collection.Passives {
    public class Curse : Card {
        public override void Initialize() {
            this.Name = "Curse";
            this.CardEffects = new List<CardEffect> { new PoisonOnActionPoint(1) };
        }
    }
}
