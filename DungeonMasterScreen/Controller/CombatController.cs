using System;
using System.Collections.Generic;
using System.Text;
using DungeonMasterScreen.Model;
using DungeonMasterScreen.Events;
using DungeonMasterScreen.Properties;

namespace DungeonMasterScreen.Controller
{
    public delegate void BatlleLogEventHandler(object sender, BattleLogEventArgs e);

    /// <summary>
    /// Kontrolér, který je zodpovědný za obsluhu záložky souboje a událostí okolo.
    /// </summary>
    public class CombatController
    {
        #region Attributes
        public TurnCounter turnCounter { get; private set; }

        private int actualIndex = 0;

        public int ActualIndex { get { return actualIndex; } set { actualIndex = value; } }

        public event BatlleLogEventHandler BattleLogEvent;

        #endregion
        #region Public members

        public CombatController()
        {
            turnCounter = new TurnCounter(getMonsterCave().ActiveMonstersCount);
            turnCounter.SetMonsterListener();
            turnCounter.NewTurn += TurnCounter_NewTurn;
        }

        private MonsterCave getMonsterCave() {
            return MonsterCaveFactory.getMonsterCaveInstance();
        }

       
        public void AddMonsterIntoCombat(MonsterDto dto)
        {
            MonsterParser.ValidateMonsterDto(dto);
            Monster monster = MonsterParser.createNewInstanceOfMonster(dto);
            monster.MonsterChange += Monster_MonsterChange;
            getMonsterCave().AddMonster(monster);
            fireMonsterAddedEvent(monster.Name, monster.Initiative);
        }

        public List<MonsterDto> GetAllMonsters()
        {
            List<MonsterDto> monsters = new List<MonsterDto>();
            foreach (Monster actualMonster in getMonsterCave().GetAllActiveMonsters())
            {
                MonsterDto monsterDto = MonsterParser.convertMonsterIntoDto(actualMonster);
                monsters.Add(monsterDto);
            }
            return monsters;
        }

        public void UpdateMonster(int id, MonsterDto dto)
        {
            MonsterParser.ValidateMonsterDto(dto);
            Monster monster = getMonsterCave().FindActiveMonsterById(id);
            MonsterParser.CopyAttributes(dto, monster);
            getMonsterCave().SortActiveMonsters();
        }

        public void RemoveMonster(MonsterDto dto)
        {
            Monster monster = getMonsterCave().FindActiveMonsterById(dto.id);
            monster.MonsterChange -= Monster_MonsterChange;
            getMonsterCave().KillMonster(monster.Id);
            fireMonsterRemovedEvent(monster.Name);
        }

        public void UpdateMonstersHealth(int id, int health)
        {
            Monster monster = getMonsterCave().FindActiveMonsterById(id);
            monster.Health += health;
        }

        public int NextTurn()
        {
            turnCounter.NextInOrder();
            return turnCounter.ActualCombatant;
        }

        #endregion
        #region Private members

        protected virtual void OnChange(BattleLogEventArgs e)
        {
            if (this.BattleLogEvent != null)
            {
                BattleLogEvent(this, e);
            }
        }

        

        private void Monster_MonsterChange(object sender, MonsterChangedEventArgs e)
        {
            string message = buildBattleLogMessage(e);
            fireEvent(message);
        }

        private string buildBattleLogMessage(MonsterChangedEventArgs e)
        {
            if (e is HealthChangedEventArgs)
            {
                HealthChangedEventArgs args = e as HealthChangedEventArgs;
                return buildHealthChangeMessage(args.Name, args.Original, args.Updated);
            }
            else if (e is InitiativeChangedEventArgs)
            {
                InitiativeChangedEventArgs args = e as InitiativeChangedEventArgs;
                return buildInitiativeChangeMessage(args.Name, args.Initiative);
            }
            else
            {
                return String.Format(Resources.CC_UNKNOWN_EVENT, e.Name);
            }
        }

        private string buildHealthChangeMessage(string name, int original, int updated)
        {
            StringBuilder builder = new StringBuilder(name);
            builder.Append(getVerbForMessage(original, updated));
            builder.Append(String.Format(Resources.CC_NEW_LIFE_COUNT, computeDifference(original, updated), updated));
            return builder.ToString();
        }

        private int computeDifference(int original, int updated)
        {
            int different = 0;
            if (original < updated)
            {
                different = updated - original;
            }
            else
            {
                different = original - updated;
            }
            return different;
        }

        private string getVerbForMessage(int original, int updated)
        {
            if (original < updated)
            {
                return Resources.CC_HEALING_LOG;
            }
            else
            {
                return Resources.CC_DAMAGE_LOG;
            }
        }

        private string buildInitiativeChangeMessage(string name, int initiative)
        {
            StringBuilder builder = new StringBuilder(name);
            builder.Append(String.Format(Resources.CC_INITIATIVE_CHANGE, initiative));
            return builder.ToString();
        }

        private void fireMonsterAddedEvent(string name,int initiative) {
            string message = String.Format(Resources.CC_COMBAT_ENTER, name, initiative);
            fireEvent(message);
        }

        private void fireMonsterRemovedEvent(string name) {
            string message = String.Format(Resources.CC_REMOVE_FROM_COMBAT, name);
            fireEvent(message);
        }

        private void fireNewTurnEvent(int turn)
        {
            string message = String.Format(Resources.CC_NEXT_TURN, turn);
            fireEvent(message);
        }

        private void fireEvent(string message) {
            BattleLogEventArgs args = new BattleLogEventArgs(message);
            OnChange(args);
        }

        private void TurnCounter_NewTurn(object sender, NewTurnEventArgs e)
        {
            fireNewTurnEvent(e.Turn);
        }

        #endregion

    }
}
