using Code.Characters;
using Code.Extensions;
using Code.UI;
using UnityEngine;

namespace Code.Cards.UI {
    public class CardSelection : WithRaycast {
        public Player Player { private get; set; }
        [field: SerializeField] private LayerMask CardUILayer;
        [field: SerializeField] private LayerMask CharacterLayer;

        private float CardDistance;
        private CardUI CardUI;
        private Character Target;

        protected override void Update() {
            base.Update();

            this.GatherInput();

            Hit<CardUI> cardUIHit = this.Raycast<CardUI>(this.CardUILayer);
            if (this.Input.DragCardPerformed && this.CardUI != null) {
                this.Player.UI.HandUI.HideCards(this.CardUI);
                this.CardDistance = cardUIHit.Distance;
                this.CardUI.TargetPositionReached = false;
            } else if (this.Input.DragCardInProgress && this.CardUI != null) {
                Hit<Character> characterHit = this.Raycast<Character>(this.CharacterLayer);
                if (characterHit.Obj != null && !this.CardUI.Card.CanUse(this.Player, characterHit.Obj))
                    characterHit.Obj = null;

                switch (characterHit.Obj) {
                    case null:
                        switch (this.Target) {
                            case null:
                                this.MoveToMouse();
                                break;
                            default:
                                this.Target.DisableHighlight();
                                this.Target = null;
                                this.CardUI.TargetPositionReached = false;
                                this.MoveToMouse();
                                break;
                        }
                        break;
                    case var same when same == this.Target:
                        this.MoveToTarget();
                        break;
                    case var other when other != this.Target:
                        this.Target = other;
                        this.Target.EnableHighlight();
                        this.CardUI.TargetPositionReached = false;
                        this.MoveToTarget();
                        break;
                }
            } else if (this.Input.DragCardEnded && this.CardUI != null) {
                bool played = false;
                if (this.Target != null) {
                    played = this.Player.UseCard(this.CardUI.Card, this.Target);
                    this.Target.DisableHighlight();
                    this.Target = null;
                }
                if (played) {
                    this.Player.UI.HandUI.HideCards();
                    this.Player.UI.HandUI.RemoveCard(this.CardUI);
                    this.While(() => this.Player.FightLocked, () => this.Player.UI.HandUI.ShowCards());
                } else {
                    this.Player.UI.HandUI.ShowCards(this.CardUI);
                    this.CardUI.Deselect();
                }
                this.CardUI = null;
            } else {
                if (cardUIHit.Obj != null) {
                    if (cardUIHit.Obj != this.CardUI) {
                        if (this.CardUI != null) this.CardUI.Deselect();
                        this.CardUI = cardUIHit.Obj;
                        this.CardUI.Select();
                    }
                } else if (this.CardUI != null) {
                    if (this.CardUI != null) this.CardUI.Deselect();
                    this.CardUI = null;
                }
            }
        }

        private void MoveToMouse() {
            Ray ray = this.Camera.ScreenPointToRay(this.MousePosition);
            // Vector3 position = this.CardUI.transform.position;
            Vector3 targetPosition = ray.origin + ray.direction * this.CardDistance;
            // targetPosition.y = Mathf.Max(this.CardUI.HeightAboveGround, targetPosition.y);
            this.CardUI.TargetPosition = targetPosition;
        }

        private void MoveToTarget() {
            Transform cameraTransform = this.Camera.transform;
            this.CardUI.TargetPosition = this.Target.transform.position
                                         + cameraTransform.right * 2
                                         + cameraTransform.up * 2
                                         - cameraTransform.forward * 2;
        }

        #region Input
        private class _Input {
            public bool DragCardEnded;
            public bool DragCardInProgress;
            public bool DragCardPerformed;
        }
        private _Input Input = new();

        protected override void OnEnable() {
            base.OnEnable();
            this.InputActions.CardSelection.Enable();
        }

        protected override void OnDisable() {
            base.OnDisable();
            this.InputActions.CardSelection.Disable();
        }

        private void GatherInput() {
            this.Input = new _Input {
                DragCardPerformed = this.InputActions.CardSelection.DragCard.WasPerformedThisFrame(),
                DragCardInProgress = this.Input.DragCardInProgress,
                DragCardEnded = this.InputActions.CardSelection.DragCard.WasReleasedThisFrame()
            };

            if (this.Input.DragCardPerformed)
                this.Input.DragCardInProgress = true;
            if (this.Input.DragCardEnded)
                this.Input.DragCardInProgress = false;
        }
        #endregion
    }
}
