using System;
using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Characters;

namespace Code.Callbacks {
    public abstract class OnTurnEnds : Callback {
        protected OnTurnEnds(int priority) : base(priority) { }

        public override int Run(List<CardEffect.CardEffectValues> _, Character from, Character to, int value) {
            throw new Exception(
                $"[{this.GetType()}:Run] Run(List<CardEffect.CardEffectValues> _, Character from, Character to, int value) forbidden"
            );
        }
        public override int Run(SimulationCharacter from, SimulationCharacter to, int value) {
            throw new Exception($"[{this.GetType()}:Run] Run(SimulationCharacter from, SimulationCharacter to, int value) forbidden");
        }
    }
}
