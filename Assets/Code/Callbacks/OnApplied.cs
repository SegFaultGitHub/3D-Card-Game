using System;
using System.Collections.Generic;
using Code.Callbacks.Enums;
using Code.Cards.Effects;
using Code.Characters;

namespace Code.Callbacks {
    public abstract class OnApplied : Callback {
        protected OnApplied(int priority, CallbackType type) : base(priority, type) { }

        public override IEnumerable<CardEffect.CardEffectValues> Run(Character character) {
            throw new Exception($"[{this.GetType()}:Run] Run(Character character) forbidden");
        }
        public override void Run(SimulationCharacter character) {
            throw new Exception($"[{this.GetType()}:Run] Run(SimulationCharacter character) forbidden");
        }
    }
}
