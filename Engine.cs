using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RandomWordGenerator;

namespace RPGshechka
{
    public class Engine
    {
        public static List<string> skills_list = new List<string> { "strength", "defense", "charisma", "barter" }; // скиллы
        public static void ShowSkills(){ // показать скиллы
            Console.Clear();
            int n = 0;
            List<int> a = MainClass.player.GetSkills();
            foreach (int s in a)
            {
                List<string> list = new List<string> { "Сила", "Защита", "Харизма", "Торговец" };
                Console.WriteLine($"Скилл {list[n]}: {s}");
                n++;
            }
            Console.WriteLine("\nНажми, чтобы продолжить...");
            Console.ReadKey();
        }
        public static void ShowInventory(){ // показать инвентарь
            Console.Clear();
            Console.WriteLine("Ваш инвентарь:");
            foreach (Items x in MainClass.player.inventory){
                Console.Write(x.Name);
                if (x.Up == 0)
                {
                    Console.WriteLine("\tПросто для красоты, ничего не дает");
                }
                else{
                    Console.WriteLine("\tПлюс к "+x.SkillUp + " на "+x.Up+" стоимостью "+x.cost+". ID "+x.item_id);
                }
            }
            Console.WriteLine("\nНажмите, чтобы продолжить...");
            Console.ReadKey();

        }
        private static List<Items> trader_items = new List<Items>(); // предметы торговца
        private static bool clearTrader = true; // false - предметы остаются, true - очищается ассортимент
        public static void Trader(){ // торговец
            Console.Clear();
            Console.WriteLine("О, да ты проходи, у меня ассортимент хороший, гляди, чего прикупишь.");
            Console.Write("У меня каждые 5 минут новый товар поставляется! Но старый, к сожалению, выбрасывать надо. ");
            if (clearTrader){
                Console.WriteLine("Ну и как раз у меня новенькое есть!");
            }
            else{
                Console.WriteLine("К сожалению, на данный момент пока ничего нового нет.");
            }
            Console.WriteLine("Сейчас, покажу весь ассортимент...");
            if (clearTrader){
                trader_items.Clear();
                Random rand_name = new Random();
                for (int i = 0; i < 10; i++){
                    string name = Generator.GenerateWord(rand_name.Next(3, 12));
                    name = name.First().ToString().ToUpper() + name.Substring(1);
                    trader_items.Add(new Items(name, skills_list[rand_name.Next(0, skills_list.Count - 1)], rand_name.Next(0, 7), trader:true));
                    Thread.Sleep(100);
                }
                clearTrader = false;
                Thread clearItems = new Thread(TraderClearTimer);
                clearItems.Start();
            }

            void ShowItems() //показать предметы
            {
                foreach (Items x in trader_items)
                {
                    Console.WriteLine(x.Name + "\tплюс к " + x.SkillUp + " на " + x.Up+ ". ID = " + x.item_id+" СТОИМОСТЬ "+ x.cost);
                }
            }

            void sellItems(){ //продать предметы
                foreach (Items x in MainClass.player.inventory)
                {
                    Console.WriteLine(x.Name + "\tплюс к " + x.SkillUp + " на " + x.Up + ". ID = " + x.item_id + " СТОИМОСТЬ " + x.cost);
                }
                string choose = Console.ReadLine();
                if (choose == "вых" || choose == "ext")
                {
                    Console.WriteLine("Не хочешь продавать? Ну ладно...");
                    Thread.Sleep(3000);
                    Console.Clear();
                    return;
                }
                int n = 1;
                foreach (Items item in MainClass.player.inventory)
                {
                    try
                    {
                        if (item.item_id == Convert.ToInt64(choose))
                        {
                            n = 0;
                            break;
                        }
                        else
                        {
                            n++;
                        }
                    }
                    catch (FormatException)
                    {
                        n++;
                        break;
                    }
                }
                if (n > 0)
                {
                    Console.WriteLine("Да нет же у тебя такого! Еще меня мошенником называют...");
                    Thread.Sleep(1000);
                }
                else
                {
                    int cost = MainClass.player.inventory.Find(x => x.item_id == Convert.ToInt64(choose)).cost;
                    MainClass.player.Money = cost;
                    MainClass.player.inventory.Remove(MainClass.player.inventory.Find(x => x.item_id == Convert.ToInt64(choose)));
                    MainClass.player.UpdateSkillInventory();
                    Console.WriteLine("Вот тебе "+cost+".");
                    Thread.Sleep(1000);
                }
                Console.Clear();
            }
            while (true){
                ShowItems();
                Thread.Sleep(500);
                Console.WriteLine("Что купить хочешь?\n(Введите айди предмета, введите 'вых' или 'ext' чтобы выйти)");
                Console.WriteLine("('sell' или 'прод' - продать свои предметы)");
                string choose = Console.ReadLine();
                if (choose == "вых"||choose == "ext"){
                    Console.WriteLine("Ну ладно, приходи еще!");
                    break;
                }
                else if (choose == "sell"||choose=="прод"){
                    Console.Clear();
                    Console.WriteLine("О, ты мне что-то продать хочешь? С радостью выкуплю, только покажи что есть");
                    Console.WriteLine("(Введите айди предмета, введите 'вых' или 'ext' чтобы выйти)\n");
                    sellItems();
                }
                else{
                    int n = 0;
                    foreach (Items item in trader_items){
                        try{
                        if (item.item_id == Convert.ToInt64(choose)){
                            n = 0;
                            break;
                        }
                        else{
                            n++;
                        }
                        }
                        catch (FormatException){
                            n++;
                            break;
                        }
                    }
                    if (n > 0)
                    {
                        Console.WriteLine("Да нет же у меня такого!");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        if (trader_items.Find(x => x.item_id == Convert.ToInt64(choose)).cost > MainClass.player.Money){
                            Console.WriteLine("А у тебя денег столько нет!");
                            Thread.Sleep(1000);
                        }
                        else{
                            MainClass.player.inventory.Add(trader_items.Find(x => x.item_id == Convert.ToInt64(choose)));
                            int cost = trader_items.Find(x => x.item_id == Convert.ToInt64(choose)).cost * -1;
                            MainClass.player.Money = cost;
                            trader_items.Remove(trader_items.Find(x => x.item_id == Convert.ToInt64(choose)));
                            MainClass.player.UpdateSkillInventory();
                            Console.WriteLine("Спасибо за покупку!");
                            Thread.Sleep(1000);
                        }
                    }
                    Console.Clear();
                }
            }
            Console.ReadKey();
        }
        private static void TraderClearTimer(){ // очистка ассортимента у торгаша
            Thread.Sleep(300000);
            clearTrader = true;
            return;
        }

        // данж
        private static void Dungeon()
        {
            Console.Clear();
            Console.Write("Вы собираетесь в данж ");
            Thread.Sleep(1000);
            Console.Write(". ");
            Thread.Sleep(1000);
            Console.Write(". ");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.ReadKey();
        }
        public class Tech
        {
            protected internal static Dictionary<char, Action> commands = new Dictionary<char, Action> { { 's', Engine.ShowSkills },
            {'t',  Engine.Trader}, {'i', Engine.ShowInventory } }; //команды
            public List<char> list_commands = new List<char>(); // кажется эта хуйня еще не пригодилась, но оставлю

            public bool endgame = false;
            public void UpdateScreen() // показатель всей хуйни на главном экране
            {
                while (true)
                {
                    Console.WriteLine("Имя: " + MainClass.player.Name);
                    Console.WriteLine("ЗДР " + MainClass.player.Health + " МАН " + MainClass.player.Mana + " СТМ " + MainClass.player.Stamina+ " ДЕН " + MainClass.player.Money);
                    if (MainClass.player.CheckLVL() == true)
                    {
                        Console.WriteLine("Уровень повышен! Теперь твой уровень " + MainClass.player.LvL);

                    }
                    Console.WriteLine("УРВ " + MainClass.player.LvL + " ОПТ " + MainClass.player.experience);
                    Console.WriteLine("\n\ns - скиллы, i - инвентарь, t - торговец, d - пойти в данж");
                    MainClass.player.UpdateSkillInventory();
                    AwaitKey();
                    Thread.Sleep(100);
                    Console.Clear();
                }
            }
            public void AwaitKey() //ожидание команды
            {
                char choose = Console.ReadKey().KeyChar;
                if (!commands.Keys.Contains(choose))
                {
                    return;
                }
                commands[choose]();
            }
        }
    }
    
}