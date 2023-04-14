using System.Collections.Generic;
using Code.Callbacks.Enums;
using Code.Cards.Effects;
using Code.Characters;

namespace Code.Callbacks {
    public abstract class Callback {
        protected Callback(int priority, CallbackType type) {
            this.Priority = priority;
            this.Type = type;
        }

        protected Callback(int priority) => this.Priority = priority;
        // Lower = more priority = executed last
        public int Priority { get; }
        public CallbackType Type { get; }

        public abstract int Run(List<CardEffect.CardEffectValues> list, Character from, Character to, int value);
        public abstract IEnumerable<CardEffect.CardEffectValues> Run(Character character);

        public abstract int Run(SimulationCharacter from, SimulationCharacter to, int value);
        public abstract void Run(SimulationCharacter character);
    }
}
