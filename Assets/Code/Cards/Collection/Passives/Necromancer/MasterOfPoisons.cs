using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Passive;

namespace Code.Cards.Collection.Passives {
    public class MasterOfPoisons : Card {
        public override void Initialize() {
            this.Name = "Master of Poisons";
            this.CardEffects = new List<CardEffect> { new DrawOnPoison(1) };
        }
    }
}
