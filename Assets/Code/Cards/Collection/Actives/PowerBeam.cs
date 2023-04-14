using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;

namespace Code.Cards.Collection.Actives {
    public class PowerBeam : Card {
        public override void Initialize() {
            this.Name = "Power Beam";
            this.CardEffects = new List<CardEffect> { new Damage(12) };
        }
    }
}
