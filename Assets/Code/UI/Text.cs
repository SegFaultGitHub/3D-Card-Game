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
                    Width = this.Numbers[i].GetComponent<MeshFilter>().sharedMesh.bounds.size.x,
                    Height = this.Numbers[i].GetComponent<MeshFilter>().sharedMesh.bounds.size.y
                };
            }
            LETTERS_WITH_SIZE = new Character[this.Letters.Length];
            for (int i = 0; i < this.Letters.Length; i++) {
                LETTERS_WITH_SIZE[i] = new Character {
                    Obj = this.Letters[i],
                    Width = this.Letters[i].GetComponent<MeshFilter>().sharedMesh.bounds.size.x,
                    Height = this.Letters[i].GetComponent<MeshFilter>().sharedMesh.bounds.size.y
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

        public class Word {
            public Transform Parent;
            public float Offset;
            public float Width;
            public float Height;
        }
        public void Initialize(string text, float? maxWidth = null, bool allowMultiLine = true) {
            Word CreateWord(int index) {
                Word result = new() { Parent = new GameObject($"Word {index}").transform };
                result.Parent.SetParent(this.TextParent);
                result.Parent.localPosition *= 0;
                result.Parent.localEulerAngles *= 0;
                result.Parent.localScale = Vector3.one;
                return result;
            }

            if (!REGEX.IsMatch(text)) throw new Exception($"[Text:Initialize] Text '{text}' is not valid.");
            text = text.ToLower();

            string symbol = "";
            bool parsingSymbol = false;
            bool newWord = false;

            int wordIndex = 0;
            Word currentWord = CreateWord(wordIndex);

            List<Word> words = new() { currentWord };

            foreach (char c in text) {
                if (c != ' ' && newWord) {
                    currentWord = CreateWord(++wordIndex);
                    words.Add(currentWord);
                    newWord = false;
                }
                GameObject characterInstance;
                Character character;
                Dimensions dimensions;
                switch (c) {
                    case >= 'a' and <= 'z' when !parsingSymbol:
                        character = LETTERS_WITH_SIZE[c - 'a'];
                        characterInstance = Instantiate(character.Obj, currentWord.Parent);
                        dimensions = characterInstance.AddComponent<Dimensions>();
                        dimensions.Width = character.Width;
                        dimensions.Height = character.Height;
                        characterInstance.layer = this.gameObject.layer;
                        characterInstance.transform.localPosition = new Vector3(-currentWord.Offset - character.Width / 2, -character.Height / 2, 0);
                        currentWord.Offset += character.Width + 0.02f;
                        currentWord.Width += character.Width + 0.02f;
                        currentWord.Height = Mathf.Max(currentWord.Height, character.Height);
                        break;
                    case not '}' when parsingSymbol:
                        symbol += c;
                        break;
                    case >= '0' and <= '9':
                        character = NUMBERS_WITH_SIZE[c - '0'];
                        characterInstance = Instantiate(character.Obj, currentWord.Parent);
                        dimensions = characterInstance.AddComponent<Dimensions>();
                        dimensions.Width = character.Width;
                        dimensions.Height = character.Height;
                        characterInstance.layer = this.gameObject.layer;
                        characterInstance.transform.localPosition = new Vector3(-currentWord.Offset - character.Width / 2, -character.Height / 2, 0);
                        currentWord.Offset += character.Width + 0.02f;
                        currentWord.Width += character.Width + 0.02f;
                        currentWord.Height = Mathf.Max(currentWord.Height, character.Height);
                        break;
                    case '{':
                        symbol = "";
                        parsingSymbol = true;
                        break;
                    case '}':
                        string[] parts = symbol.Split(":");
                        float scale = 1;
                        for (int ii = 1; ii < parts.Length; ii++) {
                            string[] subParts = parts[ii].Split("=");
                            scale = subParts[0] switch {
                                "scale" => float.Parse(subParts[1], CultureInfo.InvariantCulture),
                                _ => scale
                            };
                        }
                        Symbol s = SYMBOLS_WITH_SIZE[parts[0]];
                        GameObject symbolInstance = Instantiate(s.Obj, currentWord.Parent);
                        dimensions = symbolInstance.AddComponent<Dimensions>();
                        dimensions.Width = s.Width;
                        dimensions.Height = s.Height;
                        symbolInstance.layer = this.gameObject.layer;
                        symbolInstance.transform.localPosition = new Vector3(-currentWord.Offset - scale * s.Width / 2, -s.Height / 2, 0);
                        symbolInstance.transform.localScale = new Vector3(scale, scale, scale);
                        currentWord.Offset += scale * s.Width + 0.02f;
                        currentWord.Width += scale * s.Width + 0.02f;
                        currentWord.Height = Mathf.Max(currentWord.Height, scale * s.Height);
                        parsingSymbol = false;
                        break;
                    case ' ':
                        newWord = true;
                        break;
                    default:
                        throw new Exception($"[Text:Initialize] Unexpected character {c}");
                }
            }
            foreach (Word word in words) {
                word.Parent.localPosition = new Vector3(word.Width / 2, 0, 0);
                foreach (Dimensions dimensions in word.Parent.GetComponentsInChildren<Dimensions>()) {
                    dimensions.transform.localPosition -= new Vector3(0, -dimensions.Height / 2 + word.Height / 2, 0);
                }
            }

            if (!allowMultiLine) {
                float offset = 0;
                foreach (Word word in words) {
                    word.Parent.localPosition = new Vector3(-offset, word.Height / 2, 0);
                    offset += word.Width + SPACE_SIZE;
                    this.Height = Mathf.Max(this.Height, word.Height);
                }
                offset -= SPACE_SIZE;
                foreach (Word word in words) {
                    word.Parent.localPosition += new Vector3(offset / 2, 0, 0);
                }
                if (maxWidth < offset) {
                    float scale = maxWidth.Value / offset;
                    this.transform.localScale = new Vector3(scale, scale, scale);
                }
            } else {
                float xOffset = 0;
                float yOffset = 0;
                float height = 0;
                List<Word> currentLine = new();
                foreach (Word word in words) {
                    if (xOffset + word.Width > maxWidth) {
                        xOffset -= SPACE_SIZE;
                        foreach (Word word2 in currentLine) {
                            word2.Parent.localPosition += new Vector3(xOffset / 2, 0, 0);
                        }
                        xOffset = 0;
                        yOffset -= height;
                        height = 0;
                        currentLine = new List<Word>();
                    }
                    currentLine.Add(word);
                    word.Parent.localPosition = new Vector3(-xOffset, yOffset, 0);
                    xOffset += word.Width + SPACE_SIZE;
                    height = Mathf.Max(height, word.Height);
                }
                yOffset -= height;
                xOffset -= SPACE_SIZE;
                foreach (Word word in currentLine) {
                    word.Parent.localPosition += new Vector3(xOffset / 2, 0, 0);
                }
                foreach (Word word in words) {
                    word.Parent.localPosition -= new Vector3(0, yOffset / 2, 0);
                }
                this.Height = -yOffset;
            }
        }

        // public string Initialize(string text, float? maxWidth) {
        //     if (!REGEX.IsMatch(text)) throw new Exception($"[Text:Initialize] Text '{text}' is not valid.");
        //     text = text.ToLower();
        //
        //     float offset = 0;
        //     float width = 0;
        //     float height = 0;
        //     string symbol = "";
        //     bool parsingSymbol = false;
        //     int lastSpaceIndex = -1;
        //
        //     for (int i = 0; i < text.Length; i++) {
        //         char c = text[i];
        //         char? next = i < text.Length - 1 ? text[i + 1] : null;
        //         if (c != ' ' && next == ' ') lastSpaceIndex = i;
        //
        //         Character character;
        //         GameObject characterInstance;
        //         switch (c) {
        //             case >= 'a' and <= 'z' when !parsingSymbol:
        //                 character = LETTERS_WITH_SIZE[c - 'a'];
        //                 characterInstance = Instantiate(character.Obj, this.TextParent);
        //                 characterInstance.layer = this.gameObject.layer;
        //                 characterInstance.transform.localPosition = new Vector3(-offset - character.Width / 2, 0, 0);
        //                 offset += character.Width + 0.02f;
        //                 width += character.Width + 0.02f;
        //                 height = Mathf.Max(height, character.Height);
        //                 break;
        //             case not '}' when parsingSymbol:
        //                 symbol += c;
        //                 break;
        //             case >= '0' and <= '9':
        //                 character = NUMBERS_WITH_SIZE[c - '0'];
        //                 characterInstance = Instantiate(character.Obj, this.TextParent);
        //                 characterInstance.layer = this.gameObject.layer;
        //                 characterInstance.transform.localPosition = new Vector3(-offset - character.Width / 2, 0, 0);
        //                 offset += character.Width + 0.02f;
        //                 width += character.Width + 0.02f;
        //                 height = Mathf.Max(height, character.Height);
        //                 break;
        //             case '{':
        //                 symbol = "";
        //                 parsingSymbol = true;
        //                 break;
        //             case '}':
        //                 string[] parts = symbol.Split(":");
        //                 float scale = 1;
        //                 for (int ii = 1; ii < parts.Length; ii++) {
        //                     string[] subParts = parts[ii].Split("=");
        //                     scale = subParts[0] switch {
        //                         "scale" => float.Parse(subParts[1], CultureInfo.InvariantCulture),
        //                         _ => scale
        //                     };
        //                 }
        //                 Symbol s = SYMBOLS_WITH_SIZE[parts[0]];
        //                 GameObject symbolInstance = Instantiate(s.Obj, this.TextParent);
        //                 symbolInstance.layer = this.gameObject.layer;
        //                 symbolInstance.transform.localPosition = new Vector3(-offset - scale * s.Width / 2, 0, 0);
        //                 symbolInstance.transform.localScale = new Vector3(scale, scale, scale);
        //                 offset += scale * s.Width + 0.02f;
        //                 width += scale * s.Width + 0.02f;
        //                 height = Mathf.Max(height, scale * s.Height);
        //                 parsingSymbol = false;
        //                 break;
        //             case ' ':
        //                 offset += SPACE_SIZE;
        //                 width += SPACE_SIZE;
        //                 break;
        //             default:
        //                 throw new Exception($"[Text:Initialize] Unexpected character {c}");
        //         }
        //
        //         if (width > maxWidth) break;
        //     }
        //     this.Width = width - 0.02f;
        //     this.Height = height + 0.1f;
        //     this.TextParent.transform.localPosition = new Vector3(this.Width / 2, 0, 0);
        //
        //     return "";
        // }

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

        private class Dimensions : MonoBehaviour {
            public float Width;
            public float Height;
        }
    }
}
