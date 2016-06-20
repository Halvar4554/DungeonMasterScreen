using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterScreen.Core;
using DungeonMasterScreen.Events;

namespace DungeonMasterScreen.Model
{

    public delegate void MonsterChangedEventHandler(object sender, MonsterChangedEventArgs e);

    /// <summary>
    /// Represent one monster. Its base model for storing in "database" <see cref="Controller.MonsterCave"/>
    /// </summary>
    public class Monster
    {
        public int Id { get; }
        public string Name { get; set; }

        private int initiative;

        public int Initiative
        {
            get { return this.initiative; }
            set
            {
                int original = initiative;
                this.initiative = value;
                if (original!=initiative)
                {
                    fireInitiativeChangedEvent(initiative); 
                }
            }
        }

        private int health;

        public int Health
        {
            get { return this.health; }
            set
            {
                int original = this.health;
                this.health = value;
                if (original!=health)
                {
                    fireHealthChangedEvent(original); 
                }
            }
        }

        public string AttackBonusess { get; set; }
        public string Damage { get; set; }
        public int Defense { get; set; }
        public string Effects { get; set; }

        public event MonsterChangedEventHandler MonsterChange;

        public Monster(int id)
        {
            this.Id = id;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Name).Append(Constants.DELIMITER);
            builder.Append(Initiative).Append(Constants.DELIMITER);
            builder.Append(Health).Append(Constants.DELIMITER);
            builder.Append(AttackBonusess).Append(Constants.DELIMITER);
            builder.Append(Damage).Append(Constants.DELIMITER);
            builder.Append(Defense).Append(Constants.DELIMITER);
            builder.Append(Effects);
            return builder.ToString();
        }

        private void fireHealthChangedEvent(int original)
        {
            MonsterChangedEventArgs e = new HealthChangedEventArgs(Name,original, health);
            OnChange(e);
        }

        protected virtual void OnChange(MonsterChangedEventArgs e)
        {
            if (MonsterChange != null)
            {
                this.MonsterChange(this, e);
            }
        }

        private void fireInitiativeChangedEvent(int initiative)
        {
            MonsterChangedEventArgs e = new InitiativeChangedEventArgs(Name, initiative);
            OnChange(e);
        }
           

    }
}
