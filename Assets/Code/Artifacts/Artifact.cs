using System;
using System.Collections.Generic;
using Code.Characters;
using UnityEngine;

namespace Code.Artifacts {
    public abstract class Artifact : MonoBehaviour {
        public string Name { get; protected set; }
        public List<string> Description { get; protected set; }
        [field: SerializeField] public GameObject Icon { get; private set; }

        public virtual void Equip(Character character) {
            this.transform.SetParent(character.transform.Find("Artifacts"));
        }

        public virtual void Initialize() {
            throw new Exception("[Artifact:Initialize] Must be implemented in the child");
        }
    }
}
