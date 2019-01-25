using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RPGshechka
{
    public class Hero
    {
        public string Name { get; set; }
        private int health, mana, stamina;
        public int Health
        {
            get => health;
            set => health = value > maxStat ? 150 : value;
        }
        public int Mana
        {
            get => mana;
            set => mana = value > maxStat ? 150 : value;
        }
        public int Stamina
        {
            get => stamina;
            set => stamina = value > maxStat ? 150 : value;
        }
        private const int maxStat = 150;
        public int LvL = 1;
        public int experience = 0;
        protected Dictionary<string, int> skills = new Dictionary<string, int>();
        public List<Items> inventory = new List<Items>(); //инвентарь
        public List<Items> updated = new List<Items>(); //предметы, которые уже обновили скиллы, чтобы по второму разу не обновлять
        public List<int> GetSkills()
        {
            List<int> skillReturn = new List<int>();
            foreach (int s in skills.Values)
            {
                skillReturn.Add(s);
            }
            return skillReturn;
        }
        public void SetSkill(string skill, int setskill, bool add = true) // добавить число к скиллу или убавить
        {
            if (!skills.Keys.Contains(skill))
            {
                Console.WriteLine("Ошибка во время записи скилла");
                return;
            }
            if (add == true) { skills[skill] += setskill; }
            else
            {

                if (skills[skill] < setskill)
                {
                    skills[skill] = 1;
                }
                else
                {
                    skills[skill] -= setskill;
                }
            }
            Console.WriteLine("Скиллы обновлены (метод SetSkill)");

        }
        public Hero()
        { // изначальные значения скиллов
            skills.Add("strength", 1);
            skills.Add("defense", 1);
            skills.Add("charisma", 1);
            skills.Add("barter", 1);
        }
        /* public void UpdateSkillInventory() //обновление скиллов через предметы в инвентаре
        {
            foreach (Items item in inventory)
            {
                if (MainClass.player.updated.Contains(item))
                {
                    continue;
                }
                if (item.SkillUp == null || item.Up == 0)
                {
                    continue;
                }
                MainClass.player.SetSkill(item.SkillUp, item.Up);
                updated.Add(item);
            }
            foreach (Items item in updated.ToArray())
            {
                if (!(MainClass.player.inventory.Contains(item)))
                {
                    MainClass.player.SetSkill(item.SkillUp, item.Up, false);
                    MainClass.player.updated.Remove(item);
                }
                else
                {
                    continue;
                }
            }
            Console.WriteLine("Скиллы обновлены (инвентарь).");
        } */
        public void UpdateSkillInventory() //обновление скиллов через предметы в инвентаре
        {
            foreach (Items item in inventory)
            {
                if (updated.Contains(item))
                {
                    continue;
                }
                if (item.SkillUp == null || item.Up == 0)
                {
                    continue;
                }
                SetSkill(item.SkillUp, item.Up);
                updated.Add(item);
            }
            foreach (Items item in updated.ToArray())
            {
                if (!(inventory.Contains(item)))
                {
                    SetSkill(item.SkillUp, item.Up, false);
                    updated.Remove(item);
                }
                else
                {
                    continue;
                }
            }
            Console.WriteLine("Скиллы обновлены (инвентарь).");
        }

        // S D C B
    }

    public class MainHero : Hero
    {
        private long money = 0;
        public long Money { get => money; set => money += value; } // еб твою налево бляьь........
        public MainHero(string name = "Hero", params int[] skill)
        {
            Random rand = new Random();
            Name = name;
            Health = rand.Next(70, 101);
            Mana = rand.Next(70, 91);
            Stamina = rand.Next(50, 96);
            Money = rand.Next(1, 30);
            int n = 0;
            foreach (string x in skills.Keys.ToList())
            {
                if (n > skill.Length - 1)
                {
                    break;
                }
                skills[x] = skill[n];
                n++;
            }
            UpdateSkillInventory();
        }
        public void ChoosePath()
        { //ну собсна создание перса, точнее скиллы
            char[] buttons = new char[4] { 'z', 'x', 'c', 'v' };
            Dictionary<char, string> prof = new Dictionary<char, string>();
            string[] classes = new string[] { "strength", "defense", "charisma", "barter" };
            Dictionary<char, string> firstItem = new Dictionary<char, string> { { 'z', "Фиолетовый меч" }, { 'x', "Щит с фонариком" },
            {'c', "Маска Гая Фокса" }, {'v', "Кошерный мешочек" } };
            int n = 0;
            foreach (string x in classes)
            {
                prof.Add(buttons[n], x);
                n++;
            }
            char choose = '\0';
            Console.WriteLine("Выберите же свою судьбу как героя этих мест:");
            Console.WriteLine("z - Обучающийся воин, x - Искусный тренер по самозащите\nc - Очень Привлекательный Самец, v - Торговец-еврей");
            while (!(buttons.Contains(choose)))
            {
                choose = Console.ReadKey().KeyChar;
            }
            SetSkill(prof[choose], 2, true);
            Console.Clear();
            choose = '\0';
            Console.WriteLine("Теперь же выбери того, кем ты был в детстве: ");
            Console.WriteLine("z - Задира в православной школе, x - Строитель замков из подушек\nc - Профессиональный врун, v - Обменщик листьев");
            while (!(buttons.Contains(choose)))
            {
                choose = Console.ReadKey().KeyChar;
            }
            SetSkill(prof[choose], 1, true);
            Console.Clear();
            choose = '\0';
            Console.WriteLine("Выбери свое начальное оружие: ");
            Console.WriteLine("z - Меч со странным фиолетовым оттенком, x - Щит с фонариком\nc - Маска Гая Фокса, v - Кошерный мешочек с псевдочерной дырой внутри");
            while (!(buttons.Contains(choose)))
            {
                choose = Console.ReadKey().KeyChar;
            }
            MainClass.player.inventory.Add(new Items(firstItem[choose], classes[firstItem.Keys.ToList().IndexOf(choose)], 1));
            Console.Clear();
            Console.WriteLine("Приветствуем тебя на новой земле, " + this.Name + ", и да пребудут с тобой боги!");
            Thread.Sleep(2000);
            Console.Clear();
        }

        public bool CheckLVL(bool upgrade=false)
        {
            if (experience > (LvL + 2) * 1000)
            {
                if (upgrade)
                {
                    LvL += 1;
                }

                return true;

            }
            else
            {
                return false;
            }

        }
    }

    public class NPC : Hero
    {
        public NPC(string name = null, params int[] znacheniya) //health mana stamina
        {
            if (znacheniya.Length < 3 || name is null)
            {
                throw new ArgumentException();
            }
            Name = name;
            Health = znacheniya[0];
            Mana = znacheniya[1];
            Stamina = znacheniya[2];
            UpdateSkillInventory();

        }

    }
}