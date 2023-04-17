using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Passive;

namespace Code.Cards.Collection.Passives {
    public class Plague : Card {
        public override void Initialize() {
            this.Name = "Plague";
            this.CardEffects = new List<CardEffect> { new PoisonOnDamage(1) };
        }
    }
}
