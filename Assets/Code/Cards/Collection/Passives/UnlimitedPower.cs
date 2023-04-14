using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Passive;

namespace Code.Cards.Collection.Passives {
    public class UnlimitedPower : Card {
        public override void Initialize() {
            this.Name = "Unlimited Power";
            this.CardEffects = new List<CardEffect> {
                new DamageUp(15, 15),
                new DamageMultiplier(2, 15)
            };
        }
    }
}
