using System;
using System.Globalization;
using Code.Callbacks.Enums;
using Code.Cards.Enums;

namespace Code.Cards.UI {
    public static class SpriteEffectMapping {

        public static string Arrow => Format("arrow");
        public static string Clock => Format("clock");
        public static string Heart => Format("heart");
        public static string Turn => Format("refresh");
        public static string Get(Effect effect, Modifier modifier = Modifier.Neutral) {
            string effectString = effect switch {
                Effect.Damage => Format("sword"),
                Effect.Poison => Format("skull"),
                Effect.Heal => Format("heal"),
                Effect.Shield => Format("shield"),
                Effect.ActionPoint => Format("actionPoint"),
                Effect.Draw => Format("book"),
                _ => throw new Exception($"[SpriteEffectMapping:Get] Unexpected effect {effect}")
            };
            string modifierString = modifier switch {
                Modifier.Neutral => "",
                Modifier.Plus => Format("plus", .33f, 0),
                Modifier.Minus => Format("minus", .33f, 0),
                _ => throw new Exception($"[SpriteEffectMapping:Get] Unexpected modifier {modifier}")
            };

            return $"{effectString}{modifierString}";
        }
        public static string Get(CallbackType type) {
            return type switch {
                CallbackType.Damage => Format("sword"),
                CallbackType.Poison => Format("skull"),
                CallbackType.Heal => Format("heal"),
                CallbackType.ActionPoint => Format("actionPoint"),
                CallbackType.Shield => Format("shield"),
                _ => throw new Exception($"[SpriteEffectMapping:Get] Unexpected type {type}")
            };
        }

        private static string Format(string s) {
            return $"{{{s}}}";
        }

        private static string Format(string s, float scale, float pos) {
            return $"{{{s}:scale={scale.ToString(CultureInfo.InvariantCulture)}:pos={pos.ToString(CultureInfo.InvariantCulture)}}}";
        }
    }
}
