using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterScreen.Model;
using DungeonMasterScreen.Comparers;
using DungeonMasterScreen.Events;

namespace DungeonMasterScreen.Controller
{

    public delegate void MonsterAddedHandler(object sender, MonsterCountChangedEventArgs e);

    /// <summary>
    /// This class is core for all operations with monsters.
    /// It emulates database and contains three lists of monsters.
    /// First list is active monsters. These monsters are in actual combat.
    /// Second list is reserve monsters. These monsters are in reserve and wait for their activation.
    /// Third list is dead monsters. These monsters are dead and they are waiting to be deleted or resurect again.
    /// </summary>
    public class MonsterCave
    {
        /// <summary>
        /// This list contains all monsters in actual combat. Dead monster are moved into deadMonsters list. They are sorted by <see cref="Monster.initiative"/>
        /// </summary>
        private List<Monster> activeMonsters;

        public event MonsterAddedHandler ActiveMonstersChanged;

        /// <summary>
        /// Dead monsters are monsters that died in combat. They can be reused.
        /// But these monsters are not stored after program exit.
        /// </summary>
        private List<Monster> deadMonsters = new List<Monster>();

        /// <summary>
        /// Reserve monsters are monsters, that waits for activating. Simply its monster manual of all monsters that has been soterd in central database
        /// </summary>
        private List<Monster> reserveMonsters = new List<Monster>();

        private MonsterInitiativeComparator initiativeComparator = new MonsterInitiativeComparator();

        private static MonsterCave instance;

        /// <summary>
        /// Returns actual count of active monsters if <see cref="activeMonsters"/> are initialized, 0 otherwise.
        /// </summary>
        public int ActiveMonstersCount
        {
            get
            {
                if (activeMonsters != null)
                {
                    return activeMonsters.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        private MonsterCave(List<Monster> reserveMonsters)
        {
            this.activeMonsters = new List<Monster>();
            fireActiveMonsterCountChangedEvent(0, 0);
            initializeReserveMonsters(reserveMonsters);
            this.deadMonsters = new List<Monster>();
        }

        private void initializeReserveMonsters(List<Monster> reserveMonsters)
        {
            if (reserveMonsters != null || reserveMonsters.Count > 1)
            {
                this.reserveMonsters = reserveMonsters;
            }
            else
            {
                this.reserveMonsters = new List<Monster>();
            }
        }

        public static MonsterCave GetInstance(List<Monster> reserveMonsters)
        {
            if (instance == null)
            {
                instance = new MonsterCave(reserveMonsters);
            }
            return instance;
        }

        public void AddMonster(Monster monster)
        {
            int oldCount = activeMonsters.Count;
            activeMonsters.Add(monster);
            activeMonsters.Sort(initiativeComparator);
            fireActiveMonsterCountChangedEvent(oldCount, ActiveMonstersCount);

        }

        public Monster GetMonster(int index)
        {
            return activeMonsters[index];
        }

        public List<Monster> GetAllActiveMonsters()
        {
            return activeMonsters;
        }

        public Monster FindActiveMonsterById(int id)
        {
            return activeMonsters.Find(monster => monster.Id == id);
        }

        public void KillMonster(int id)
        {
            Monster killed = FindActiveMonsterById(id);
            int oldCount = ActiveMonstersCount;
            activeMonsters.Remove(killed);
            deadMonsters.Add(killed);
            activeMonsters.Sort(initiativeComparator);
            fireActiveMonsterCountChangedEvent(oldCount, ActiveMonstersCount);
        }

        public void RemoveMonster(int id)
        {
            Monster monster = reserveMonsters.Find(find => find.Id == id);
            if (monster != null)
            {
                reserveMonsters.Remove(monster);
            }
        }

        public void SortActiveMonsters()
        {
            activeMonsters.Sort(initiativeComparator);
        }

        public void AddReserveMonster(Monster monster)
        {
            reserveMonsters.Add(monster);
        }

        public Monster FinReserveMonsterById(int id)
        {
            return reserveMonsters.Find(monster => monster.Id == id);
        }

        public List<Monster> GetAllReserveMonsters()
        {
            return reserveMonsters;
        }

        public List<Monster> GetAllKilledMonsters()
        {
            return deadMonsters;
        }

        public Monster FindMonsterById(int id)
        {
            Monster monster = activeMonsters.Find(find => find.Id == id);
            if (monster == null)
            {
                monster = reserveMonsters.Find(find => find.Id == id);
            }
            if (monster == null)
            {
                monster = deadMonsters.Find(find => find.Id == id);
            }
            return monster;
        }

        public void RemoveDeadMonster(int id)
        {
            Monster monster = deadMonsters.Find(find => find.Id == id);
            if (monster != null)
            {
                deadMonsters.Remove(monster);
            }
        }

        public void AddMonstersToReserve(List<Monster> monsters)
        {
            reserveMonsters.AddRange(monsters);
        }

        private void fireActiveMonsterCountChangedEvent(int oldCount, int newCount)
        {
            MonsterCountChangedEventArgs args = new MonsterCountChangedEventArgs(oldCount, newCount);
            OnActiveMonstersChange(args);
        }

        protected virtual void OnActiveMonstersChange(MonsterCountChangedEventArgs e)
        {
            if (this.ActiveMonstersChanged != null)
            {
                this.ActiveMonstersChanged(this, e);
            }
        }

    }
}
