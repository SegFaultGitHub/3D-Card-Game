using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Passive;

namespace Code.Cards.Collection.Passives {
    public class Gangrene : Card {
        public override void Initialize() {
            this.Name = "Gangrene";
            this.CardEffects = new List<CardEffect> { new DamageOnPoison(1) };
        }
    }
}
