using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;

namespace Code.Cards.Collection.Actives {
    public class VampiricSlice : Card {
        public override void Initialize() {
            this.Name = "Vampiric Slice";
            this.CardEffects = new List<CardEffect> { new DamageStealingLife(4, 1) };
        }
    }
}
