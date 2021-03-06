﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterScreen.Events;

namespace DungeonMasterScreen.Controller
{

    public delegate void NewTurnEventHandler(object sender, NewTurnEventArgs e);

    public class TurnCounter
    {
        public int ActualTurn { get; private set; }
        public int CountOfMonsters { get; private set; }
        public int ActualCombatant { get; private set; }
        public event NewTurnEventHandler NewTurn;

        public TurnCounter(int countOfMonsters)
        {
            CountOfMonsters = countOfMonsters;
            ActualTurn = 1;
            ActualCombatant = 0;
        }

        public void SetMonsterListener()
        {
            MonsterCaveFactory.getMonsterCaveInstance().ActiveMonstersChanged += TurnCounter_ActiveMonstersChanged;
        }

        public void SetNewEncounter(int actualTurn, int actualCombatant, int countOfMonsters)
        {
            ActualTurn = actualTurn;
            ActualCombatant = actualCombatant;
            CountOfMonsters = countOfMonsters;
        }

        private void TurnCounter_ActiveMonstersChanged(object sender, MonsterCountChangedEventArgs e)
        {
            int difference = e.NewCount - e.OldCount;
            if (difference > 0)
            {
                increaseNumberOfCombatants();
            }
            else
            {
                decreaseNumberOfCombatants();
            }
        }

        public void NextInOrder()
        {
            if (CountOfMonsters > 0)
            {
                handleNextInOrder();
            }
        }

        private void handleNextInOrder()
        {
            ActualCombatant++;
            if (ActualCombatant >= CountOfMonsters)
            {
                updateTurnOrder();
            }
        }

        private void updateTurnOrder()
        {
            ActualCombatant = 0;
            ActualTurn++;
            fireChangeEvent();
        }

        private void fireChangeEvent()
        {
            NewTurnEventArgs e = new NewTurnEventArgs(ActualTurn);
            OnChange(e);
        }

        private void increaseNumberOfCombatants()
        {
            CountOfMonsters++;
        }

        public void decreaseNumberOfCombatants()
        {
            if (CountOfMonsters > 0)
            {
                CountOfMonsters--;
            }
            else
            {
                CountOfMonsters = 0;
            }
        }

        protected virtual void OnChange(NewTurnEventArgs e)
        {
            if (NewTurn != null)
            {
                NewTurn(this, e);
            }
        }

    }
}
