using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DungeonMasterScreen.Model;
using DungeonMasterScreen.Core;
using DungeonMasterScreen.Controller;
using System.Reflection;

namespace DungeonMasterScreen.Files
{
    class FileController
    {
        public List<Monster> ReadMonsterManual()
        {
            List<Monster> monsters = new List<Monster>();
            using (StreamReader reader = new StreamReader(Constants.MONSTER_MANUAL_FILE))
            {
                string fileFormat = reader.ReadLine();
                if (fileFormat.Equals(Constants.MONSTER_MANUAL_FORMAT))
                {
                    string monsterLine;
                    while ((monsterLine=reader.ReadLine())!=null)
                    {
                        Monster monster = MonsterParser.parseMonsterFromString(monsterLine);
                        monsters.Add(monster);
                    }
                }
            }
            return monsters;
        }

        public void SaveMonsterManual(List<Monster> monsters) {
            using (StreamWriter writer=new StreamWriter(Constants.MONSTER_MANUAL_FILE))
            {
                writer.WriteLine(Constants.MONSTER_MANUAL_FORMAT);
                foreach (Monster monster in monsters)
                {
                    writer.WriteLine(MonsterParser.parseMonsterIntoString(monster));
                }
            }
        }

        public Monster ImportMonster(string filePath)
        {
            using (StreamReader reader=new StreamReader(filePath))
            {
                string fileFormat = reader.ReadLine();
                if (fileFormat.Equals(Constants.MONSTER_FILE_FORMAT))
                {
                    return MonsterParser.parseMonsterFromString(reader.ReadLine());
                }
            }
            throw new FileFormatException("Tohle není soubor s monstrem!");
        }

        public void ExportMonster(string filePath,Monster monster)
        {
            using (StreamWriter writer=new StreamWriter(filePath))
            {
                writer.WriteLine(Constants.MONSTER_FILE_FORMAT);
                writer.WriteLine(monster.ToString());
            }
        }


    }
}
