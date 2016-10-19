using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DungeonMasterScreen.Model;
using DungeonMasterScreen.Core;
using DungeonMasterScreen.Controller;
using DungeonMasterScreen.Properties;
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
                try
                {
                    string fileFormat = reader.ReadLine();
                    if (fileFormat.Equals(Constants.MONSTER_MANUAL_FORMAT))
                    {
                        string monsterLine;

                        while ((monsterLine = reader.ReadLine()) != null)
                        {
                            Monster monster = MonsterParser.parseMonsterFromString(monsterLine);
                            monsters.Add(monster);
                        }
                    }
                }
                catch (Exceptions.MonsterParseException e)
                {
                    throw new FileFormatException(Resources.FC_OPEN_FAILURE);
                }
            }
            return monsters;
        }

        public void SaveMonsterManual(List<Monster> monsters)
        {
            using (StreamWriter writer = new StreamWriter(Constants.MONSTER_MANUAL_FILE))
            {
                try
                {
                    writer.WriteLine(Constants.MONSTER_MANUAL_FORMAT);
                    foreach (Monster monster in monsters)
                    {
                        writer.WriteLine(MonsterParser.parseMonsterIntoString(monster));
                    }
                }
                catch (IOException e)
                {
                    throw new Exceptions.MonsterManualOpenException(string.Format(Resources.FC_SAVE_FAILURE,e.Message));
                }
                
            }
        }

        /// <summary>
        /// Importuje jedno monstrum ze souboru. Soubor musí mít formát uloženého monstra.
        /// Metoda vyhazuje dvě výjimky: <see cref="IOException"/> v případě, že dojde k chbě čtení ze souboru nebo <see cref="FileFormatException"/> pokud je soubor ve špatném formátu.
        /// </summary>
        /// <param name="filePath">Cesta k souboru, ze kterého se má monstrum načíst</param>
        /// <returns>Novou instanci <see cref="Monster"/> </returns>
        public Monster ImportMonster(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string fileFormat = reader.ReadLine();
                if (fileFormat.Equals(Constants.MONSTER_FILE_FORMAT))
                {
                    return MonsterParser.parseMonsterFromString(reader.ReadLine());
                }
            }
            throw new FileFormatException(Resources.FC_FORMAT_ERROR);
        }

        public void ExportMonster(string filePath, Monster monster)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(Constants.MONSTER_FILE_FORMAT);
                writer.WriteLine(monster.ToString());
            }
        }


    }
}
