using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterScreen.Model;
using DungeonMasterScreen.Files;
using DungeonMasterScreen.Exceptions;
using DungeonMasterScreen.Properties;
using System.IO;

namespace DungeonMasterScreen.Controller
{
    /// <summary>
    /// Kontrolér, který je zodpovědný za obsluhu záložky Monster manual
    /// </summary>
    public class ManualController
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
                throw new MonsterManualOpenException(Resources.MC_MANUAL_OPEN_ERROR);
            }
            

        }

        public void SaveMonsterManual()
        {
            if (isManualOpen)
            {
                try
                {
                    fileController.SaveMonsterManual(getMonsterCave().GetAllReserveMonsters());
                }
                catch (IOException e)
                {
                    throw new MonsterManualOpenException(string.Format(Resources.MC_MANUAL_WRITE_ERROR, e.Message));
                }                
                isManualOpen = false;
            }
            else
            {
                throw new MonsterManualOpenException(Resources.MC_MANUAL_INITIALIZATION_ERROR);
            }
        }

        public void ImportMonster(string filePath)
        {
            Monster monster = null;
            try
            {
                monster = fileController.ImportMonster(filePath);
            }
            catch (FileFormatException e)
            {
                throw new MonsterManualOpenException(string.Format(Resources.MC_IMPORT_ERROR,filePath,e.Message));
            }            
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
