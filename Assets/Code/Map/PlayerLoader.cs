using Cinemachine;
using Code.Cards.UI;
using Code.Characters;
using Code.Fight;
using Code.Singleton;
using Code.UI;
using UnityEngine;

namespace Code.Map {
    public class PlayerLoader : MonoBehaviour {
        [field: SerializeField] private FightManager FightManager;
        [field: SerializeField] private CardSelection CardSelection;
        [field: SerializeField] private LootSelection LootSelection;
        [field: SerializeField] private CinemachineFreeLook Cinemachine;

        // Temp
        [field: SerializeField] private Room Room;

        private void Start() {
            Player player = Instantiate(Static.Player, this.Room.Center.position, Quaternion.identity);
            this.FightManager.Player = player;
            this.CardSelection.Player = player;
            this.LootSelection.Player = player;
            this.Cinemachine.Follow = player.transform;
            this.Cinemachine.LookAt = player.transform;
        }
    }
}
