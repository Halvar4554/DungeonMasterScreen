using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterScreen.Model;
using DungeonMasterScreen.Exceptions;
using DungeonMasterScreen.Events;

namespace DungeonMasterScreen.Controller
{
    delegate void BatlleLogEventHandler(object sender, BattleLogEventArgs e);

    class CombatController
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
            turnCounter.IncreaseNumberOfCombatants();
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

        public MonsterDto FindMonsterById(int id)
        {
            return MonsterParser.convertMonsterIntoDto(getMonsterCave().FindActiveMonsterById(id));
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
            turnCounter.DecreaseNumberOfCombatants();
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
                return String.Format("{0} hodilo neznámou eventu!", e.Name);
            }
        }

        private string buildHealthChangeMessage(string name, int original, int updated)
        {
            StringBuilder builder = new StringBuilder(name);
            builder.Append(getVerbForMessage(original, updated));
            builder.Append(String.Format("{0} a jeho aktuální počet životů je {1}.", computeDifference(original, updated), updated));
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
                return " byl vyléčen za:";
            }
            else
            {
                return " byl zraněn za:";
            }
        }

        private string buildInitiativeChangeMessage(string name, int initiative)
        {
            StringBuilder builder = new StringBuilder(name);
            builder.Append(String.Format(" změnil iniciativu na {0}.", initiative));
            return builder.ToString();
        }

        private void fireMonsterAddedEvent(string name,int initiative) {
            string message = String.Format("{0} vstoupil do souboje s iniciativou: {1}", name, initiative);
            fireEvent(message);
        }

        private void fireMonsterRemovedEvent(string name) {
            string message = String.Format("{0} byl odstraněn ze souboje", name);
            fireEvent(message);
        }

        private void fireNewTurnEvent(int turn)
        {
            string message = String.Format("Další kolo:{0}", turn);
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
