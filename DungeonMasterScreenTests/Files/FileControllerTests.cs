using Microsoft.VisualStudio.TestTools.UnitTesting;
using DungeonMasterScreen.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterScreen.Properties;
using System.IO;

namespace DungeonMasterScreen.Files.Tests
{
    [TestClass()]
    public class FileControllerTests
    {
        [TestMethod()]
        public void TestReadMonsterManualSuccessfull()
        {
            FileController controller = new FileController();
            List<Model.Monster> monsters = controller.ReadMonsterManual();
            Assert.AreEqual(0, monsters.Count);
        }

        [TestMethod()]
        public void SaveMonsterManualTest()
        {
            List<Model.Monster> monsters = new List<Model.Monster>();
            try
            {
                new FileController().SaveMonsterManual(monsters);
            }
            catch (Exceptions.MonsterManualOpenException e)
            {
                StringAssert.Contains(e.Message, Resources.FC_SAVE_FAILURE);
                return;
            }
        }

        [TestMethod()]
        public void ImportMonsterTest()
        {
            FileController cnt = new FileController();
            Model.Monster monster = cnt.ImportMonster("D:\\work\\DungeonMasterScreen\\DungeonMasterScreenTests\\Files\\Droid.txt");
            Assert.IsNotNull(monster);
        }

        [TestMethod]
        public void ImportMonsterWrongFormat() {
            FileController cnt = new FileController();
            try
            {
                Model.Monster monster = cnt.ImportMonster("D:\\work\\DungeonMasterScreen\\DungeonMasterScreenTests\\Files\\WrongDroid.txt");
            }
            catch (FileFormatException e)
            {
                StringAssert.Contains(e.Message, Resources.FC_FORMAT_ERROR);
            }
        }

        [TestMethod()]
        public void ExportMonsterTest()
        {
            FileController cnt = new FileController();
            Model.Monster monster = cnt.ImportMonster("D:\\work\\DungeonMasterScreen\\DungeonMasterScreenTests\\Files\\Droid.txt");
            Assert.IsNotNull(monster);
            cnt.ExportMonster("D:\\work\\DungeonMasterScreen\\DungeonMasterScreenTests\\Files\\ExportedDroid.txt", monster);
            Model.Monster secondMonster = cnt.ImportMonster("D:\\work\\DungeonMasterScreen\\DungeonMasterScreenTests\\Files\\ExportedDroid.txt");
            Assert.IsNotNull(secondMonster);
        }

        [TestMethod()]
        public void ImportEncounterTest()
        {
            FileController cnt = new FileController();
            Model.EncounterCarrier enc = cnt.ImportEncounter("D:\\work\\DungeonMasterScreen\\DungeonMasterScreenTests\\Files\\finale.txt");
            Assert.IsNotNull(enc);
            Assert.IsNotNull(enc.Monsters);
            Assert.IsNotNull(enc.BattleLog);
            Assert.AreEqual(4, enc.Monsters.Count);
            Assert.AreEqual(29, enc.BattleLog.Count);
            Assert.AreEqual(3, enc.ActualCombatant);
            Assert.AreEqual(12, enc.ActualTurn);
        }

        [TestMethod]
        public void ExportEncounterTest() {
            FileController cnt = new FileController();
        }
    }
}