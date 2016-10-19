using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterScreen.Model;
using DungeonMasterScreen.Core;
using DungeonMasterScreen.Exceptions;
using DungeonMasterScreen.Properties;

namespace DungeonMasterScreen.Controller
{
    /// <summary>
    /// Class responsible for parsing <see cref="Model.Monster"/> into <see cref="Model.MonsterDto"/>.
    /// Also can convert <see cref="Monster"/> from string and into string.
    /// </summary>
    class MonsterParser
    {
        public static string parseMonsterIntoString(Monster monster)
        {
            return monster.ToString();
        }

        public static string parseMonsterDtoIntoString(MonsterDto monsterDto)
        {
            return monsterDto.ToString();
        }

        public static Monster parseMonsterFromString(string monsterString)
        {
            string[] parts=monsterString.Split(Constants.DELIMITER);
            if (parts.Length==Constants.NUMBER_OF_ATTRIBUTES)
            {
                return parseAttributes(parts);
            }
            else
            {
                throw new MonsterParseException(String.Format(Resources.MP_INVALID_MONSTER_FORMAT, parts.Length, Constants.NUMBER_OF_ATTRIBUTES));
            }
        }

        private static Monster parseAttributes(string[] parts)
        {
            Monster monster = new Monster(IdentityGenerator.GetNewId());
            monster.Name = parts[0];
            monster.Initiative = int.Parse(parts[1]);
            monster.Health = int.Parse(parts[2]);
            monster.AttackBonusess = parts[3];
            monster.Damage = parts[4];
            monster.Defense = int.Parse(parts[5]);
            monster.Effects = parts[6];
            return monster;
        }

        public static MonsterDto convertMonsterIntoDto(Monster source)
        {
            MonsterDto dto = new MonsterDto();
            CopyAttributes(source, dto);
            return dto;
        }

        public static Monster createNewInstanceOfMonster(MonsterDto sourceDto)
        {
            Monster monster = new Monster(IdentityGenerator.GetNewId());
            CopyAttributes(sourceDto, monster);
            return monster;
        }

        public static void CopyAttributes(Monster source, MonsterDto destination)
        {
            destination.id = source.Id;
            destination.name = source.Name;
            destination.initiative = source.Initiative.ToString();
            destination.lifes = source.Health.ToString();
            destination.attackBonusess = source.AttackBonusess;
            destination.damage = source.Damage;
            destination.defense = source.Defense.ToString();
            destination.effects = source.Effects;
        }

        public static void CopyAttributes(MonsterDto source, Monster destination)
        {
            destination.Name = source.name;
            destination.Initiative = parseNonEmptyStringIntoInteger(source.initiative);
            destination.Health = parseNonEmptyStringIntoInteger(source.lifes);
            destination.AttackBonusess = source.attackBonusess;
            destination.Damage = source.damage;
            destination.Defense = parseNonEmptyStringIntoInteger(source.defense);
            destination.Effects = source.effects;
        }

        private static int parseNonEmptyStringIntoInteger(string source)
        {
            if (source==null || source.Equals(String.Empty))
            {
                return 0;
            }
            else
            {
                return int.Parse(source);
            }
        }


        public static void ValidateMonsterDto(MonsterDto dto)
        {
            if (dto == null)
            {
                throw new ValidationException(Resources.MP_EMPTY_MONSTER);
            }
            int numberVariable;
            if (!int.TryParse(dto.initiative, out numberVariable))
            {
                throw new ValidationException(Resources.MP_INITIATIVE_WARNING);
            }
            if (dto.name == null)
            {
                throw new ValidationException(Resources.MP_NAME_WARNING);
            }
            if (dto.defense != null && dto.defense != String.Empty && !int.TryParse(dto.defense, out numberVariable))
            {
                throw new ValidationException(Resources.MP_DEFENSE_WARNING);
            }
            if (dto.lifes != null && dto.lifes != String.Empty && !int.TryParse(dto.lifes, out numberVariable))
            {
                throw new ValidationException(Resources.MP_LIFE_WARNING);
            }
        }

    }
}
