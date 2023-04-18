using System.Collections.Generic;
using Code.Callbacks.Enums;
using Code.Cards.UI;
using Code.Characters;
using JetBrains.Annotations;

namespace Code.Cards.Effects {
    public abstract class CardEffect {
        public string[] Description { get; protected set; }

        public abstract IEnumerable<CardEffectValues> Run(List<CardEffectValues> sideEffects, Character from, Character to);
        public abstract void Run(SimulationCharacter from, SimulationCharacter to);

        public abstract void UpdateDescription([CanBeNull] Player player = null);

        public static CardEffectValues RunEffect(
            List<CardEffectValues> sideEffects, CallbackType type, Character from, Character to, int value, int priority
        ) {
            CardEffectValues values = new() {
                Input = value,
                From = from,
                Target = to,
                CallbackType = type
            };

            value = from.Compute(sideEffects, type, from, to, value, priority);
            values.Compute = value;
            value = from.Apply(sideEffects, type, from, to, value, priority);
            values.Apply = value;
            value = to.Take(sideEffects, type, from, to, value, priority);
            values.Take = value;
            value = from.Applied(sideEffects, type, from, to, value, priority);
            values.Applied = value;

            return values;
        }

        public static CardEffectValues RunEffect(
            CallbackType type, SimulationCharacter from, SimulationCharacter to, int value, int priority
        ) {
            CardEffectValues values = new() { Input = value };
            value = from.Compute(type, from, to, value, priority);
            values.Compute = value;
            value = from.Apply(type, from, to, value, priority);
            values.Apply = value;
            value = to.Take(type, from, to, value, priority);
            values.Take = value;
            value = from.Applied(type, from, to, value, priority);
            values.Applied = value;

            return values;
        }

        public class CardEffectValues {
            public int Input;
            public int Compute;
            public int Apply;
            public int Take;
            public int Applied;
            public Character From;
            public Character Target;
            public CallbackType CallbackType;
        }

        #region String methods
        protected static string TurnsString(int duration) => $"{duration}{SpriteEffectMapping.Clock}";
        #endregion
    }
}
