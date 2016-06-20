using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterScreen.Model;
using DungeonMasterScreen.Files;
using DungeonMasterScreen.Exceptions;

namespace DungeonMasterScreen.Controller
{
    class ManualController
    {
        private FileController fileController = new FileController();

        private static bool isManualOpen;

        public ManualController()
        {
            isManualOpen = true;
        }

        private MonsterCave getMonsterCave() {
            return MonsterCaveFactory.getMonsterCaveInstance();
        }

        public void AddMonsterToReserve(MonsterDto dto)
        {
            MonsterParser.ValidateMonsterDto(dto);
            Monster monster = MonsterParser.createNewInstanceOfMonster(dto);
            getMonsterCave().AddReserveMonster(monster);
        }

        public List<MonsterDto> GetAllReserveMonsters()
        {
            List<MonsterDto> result = new List<MonsterDto>();
            foreach (Monster monster in getMonsterCave().GetAllReserveMonsters())
            {
                MonsterDto dto = MonsterParser.convertMonsterIntoDto(monster);
                result.Add(dto);
            }
            return result;
        }

        public List<MonsterDto> GetAllKilledMonster()
        {
            List<MonsterDto> result = new List<MonsterDto>();
            foreach (Monster monster in getMonsterCave().GetAllKilledMonsters())
            {
                MonsterDto dto = MonsterParser.convertMonsterIntoDto(monster);
                result.Add(dto);
            }
            return result;
        }

        public List<MonsterDto> GetAllActiveMonsters()
        {
            List<MonsterDto> result = new List<MonsterDto>();
            foreach (Monster monster in getMonsterCave().GetAllActiveMonsters())
            {
                MonsterDto dto = MonsterParser.convertMonsterIntoDto(monster);
                result.Add(dto);
            }
            return result;
        }

        public void UpdateMonster(int id, MonsterDto dto)
        {
            MonsterParser.ValidateMonsterDto(dto);
            Monster monster = getMonsterCave().FindMonsterById(id);
            MonsterParser.CopyAttributes(dto, monster);
            getMonsterCave().SortActiveMonsters();
        }

        public MonsterDto FindReserveMonsterById(int id)
        {
            Monster monster = getMonsterCave().FinReserveMonsterById(id);
            return MonsterParser.convertMonsterIntoDto(monster);
        }

        public MonsterDto FindMonsterById(int id)
        {
            Monster monster = getMonsterCave().FindMonsterById(id);
            return MonsterParser.convertMonsterIntoDto(monster);
        }

        public void RemoveDeadMonster(MonsterDto dto)
        {
            getMonsterCave().RemoveDeadMonster(dto.id);
        }

        public void RemoveMonster(MonsterDto dto)
        {
            getMonsterCave().RemoveMonster(dto.id);
        }

        public void OpenMonsterManual()
        {

            if (!isManualOpen)
            {
                List<Monster> monsters = fileController.ReadMonsterManual();
                getMonsterCave().AddMonstersToReserve(monsters);
                isManualOpen = true;
            }
            else
            {
                throw new MonsterManualOpenException("Nelze otevřít bestiář, protože je již otevřen!");
            }
            

        }

        public void SaveMonsterManual()
        {
            if (isManualOpen)
            {
                fileController.SaveMonsterManual(getMonsterCave().GetAllReserveMonsters());
                isManualOpen = false;
            }
            else
            {
                throw new MonsterManualOpenException("Nelze uložit bestiář, protože ještě není otevřen!");
            }
        }

        public void ImportMonster(string filePath)
        {
            Monster monster = fileController.ImportMonster(filePath);
            if (monster!=null)
            {
                getMonsterCave().AddReserveMonster(monster); 
            }
        }

        public void ExportMonster(string filePath, int id) {
            Monster monster = getMonsterCave().FindMonsterById(id);
            fileController.ExportMonster(filePath, monster);
        }
    }
}
