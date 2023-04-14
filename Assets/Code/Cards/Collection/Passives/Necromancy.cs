using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Passive;

namespace Code.Cards.Collection.Passives {
    public class Necromancy : Card {
        public override void Initialize() {
            this.Name = "Necromancy";
            this.CardEffects = new List<CardEffect> { new HealOnPoison(1) };
        }
    }
}
