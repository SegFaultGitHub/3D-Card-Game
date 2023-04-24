using System;
using System.Collections.Generic;
using Code.Artifacts.UI;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Extensions;
using Code.Fight;
using Code.UI;
using UnityEngine;

namespace Code.Map.Misc {
    public class Chest : MonoBehaviour {
        [field: SerializeField] private CardUI CardUIPrefab;
        [field: SerializeField] private ArtifactUI ArtifactUIPrefab;
        [field: SerializeField] private Transform Top;
        [field: SerializeField] private float TransitionDuration;
        [field: SerializeField] private Transform CameraPosition;
        [field: SerializeField] public bool Completed { get; set; }
        [field: SerializeField] private float DeselectSize, SelectSize;

        public void Open(List<Loot> loot, Tier tier) {
            this.SetCameraTarget();

            Vector3 angles = this.Top.eulerAngles;
            LeanTween
                .rotateX(this.Top.gameObject, angles.x - 60, 0.75f)
                .setDelay(this.TransitionDuration)
                .setEaseOutBounce();

            this.InSeconds(
                this.TransitionDuration + 0.75f,
                () => {
                    const float step = 1;
                    float offset = -(loot.Count - 1) / 2f;
                    foreach (Loot item in loot) {
                        SelectableUI selectableUI;
                        switch (item) {
                            case CardLoot cardLoot:
                                selectableUI = Instantiate(this.CardUIPrefab, this.transform);
                                cardLoot.Card.Tier = tier;
                                ((CardUI)selectableUI).Card = cardLoot.Card;
                                ((CardUI)selectableUI).Initialize();
                                break;
                            case ArtifactLoot artifactLoot:
                                selectableUI = Instantiate(this.ArtifactUIPrefab, this.transform);
                                ((ArtifactUI)selectableUI).Artifact = artifactLoot.Artifact;
                                ((ArtifactUI)selectableUI).Initialize();
                                break;
                            default:
                                throw new Exception("[Chest:Open] Unexpected Loot.");
                        }
                        if (selectableUI != null) {
                            selectableUI.transform.localPosition = new Vector3(offset, 3, 1);
                            selectableUI.transform.localEulerAngles = new Vector3(-45, 0, 0);
                            selectableUI.InitialPosition = selectableUI.transform.localPosition;
                            selectableUI.transform.localPosition = Vector3.zero;
                            selectableUI.transform.localScale *= 0;
                            LeanTween.moveLocal(selectableUI.gameObject, new Vector3(offset, 3, 1), 0.3f);
                            selectableUI.DeselectSize = this.DeselectSize;
                            selectableUI.SelectSize = this.SelectSize;
                            LeanTween.scale(selectableUI.gameObject, Vector3.one * selectableUI.DeselectSize, 0.3f);
                        }
                        offset += step;
                    }
                }
            );
        }

        private void SetCameraTarget() {
            GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            LeanTween.rotate(mainCamera.gameObject, this.transform.eulerAngles + new Vector3(45, 180, 0), this.TransitionDuration);
            LeanTween.move(mainCamera.gameObject, this.CameraPosition, this.TransitionDuration);
        }
    }
}
