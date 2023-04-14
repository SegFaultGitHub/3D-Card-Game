using System.Collections.Generic;
using Code.Cards.Collection;
using Code.Characters;
using Code.Fight;
using Code.Map;
using Code.Map.Misc;
using Code.Utils;
using UnityEngine;

public class DebugButton : MonoBehaviour {
    public List<Enemy> Enemies;
    public Chest Chest;
    public Room Room;

    public void StartFight(FightManager fightManager) {
        fightManager.StartFight(this.Enemies);
    }
}
