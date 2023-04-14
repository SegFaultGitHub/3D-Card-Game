using System.Collections.Generic;
using System.Linq;
using Code.Cards.Collection;
using Code.Characters;
using UnityEditor;
using UnityEngine;

namespace Code.Cards.UI {
    public class HandUI : MonoBehaviour {

        [field: SerializeField] private Player Player;
        [field: SerializeField] private List<CardUI> Hand;
        [field: SerializeField] private float AngleStep;
        [field: SerializeField] private float MaxAngle;

        [field: SerializeField] private CardUI CardUIPrefab;
        private UnityEngine.Camera Camera;

        private void Awake() {
            this.Player = this.GetComponentInParent<Player>();
            this.Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UnityEngine.Camera>();
        }

        private void FixedUpdate() {
            this.transform.forward = -this.Camera.transform.forward;
        }

        private void OnDrawGizmos() {
            if (this.AngleStep <= 0 || this.Player == null) return;
            Vector3 playerPosition = this.Player.transform.position;

            for (float i = -this.MaxAngle / 2; i <= this.MaxAngle / 2; i += this.AngleStep) {
                Vector3 direction = Quaternion.AngleAxis(i, this.transform.forward) * this.transform.up;
                Handles.DrawLine(playerPosition, playerPosition + direction * this.Player.UI.HandHeight);
            }
        }

        public void AddCard(Card card) {
            card.Initialize();
            CardUI cardUI = Instantiate(this.CardUIPrefab, this.transform);
            cardUI.Card = card;
            cardUI.Initialize(this.Player);
            this.Hand.Add(cardUI);
            this.RearrangeCards();
        }

        public void RemoveCard(CardUI card) {
            this.Hand.Remove(card);
            this.RearrangeCards();
            card.Hide().setDestroyOnComplete(true);
        }

        public void Reset() {
            foreach (CardUI cardUI in this.Hand)
                Destroy(cardUI.gameObject);
            this.Hand.Clear();
        }

        public void UpdateCardDescriptions() {
            foreach (CardUI card in this.Hand) card.UpdateDescription(this.Player);
        }

        public void HideCards(CardUI cardUI = null) {
            foreach (CardUI ui in this.Hand.Where(element => element != cardUI)) {
                ui.Hide();
            }
        }

        public void ShowCards(CardUI cardUI = null) {
            foreach (CardUI ui in this.Hand.Where(element => element != cardUI)) {
                ui.Show();
            }
        }

        private void RearrangeCards() {
            float angleStep = Mathf.Min(this.MaxAngle / (this.Hand.Count - 1f), this.AngleStep);
            float angleOffset = (this.Hand.Count - 1f) * angleStep / 2f;
            foreach (CardUI card in this.Hand) {
                Vector3 direction = Quaternion.AngleAxis(angleOffset, Vector3.forward) * Vector3.up;
                card.Move(direction.normalized * this.Player.UI.HandHeight);
                card.Rotate(new Vector3(0, 0, angleOffset));
                card.InitialPosition = direction.normalized * this.Player.UI.HandHeight;
                card.InitialAngle = new Vector3(0, 0, angleOffset);

                angleOffset -= angleStep;
            }
        }
    }
}
