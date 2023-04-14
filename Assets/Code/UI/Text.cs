using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Code.UI {
    public class Text : MonoBehaviour {

        private static readonly Regex SYMBOLS_REGEX = new(@"(arrow|sword|shield|skull|heal|clock|plus|minus|actionPoint|%|book|-|\+|tomb|heart|refresh)");
        private static readonly Regex MODIFIER_REGEX = new(@"(:(scale|pos)=[0-9]+(.[0-9]+)?)*");
        private static readonly Regex REGEX = new($@"^([A-Za-z0-9 ]|\{{{SYMBOLS_REGEX}{MODIFIER_REGEX}\}})+$");
        private static readonly float SPACE_SIZE = 0.2f;

        private static readonly bool INITIALIZED = false;
        private static Character[] NUMBERS_WITH_SIZE;
        private static Character[] LETTERS_WITH_SIZE;
        private static Dictionary<string, Symbol> SYMBOLS_WITH_SIZE;

        [field: SerializeField] private Transform TextParent;

        [field: SerializeField] private Symbol[] Symbols;
        [field: SerializeField] private GameObject[] Numbers;
        [field: SerializeField] private GameObject[] Letters;

        public float Width { get; private set; }
        public float Height { get; private set; }

        protected virtual void Awake() {
            if (INITIALIZED) return;

            NUMBERS_WITH_SIZE = new Character[this.Numbers.Length];
            for (int i = 0; i < this.Numbers.Length; i++) {
                NUMBERS_WITH_SIZE[i] = new Character {
                    Obj = this.Numbers[i],
                    Width = this.Numbers[i].GetComponent<MeshFilter>().sharedMesh.bounds.size.x
                };
            }
            LETTERS_WITH_SIZE = new Character[this.Letters.Length];
            for (int i = 0; i < this.Letters.Length; i++) {
                LETTERS_WITH_SIZE[i] = new Character {
                    Obj = this.Letters[i],
                    Width = this.Letters[i].GetComponent<MeshFilter>().sharedMesh.bounds.size.x
                };
            }
            SYMBOLS_WITH_SIZE = new Dictionary<string, Symbol>();
            for (int i = 0; i < this.Symbols.Length; i++) {
                SYMBOLS_WITH_SIZE[this.Symbols[i].Alias.ToLower()] = new Symbol {
                    Obj = this.Symbols[i].Obj,
                    Width = this.Symbols[i].Obj.GetComponent<MeshFilter>().sharedMesh.bounds.size.x,
                    Height = this.Symbols[i].Obj.GetComponent<MeshFilter>().sharedMesh.bounds.size.y
                };
            }
        }

        public void Initialize(string text) {
            if (!REGEX.IsMatch(text)) throw new Exception($"[Text:Initialize] Text '{text}' is not valid.");
            text = text.ToLower();

            float offset = 0;
            float width = 0;
            float height = 0;
            string symbol = "";
            bool parsingSymbol = false;

            foreach (char c in text) {
                Character character;
                GameObject characterInstance;
                switch (c) {
                    case >= 'a' and <= 'z' when !parsingSymbol:
                        character = LETTERS_WITH_SIZE[c - 'a'];
                        characterInstance = Instantiate(character.Obj, this.TextParent);
                        characterInstance.layer = this.gameObject.layer;
                        characterInstance.transform.localPosition = new Vector3(-offset - character.Width / 2, 0, 0);
                        offset += character.Width + 0.02f;
                        width += character.Width + 0.02f;
                        height = Mathf.Max(height, character.Height);
                        break;
                    case not '}' when parsingSymbol:
                        symbol += c;
                        break;
                    case >= '0' and <= '9':
                        character = NUMBERS_WITH_SIZE[c - '0'];
                        characterInstance = Instantiate(character.Obj, this.TextParent);
                        characterInstance.layer = this.gameObject.layer;
                        characterInstance.transform.localPosition = new Vector3(-offset - character.Width / 2, 0, 0);
                        offset += character.Width + 0.02f;
                        width += character.Width + 0.02f;
                        height = Mathf.Max(height, character.Height);
                        break;
                    case '{':
                        symbol = "";
                        parsingSymbol = true;
                        break;
                    case '}':
                        string[] parts = symbol.Split(":");
                        float scale = 1;
                        for (int i = 1; i < parts.Length; i++) {
                            string[] subParts = parts[i].Split("=");
                            scale = subParts[0] switch {
                                "scale" => float.Parse(subParts[1], CultureInfo.InvariantCulture),
                                _ => scale
                            };
                        }
                        Symbol s = SYMBOLS_WITH_SIZE[parts[0]];
                        GameObject symbolInstance = Instantiate(s.Obj, this.TextParent);
                        symbolInstance.layer = this.gameObject.layer;
                        symbolInstance.transform.localPosition = new Vector3(-offset - scale * s.Width / 2, 0, 0);
                        symbolInstance.transform.localScale = new Vector3(scale, scale, scale);
                        offset += scale * s.Width + 0.02f;
                        width += scale * s.Width + 0.02f;
                        height = Mathf.Max(height, scale * s.Height);
                        parsingSymbol = false;
                        break;
                    default:
                        offset += SPACE_SIZE;
                        width += SPACE_SIZE;
                        break;
                }
            }
            this.Width = width - 0.02f;
            this.Height = height + 0.1f;
            this.TextParent.transform.localPosition = new Vector3(this.Width / 2, 0, 0);
        }
        [Serializable]
        private struct Character {
            public GameObject Obj;
            public float Width;
            public float Height;
        }

        [Serializable]
        private struct Symbol {
            public GameObject Obj;
            public string Alias;
            [HideInInspector] public float Width;
            [HideInInspector] public float Height;
        }
    }
}
