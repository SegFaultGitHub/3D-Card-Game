using System.Collections.Generic;
using System.Globalization;
using Code.Callbacks.Enums;
using Code.Cards.UI;
using Code.Characters;
using JetBrains.Annotations;

namespace Code.Cards.Effects {
    public abstract class CardEffect {
        public string Description { get; protected set; }

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
        //protected static string GreenText(string input) => $"<color=#005500>{input}</color>";
        protected static string GreenText(string input) {
            return input;
        }
        protected static string GreenText(int input) {
            return GreenText(input.ToString());
        }
        protected static string GreenText(float input) {
            return GreenText(input.ToString(CultureInfo.InvariantCulture));
        }

        // protected static string RedText(string input) => $"<color=#550000>{input}</color>";
        protected static string RedText(string input) {
            return input;
        }
        protected static string RedText(int input) {
            return RedText(input.ToString());
        }
        protected static string RedText(float input) {
            return RedText(input.ToString(CultureInfo.InvariantCulture));
        }

        // protected static string BlueText(string input) => $"<color=#000099>{input}</color>";
        protected static string BlueText(string input) {
            return input;
        }
        protected static string BlueText(int input) {
            return BlueText(input.ToString());
        }
        protected static string BlueText(float input) {
            return BlueText(input.ToString(CultureInfo.InvariantCulture));
        }

        protected static string TurnsString(int duration) {
            return $" {BlueText(duration)}{SpriteEffectMapping.Clock}";
        }
        #endregion
    }
}
