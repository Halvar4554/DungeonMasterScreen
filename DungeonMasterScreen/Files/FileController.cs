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
using DungeonMasterScreen.Exceptions;
using System.Reflection;

namespace DungeonMasterScreen.Files
{
    public class FileController
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
                    throw new Exceptions.MonsterManualOpenException(string.Format(Resources.FC_SAVE_FAILURE, e.Message));
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

        public EncounterCarrier ImportEncounter(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                try
                {
                    string firstLine = reader.ReadLine();
                    string[] lineParts = firstLine.Split(' ');
                    if (lineParts != null && lineParts.Length == Constants.FIGHT_FORMAT_PARAMS_COUNT && lineParts[Constants.FIGHT_FORMAT_INDEX].Equals(Constants.FIGHT_FILE_FORMAT))
                    {
                        EncounterCarrier result = new EncounterCarrier();
                        extractBasicAttributes(lineParts, result);
                        readEncounterFile(reader, result, parseLineParam(lineParts[Constants.MONSTER_COUNT_INDEX]));
                    }
                }
                catch (IOException e)
                {

                    throw new FileFormatException(Resources.FC_ENCOUNTER_IMPORT_ERROR);
                }
            }
            throw new FileFormatException(Resources.FC_ENCOUNTER_IMPORT_ERROR);

        }

        private void extractBasicAttributes(string[] lineParts, EncounterCarrier result)
        {
            try
            {
                result.ActualTurn = parseLineParam(lineParts[Constants.ACTUAL_TURN_INDEX]);
                result.ActualCombatant = parseLineParam(lineParts[Constants.ACTUAL_COMBATANT_INDEX]);
            }
            catch (FightParamParsingException e)
            {
                throw new FileFormatException(e.Message);
            }
        }

        /// <summary>
        /// Try to parse string param into int value. If it fails it means that format of param is invalid and <see cref="FightParamParsingException"> is thrown.
        /// </summary>
        /// <param name="param">Parameter which will be parsed into int value.</param>
        /// <returns>int value of input param.</returns>
        private int parseLineParam(string param)
        {
            int result = 0;
            if (!int.TryParse(param, out result))
            {
                throw new FightParamParsingException(Resources.FC_ENCOUNTER_IMPORT_ERROR);
            }
            return result;
        }

        /// <summary>
        /// This method read file with encounter and parse it into <see cref="EncounterCarrier"/>. Its important to read first monsters and only then log entries.
        /// </summary>
        /// <param name="reader">Source from which data are read</param>
        /// <param name="result">Parsed <see cref="EncounterCarrier"/> with filled Monsters and log entries</param>
        /// <param name="monsterCount">Count of monsters in file.</param>
        private void readEncounterFile(StreamReader reader, EncounterCarrier result, int monsterCount) {
            result.Monsters = readMonsters(reader, monsterCount);
            result.BattleLog = readBattlelog(reader);
        }

        private List<Monster> readMonsters(StreamReader reader, int count)
        {
            List<Monster> parsedMonsters = new List<Monster>(count);
            for (int i = 0; i < count; i++)
            {
                parsedMonsters.Add(MonsterParser.parseMonsterFromString(reader.ReadLine()));
            }
            return parsedMonsters;
        }

        private List<string> readBattlelog(StreamReader reader) {
            List<string> logEntries = new List<string>();
            string line;
            while ((line=reader.ReadLine())!=null)
            {
                logEntries.Add(line);
            }
            return logEntries;
        }

    }
}
