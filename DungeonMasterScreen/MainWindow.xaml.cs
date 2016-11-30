using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;
using DungeonMasterScreen.Controller;
using DungeonMasterScreen.Model;
using DungeonMasterScreen.Exceptions;
using System.Collections.ObjectModel;
using DungeonMasterScreen.Files;
using DungeonMasterScreen.Core;
using System.IO;
using DungeonMasterScreen.Properties;

namespace DungeonMasterScreen
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private CombatController combatController;

        private ManualController manualController;

        private ObservableCollection<MonsterView> fightList = new ObservableCollection<MonsterView>();

        private ObservableCollection<ManualView> reserveList = new ObservableCollection<ManualView>();

        private ObservableCollection<ManualView> deadList = new ObservableCollection<ManualView>();

        private MonsterDto actualSelectedMonster = null;
        private int? actualSelectedId = null;

        public MainWindow()
        {
            InitializeComponent();
            //TODO: udělat inicializaci a načtení monster manualu.
            try
            {
                MonsterCaveFactory.InitFactory(new FileController().ReadMonsterManual());
            }
            catch (FileFormatException e)
            {
                MonsterCaveFactory.InitFactory(new List<Monster>());
                System.Windows.MessageBox.Show(e.Message, Properties.Resources.MW_MANUAL_OPEN_ERROR, MessageBoxButton.OK);
            }
            combatController = new CombatController();
            combatController.BattleLogEvent += CombatController_BattleLogEvent;
            fightlistView.ItemsSource = fightList;
            reserveMonstersListView.ItemsSource = reserveList;
            actualCombatListView.ItemsSource = fightList;
            killedMonstersListView.ItemsSource = deadList;
            manualController = new ManualController();
            refreshReserveList();
        }

        private void CombatController_BattleLogEvent(object sender, Events.BattleLogEventArgs e)
        {
            appendBattlelogMessage(e.Message);
            turnTB.Text = combatController.turnCounter.ActualTurn.ToString();
        }

        private void appendBattlelogMessage(string message)
        {
            battleLog.AppendText(message);
            battleLog.AppendText(Environment.NewLine);
            battleLog.ScrollToEnd();
        }

        #region MainMenu

        private void endMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                manualController.SaveMonsterManual();
            }
            catch (MonsterManualOpenException ex)
            {
                showErrorManipulatingWithManual(ex.Message);
            }
            finally
            {
                Close();
            }
        }

        #endregion
        #region CombatModule

        private void clearbutton_Click(object sender, RoutedEventArgs e)
        {
            clearCombatForm();
            actualSelectedMonster = null;
        }

        private void clearCombatForm()
        {
            initiativeTB.Text = String.Empty;
            nameTB.Text = String.Empty;
            healthTB.Text = String.Empty;
            attackTB.Text = String.Empty;
            damageTB.Text = String.Empty;
            defenseTB.Text = String.Empty;
            effectsTB.Text = String.Empty;
        }

        private void fillCombatForm(MonsterDto dto)
        {
            initiativeTB.Text = dto.initiative;
            nameTB.Text = dto.name;
            healthTB.Text = dto.lifes;
            attackTB.Text = dto.attackBonusess;
            damageTB.Text = dto.damage;
            defenseTB.Text = dto.defense;
            effectsTB.Text = dto.effects;
        }

        private void clearManualForm()
        {
            initiativeManualTB.Text = String.Empty;
            nameManualTB.Text = String.Empty;
            healthManualTB.Text = String.Empty;
            attackManualTB.Text = String.Empty;
            damageManualTB.Text = String.Empty;
            defenseManualTB.Text = String.Empty;
            effectsManualTB.Text = String.Empty;
        }

        private void fillManualForm(MonsterDto dto)
        {
            initiativeManualTB.Text = dto.initiative;
            nameManualTB.Text = dto.name;
            healthManualTB.Text = dto.lifes;
            attackManualTB.Text = dto.attackBonusess;
            damageManualTB.Text = dto.damage;
            defenseManualTB.Text = dto.defense;
            effectsManualTB.Text = dto.effects;
        }

        private void refreshCombatList()
        {
            fightList.Clear();
            foreach (MonsterDto dto in combatController.GetAllMonsters())
            {
                MonsterView view = new MonsterView(dto.id, dto.initiative, dto.name, dto.lifes);
                fightList.Add(view);
            }
        }

        private void refreshDeadList()
        {
            deadList.Clear();
            foreach (MonsterDto dto in manualController.GetAllKilledMonster())
            {
                ManualView view = new ManualView(dto.id, dto.name, dto.lifes, dto.attackBonusess, dto.defense);
                deadList.Add(view);
            }
        }

        private void refreshReserveList()
        {
            reserveList.Clear();
            foreach (MonsterDto dto in manualController.GetAllReserveMonsters())
            {
                ManualView view = new ManualView(dto.id, dto.name, dto.lifes, dto.attackBonusess, dto.defense);
                reserveList.Add(view);
            }
        }

        private void refreshActualCombatList()
        {
            fightList.Clear();
            foreach (MonsterDto dto in manualController.GetAllActiveMonsters())
            {
                MonsterView view = new MonsterView(dto.id, dto.initiative, dto.name, dto.lifes);
                fightList.Add(view);
            }
        }

        private void actualizeCombatForm()
        {
            if (actualSelectedMonster != null)
            {
                MonsterDto monster = manualController.FindMonsterById(actualSelectedMonster.id);
                fillCombatForm(monster);
            }
        }

        private void refreshView()
        {
            actualizeCombatForm();
            refreshCombatList();
        }

        private void addbutton_Click(object sender, RoutedEventArgs e)
        {
            MonsterDto dto = fillMonsterDto();
            addMonsterToCombat(dto);
            refreshCombatList();
            clearCombatForm();
        }

        private MonsterDto fillMonsterDto()
        {
            MonsterDto dto = new MonsterDto();
            dto.initiative = initiativeTB.Text;
            dto.name = nameTB.Text;
            dto.lifes = healthTB.Text;
            dto.attackBonusess = attackTB.Text;
            dto.damage = damageTB.Text;
            dto.defense = defenseTB.Text;
            dto.effects = effectsTB.Text;
            return dto;
        }

        private MonsterDto fillManualMonsterDto()
        {
            MonsterDto dto = new MonsterDto();
            dto.initiative = initiativeManualTB.Text;
            dto.name = nameManualTB.Text;
            dto.lifes = healthManualTB.Text;
            dto.attackBonusess = attackManualTB.Text;
            dto.damage = damageManualTB.Text;
            dto.defense = defenseManualTB.Text;
            dto.effects = effectsManualTB.Text;
            return dto;
        }


        private void addMonsterToCombat(MonsterDto dto)
        {
            try
            {
                combatController.AddMonsterIntoCombat(dto);
            }
            catch (ValidationException exception)
            {
                System.Windows.MessageBox.Show(exception.Message, Properties.Resources.MW_VALIDATION_ERROR_TITLE, MessageBoxButton.OK);
            }
        }

        private void displaySelectedMonster()
        {
            if (fightlistView.SelectedItem != null)
            {
                int id = (fightlistView.SelectedItem as MonsterView).Id;
                actualSelectedMonster = manualController.FindMonsterById(id);
                fillCombatForm(actualSelectedMonster);
            }
        }

        private void displaySelectedReserveMonster(int id)
        {
            MonsterDto dto = manualController.FindMonsterById(id);
            fillManualForm(dto);
            actualSelectedId = id;
        }

        private void fightlistView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            displaySelectedMonster();
        }

        #endregion

        private void modifybutton_Click(object sender, RoutedEventArgs e)
        {
            if (actualSelectedMonster != null)
            {
                combatController.UpdateMonster(actualSelectedMonster.id, fillMonsterDto());
                refreshView();
            }

        }

        private void discardButton_Click(object sender, RoutedEventArgs e)
        {
            if (actualSelectedMonster != null)
            {
                combatController.RemoveMonster(actualSelectedMonster);
                actualSelectedMonster = null;
                refreshCombatList();
                refreshDeadList();
                clearCombatForm();
            }
        }

        private void harmButton_Click(object sender, RoutedEventArgs e)
        {
            if (actualSelectedMonster != null && !String.IsNullOrEmpty(modTextBox.Text))
            {
                updateMonstersHealth(actualSelectedMonster.id, false);
                afterMonsterUpdateRefresh();
            }
        }

        private void healButton_Click(object sender, RoutedEventArgs e)
        {
            if (actualSelectedMonster != null && !String.IsNullOrEmpty(modTextBox.Text))
            {
                updateMonstersHealth(actualSelectedMonster.id, true);
                afterMonsterUpdateRefresh();
            }
        }

        private void updateMonstersHealth(int id, bool isHealing)
        {
            int healthMod = 0;
            if (int.TryParse(modTextBox.Text, out healthMod))
            {
                healthMod = isHealing ? healthMod : -healthMod;
                combatController.UpdateMonstersHealth(id, healthMod);
            }
            else
            {
                System.Windows.MessageBox.Show(Properties.Resources.MW_NUMBER_VALIDATION_ERROR, Properties.Resources.MW_ERROR_TITLE, MessageBoxButton.OK);
            }
        }

        private void afterMonsterUpdateRefresh()
        {
            refreshView();
            modTextBox.Text = String.Empty;
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            int actualCombatant = combatController.NextTurn();
            fightlistView.SelectedIndex = actualCombatant;
            displaySelectedMonster();
        }

        private void clearbutton_Copy_Click(object sender, RoutedEventArgs e)
        {
            clearManualForm();
        }

        private void addbutton_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                manualController.AddMonsterToReserve(fillManualMonsterDto());
            }
            catch (ValidationException exception)
            {
                System.Windows.MessageBox.Show(exception.Message, Properties.Resources.MW_VALIDATION_ERROR_TITLE, MessageBoxButton.OK);
            }
            refreshReserveList();
            clearManualForm();
        }

        private void modifybutton_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (actualSelectedId != null)
            {
                try
                {
                    manualController.UpdateMonster((int)actualSelectedId, fillManualMonsterDto());
                }
                catch (ValidationException ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, Properties.Resources.MW_VALIDATION_ERROR_TITLE, MessageBoxButton.OK);
                }
            }
            refreshAll();
        }

        private void refreshAll()
        {
            refreshReserveList();
            refreshActualCombatList();
            refreshDeadList();
        }

        private void reserveMonstersListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (reserveMonstersListView.SelectedItem != null)
            {
                displaySelectedReserveMonster((reserveMonstersListView.SelectedItem as ManualView).Id);
            }
        }

        private void actualCombatListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (actualCombatListView.SelectedItem != null)
            {
                displaySelectedReserveMonster((actualCombatListView.SelectedItem as MonsterView).Id);
            }
        }

        private void killedMonstersListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (killedMonstersListView.SelectedItem != null)
            {
                displaySelectedReserveMonster((killedMonstersListView.SelectedItem as ManualView).Id);
            }
        }

        private void addCombatButton_Click(object sender, RoutedEventArgs e)
        {
            if (reserveMonstersListView.SelectedItem != null)
            {
                MonsterDto dto = manualController.FindMonsterById((reserveMonstersListView.SelectedItem as ManualView).Id);
                combatController.AddMonsterIntoCombat(dto);
                refreshAll();
            }
        }

        private void discardMonsterButton_Click(object sender, RoutedEventArgs e)
        {
            if (actualCombatListView.SelectedItem != null)
            {
                MonsterDto dto = manualController.FindMonsterById((actualCombatListView.SelectedItem as MonsterView).Id);
                combatController.RemoveMonster(dto);
                refreshAll();
            }
        }

        private void resurrectButton_Click(object sender, RoutedEventArgs e)
        {
            if (killedMonstersListView.SelectedItem != null)
            {
                MonsterDto dto = manualController.FindMonsterById((killedMonstersListView.SelectedItem as ManualView).Id);
                int health = 0;
                if (int.TryParse(dto.lifes, out health))
                {
                    if (health < 0)
                    {
                        System.Windows.MessageBox.Show(Properties.Resources.MW_LIFE_ERROR_POPUP, Properties.Resources.MW_RESURRECT_ERROR_TITLE, MessageBoxButton.OK);
                    }
                    else
                    {
                        combatController.AddMonsterIntoCombat(dto);
                        manualController.RemoveDeadMonster(dto);
                        refreshAll();
                    }
                }
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (reserveMonstersListView.SelectedItem != null)
            {
                MonsterDto dto = manualController.FindReserveMonsterById((reserveMonstersListView.SelectedItem as ManualView).Id);
                MessageBoxResult result = System.Windows.MessageBox.Show(Properties.Resources.MW_REMOVE_MONSTER_TEXT, Properties.Resources.MW_REMOVE_MONSTER_TITLE, MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    manualController.RemoveMonster(dto);
                    clearManualForm();
                    refreshReserveList();
                }
            }
        }

        private void showErrorManipulatingWithManual(string message)
        {
            System.Windows.MessageBox.Show(message, Properties.Resources.MW_MANUAL_ERROR, MessageBoxButton.OK);
        }

        private void loadMonster_Click(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = Constants.FILE_FILTER;
                dialog.Multiselect = false;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        manualController.ImportMonster(dialog.FileName);
                    }
                    catch (MonsterManualOpenException ex)
                    {

                        System.Windows.MessageBox.Show(ex.Message, Properties.Resources.MW_IMPORT_ERROR_TITLE, MessageBoxButton.OK);
                    }
                    refreshReserveList();
                }
            }

        }

        private void exportMonster_Click(object sender, RoutedEventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = Constants.FILE_FILTER;
                if (reserveMonstersListView.SelectedItem != null && dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        manualController.ExportMonster(dialog.FileName, (reserveMonstersListView.SelectedItem as ManualView).Id);
                    }
                    catch (Exception)
                    {

                        System.Windows.MessageBox.Show(Properties.Resources.MW_MONSTER_EXPORT_ERROR);
                    }
                }
            }
        }

        private void newCombat_Click(object sender, RoutedEventArgs e)
        {
            prepareFormsForNewEncounter(new EncounterCarrier());
        }

        private void loadCombat_Click(object sender, RoutedEventArgs e)
        {
            EncounterCarrier encounter = new EncounterCarrier();
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = Constants.FILE_FILTER;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        encounter = manualController.ImportEncounter(dialog.FileName);

                    }
                    catch (EncounterImportFailedException)
                    {
                        System.Windows.MessageBox.Show(Properties.Resources.MW_NEW_COMBAT_ERROR);
                    }

                }
            }
            prepareFormsForNewEncounter(encounter);
        }

        private void prepareFormsForNewEncounter(EncounterCarrier encounter)
        {
            clearBattlelog();
            combatController.NewCombat(encounter);
            actualizeTurnOrder();
            clearCombatForm();
            refreshAll();
        }

        private void actualizeTurnOrder()
        {
            turnTB.Text = combatController.turnCounter.ActualTurn.ToString();
            fightlistView.SelectedIndex = combatController.turnCounter.ActualCombatant;
        }

        private void clearBattlelog()
        {
            battleLog.Text = string.Empty;
            battleLog.ScrollToEnd();
        }

        private void saveCombat_Click(object sender, RoutedEventArgs e)
        {
            /*
            TODO: Odsud vlastně jen půjde battlelog.text a zavolá se combatController, který si sestaví encounterCarrier
            Ještě odsud musím předat filepath k souboru do kterého se bude ukládat, takže vlastně otevření saveDialogu.
            */
        }
    }
}
