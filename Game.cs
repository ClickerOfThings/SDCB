using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RPGshechka
{
    public class MainClass
    {

        public static MainHero player = new MainHero("Null");
        static void Main(string[] args)
        {
            Random rand = new Random();
            Console.WriteLine("Приветствует тебя игра SDCB.\nСейчас ты создашь своего героя.");
            Thread.Sleep(1000);
            Console.WriteLine("Назови своего будущего странника.");
            Console.Write(">");
            player.Name = Console.ReadLine();
            Console.Clear();
            player.ChoosePath();
            Console.WriteLine("Имя: " + player.Name);
            Console.WriteLine("Твои начальные данные:");
            Console.WriteLine("Здоровье: " + player.Health);
            Console.WriteLine("Мана: " + player.Mana);
            Console.WriteLine("Стамина: " + player.Stamina);
            Console.WriteLine("Начальное баблишко: " + player.Money);
            Thread.Sleep(4000);
            Console.Clear();
            // Engine.Tech tech = new Engine.Tech(); за временной ненадобностью
            Thread info = new Thread(Engine.Tech.UpdateScreen);
            info.Start();
            while (true)
            {
                if (Engine.Tech.endgame == true)
                {
                    break;
                }
                Thread.Sleep(500);
            }
            // Console.Clear();
            Console.ReadKey();
        }
    }

}