using System;
using System.Collections.Generic;
using System.Linq;
using Code.Cards.Collection;
using Code.Cards.Enums;
using Code.Utils;

namespace Code.Characters {
    #region Simulation classes
    public class CardOption {
        public Card Card;
        public Character Target;
        public float Weight;
    }

    public class Score {

        public float AdditionalScore;
        public Dictionary<Team, int> CallbacksTotalTurns;
        public Dictionary<Team, int> CurrentActionPoints;
        public Dictionary<Team, int> HandSize;
        public Dictionary<Team, int> Health;
        public Dictionary<Team, int> Poison;
        public Dictionary<Team, int> Shield;

        public Score() {
            this.Health = new Dictionary<Team, int> {
                [Team.Allies] = 0,
                [Team.Enemies] = 0
            };
            this.CurrentActionPoints = new Dictionary<Team, int> {
                [Team.Allies] = 0,
                [Team.Enemies] = 0
            };
            this.HandSize = new Dictionary<Team, int> {
                [Team.Allies] = 0,
                [Team.Enemies] = 0
            };
            this.Shield = new Dictionary<Team, int> {
                [Team.Allies] = 0,
                [Team.Enemies] = 0
            };
            this.CallbacksTotalTurns = new Dictionary<Team, int> {
                [Team.Allies] = 0,
                [Team.Enemies] = 0
            };
            this.Poison = new Dictionary<Team, int> {
                [Team.Allies] = 0,
                [Team.Enemies] = 0
            };
        }

        public float Value =>
            (this.Health[Team.Allies] - this.Health[Team.Enemies]) * 1.5f
            + (this.Shield[Team.Allies] - this.Shield[Team.Enemies]) * 1f
            + (this.CallbacksTotalTurns[Team.Allies] - this.CallbacksTotalTurns[Team.Enemies]) * 5f
            - (this.Poison[Team.Allies] - this.Poison[Team.Enemies]) * 2
            + this.AdditionalScore;
    }

    public struct SimulationTeams {
        public List<SimulationCharacter> Allies;
        public List<SimulationCharacter> Enemies;
    }

    public enum Team {
        Allies, Enemies
    }
    #endregion

    public static class Simulation {
        public static bool MakeChoice(Character character) {
            List<WeightDistribution<CardOption>> cardOptions = GetCardOptions(character);
            if (cardOptions.Count == 0)
                return false;
            CardOption option = Utils.Utils.Sample(cardOptions);
            return character.UseCard(option.Card, option.Target);
        }

        private static List<WeightDistribution<CardOption>> GetCardOptions(Character character) {
            List<Character> allies = new(character.Allies);
            allies.Remove(character);

            List<CardOption> options = new();

            foreach (Card card in character.Cards.Hand) {
                void _AddOptions(IEnumerable<Character> characters, bool dead) {
                    List<CardOption> list = (
                        from target in characters
                        where target.Stats.Dead == dead && card.CanUse(character, target)
                        select new CardOption {
                            Card = card,
                            Target = target
                        }).ToList();
                    foreach (CardOption option in list)
                        option.Weight = 1f / list.Count;
                    options.AddRange(list);
                }

                foreach (Target target in card.AllowedTarget) {
                    switch (target) {
                        case Target.AliveAlly:
                            _AddOptions(allies, false);
                            break;
                        case Target.AliveEnemy:
                            _AddOptions(character.Enemies, false);
                            break;
                        case Target.Self:
                            _AddOptions(new List<Character> { character }, false);
                            break;
                        case Target.DeadEnemy:
                            _AddOptions(character.Enemies, true);
                            break;
                        case Target.DeadAlly:
                            _AddOptions(allies, true);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return EvaluateOptions(character, options);
        }

        private static List<WeightDistribution<CardOption>> EvaluateOptions(Character character, IEnumerable<CardOption> options) {
            Score before = new();
            EvaluateTeamScore(before, character.Allies.Select(ally => ally.GenerateSimulationCharacter()).ToList(), Team.Allies);
            EvaluateTeamScore(before, character.Enemies.Select(enemy => enemy.GenerateSimulationCharacter()).ToList(), Team.Enemies);

            List<WeightDistribution<CardOption>> result = new();
            foreach (CardOption option in options) {
                SimulationTeams? after = character.SimulateUseCard(option.Card, option.Target);
                if (after == null) continue;

                Score score = new();
                EvaluateTeamScore(score, after.Value.Allies, Team.Allies);
                EvaluateTeamScore(score, after.Value.Enemies, Team.Enemies);
                float weight = CompareScores(before, score);
                result.Add(
                    new WeightDistribution<CardOption> {
                        Weight = weight * option.Weight,
                        Obj = option
                    }
                );
            }
            return result;
        }

        private static float CompareScores(Score before, Score after) {
            if (before.CurrentActionPoints[Team.Allies] <= after.CurrentActionPoints[Team.Allies])
                after.AdditionalScore += (after.CurrentActionPoints[Team.Allies] - before.CurrentActionPoints[Team.Allies] + 2) * 3;
            if (before.CurrentActionPoints[Team.Enemies] > after.CurrentActionPoints[Team.Enemies])
                after.AdditionalScore += (before.CurrentActionPoints[Team.Enemies] - after.CurrentActionPoints[Team.Enemies]) * 3;

            if (before.HandSize[Team.Allies] < after.HandSize[Team.Allies] + 1)
                after.AdditionalScore += (after.HandSize[Team.Allies] + 1 - before.HandSize[Team.Allies]) * 3;
            if (before.HandSize[Team.Enemies] > after.HandSize[Team.Enemies] + 1)
                after.AdditionalScore += (before.HandSize[Team.Enemies] + 1 - after.HandSize[Team.Enemies]) * 3;

            return after.Value - before.Value;
        }

        private static void EvaluateTeamScore(Score score, List<SimulationCharacter> simulationCharacters, Team team) {
            foreach (SimulationCharacter character in simulationCharacters) {
                score.Health[team] += character.Stats.Health;
                score.CurrentActionPoints[team] += character.Stats.CurrentActionPoints;
                score.HandSize[team] += character.HandSize;
                score.Shield[team] += character.Stats.Shield;
                score.CallbacksTotalTurns[team] += character.CallbacksTotalTurns;
                score.Poison[team] += character.Stats.Poison;
            }
        }
    }
}
