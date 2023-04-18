using Code.Artifacts.UI;
using Code.Cards.UI;
using Code.Characters;
using Code.Map.Misc;
using UnityEngine;

namespace Code.UI {
    public class LootSelection : WithRaycast {
        public Player Player { private get; set; }
        [field: SerializeField] private LayerMask UILayer;
        private SelectableUI SelectableUI;
        public Chest Chest { get; set; }

        protected override void Update() {
            base.Update();
            if (this.Chest.Completed) return;

            this.GatherInput();

            Hit<SelectableUI> hit = this.Raycast<SelectableUI>(this.UILayer);
            if (this.Input.Choose && this.SelectableUI != null) {
                switch (this.SelectableUI) {
                    case CardUI cardUI:
                        this.Player.Cards.BaseDeck.Add(cardUI.Card);
                        this.Chest.Completed = true;
                        this.SelectableUI = null;
                        return;
                    case ArtifactUI artifactUI:
                        artifactUI.Artifact.Equip(this.Player);
                        this.Player.Loot.RemoveArtifact(artifactUI.Artifact);
                        this.Chest.Completed = true;
                        this.SelectableUI = null;
                        return;
                }
            }
            if (hit.Obj != null) {
                if (hit.Obj != this.SelectableUI) {
                    if (this.SelectableUI != null) this.SelectableUI.Deselect(false);
                    this.SelectableUI = hit.Obj;
                    this.SelectableUI.Select(false);
                }
            } else if (this.SelectableUI != null) {
                if (this.SelectableUI != null) this.SelectableUI.Deselect(false);
                this.SelectableUI = null;
            }
        }

        #region Input
        private class _Input {
            public bool Choose;
        }
        private _Input Input = new();

        protected override void OnEnable() {
            base.OnEnable();
            this.InputActions.LootSelection.Enable();
        }

        protected override void OnDisable() {
            base.OnDisable();
            this.InputActions.LootSelection.Disable();
        }

        private void GatherInput() {
            this.Input = new _Input {
                Choose = this.InputActions.LootSelection.Choose.WasReleasedThisFrame()
            };
        }
        #endregion
    }
}
