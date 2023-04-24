using System.Collections.Generic;
using System.Linq;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;
using Code.Extensions;
using Code.Map;
using Code.Map.Misc;
using Code.UI;
using UnityEngine;

namespace Code.Fight {
    public class FightManager : MonoBehaviour {
        [field: SerializeField] private int TimelineIndex;
        [field: SerializeField] private List<Character> Timeline;
        private Transform Camera;

        [field: SerializeField] public bool InProgress { get; private set; }
        [field: SerializeField] public int Turn { get; private set; }

        [field: SerializeField] public Room Room { get; private set; }
        [field: SerializeField] public List<Enemy> Enemies { get; private set; }
        public Player Player { get; set; }
        [field: SerializeField] private Tier Tier;

        [field: SerializeField] private CardSelection CardSelection;
        [field: SerializeField] private LootSelection LootSelection;
        [field: SerializeField] private FadeScreen FadeScreen;

        [field: SerializeField] private Chest ChestPrefab;


        private void Start() {
            this.Camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }

        private void Update() {
            if (!this.InProgress)
                return;

            this.Camera.transform.position = this.Room.CameraPosition.position;
            this.Camera.LookAt(this.Room.CameraFocus);
        }

        public void StartFight(List<Enemy> enemies) {
            if (this.InProgress)
                return;

            ((PlayerController)this.Player.CharacterController).StartFight();
            this.FadeScreen.Fade(
                1f,
                () => {
                    this.InSeconds(0, () => this.CardSelection.gameObject.SetActive(true));
                    this.Camera.transform.position = this.Room.CameraPosition.position;
                    this.Camera.LookAt(this.Room.CameraFocus);
                    this.Room.PlacePlayer(this.Player);
                    this.Enemies = this.Room.SpawnEnemies(this.Player, enemies);

                    this.InProgress = true;
                    this.Turn = 0;
                    this.TimelineIndex = 0;

                    this.Timeline.Add(this.Player);
                    this.Timeline.AddRange(this.Enemies);
                    this.Player.FightStarts(this);
                    foreach (Enemy enemy in this.Enemies)
                        enemy.FightStarts(this);

                    this.StartTurn();
                }
            );
        }

        private void EndFight() {
            if (!this.InProgress)
                return;

            this.CardSelection.gameObject.SetActive(false);
            this.InProgress = false;
        }

        private void StartTurn() {
            if (this.Timeline[this.TimelineIndex].Stats.Dead)
                this.NextTurn();
            else
                this.Timeline[this.TimelineIndex].TurnStarts();
        }

        private void EndTurn() {
            if (!this.Timeline[this.TimelineIndex].Stats.Dead)
                this.Timeline[this.TimelineIndex].TurnEnds();
            this.TimelineIndex++;
            if (this.TimelineIndex >= this.Timeline.Count) {
                this.TimelineIndex = 0;
                this.Turn++;
            }
        }

        public void NextTurn() {
            if (!this.InProgress)
                return;
            this.While(
                () => !this.Timeline[this.TimelineIndex].Stats.Dead && this.Timeline[this.TimelineIndex].FightLocked,
                () => {
                    this.EndTurn();
                    this.StartTurn();
                }
            );
        }

        public void WinFight() {
            this.EndFight();

            this.While(
                () => this.Timeline[this.TimelineIndex].FightLocked,
                () => {
                    this.Player.UI.HandUI.HideCards();
                    this.InSeconds(
                        .5f,
                        () => {
                            List<Loot> loot = this.Player.Loot.GenerateLoot();
                            Chest chest = Instantiate(this.ChestPrefab);
                            Transform chestTransform = chest.transform;
                            chestTransform.position = this.Room.Center.position;
                            chestTransform.eulerAngles = new Vector3(0, this.Room.Rotation + 180, 0);
                            chest.Open(loot, this.Tier);
                            this.LootSelection.Chest = chest;
                            this.LootSelection.gameObject.SetActive(true);

                            this.While(
                                () => !chest.Completed,
                                () => {
                                    this.FadeScreen.Fade(
                                        1f,
                                        () => {
                                            this.LootSelection.gameObject.SetActive(false);
                                            Destroy(chest.gameObject);
                                            ((PlayerController)this.Player.CharacterController).EndFight();
                                            this.Player.FightEnds();
                                            foreach (Enemy enemy in this.Enemies) {
                                                enemy.FightEnds();
                                                Destroy(enemy.gameObject);
                                            }
                                            this.Enemies.Clear();
                                            this.Timeline.Clear();
                                        }
                                    );
                                }
                            );
                        }
                    );
                }
            );
        }

        private void LoseFight() {
            this.EndFight();
        }

        public void CheckFightEnded() {
            if (!this.InProgress)
                return;
            if (this.Player.Stats.Dead)
                this.LoseFight();
            else if (this.Enemies.All(enemy => enemy.Stats.Dead))
                this.WinFight();
        }
    }
}
