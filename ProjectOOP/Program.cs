using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ProjectOOP
{
    delegate void MyEventHandler();
    class MyEvent
    {
        public event MyEventHandler SomeEvent;
        public void OnSomeEvent()
        {
            SomeEvent?.Invoke();
        }

    }
    class Footballer
    {
        public string name;
        public int age;
        public int level;
        public Footballer(string name, int age)
        {
            this.name = name;
            this.age = age;
            Random r = new Random();
            level = r.Next(0, 101);
        }
    }
    class Team
    {
        public string team_name;
        List<Footballer> team = new List<Footballer>();
        public int team_level;
        public Coach coach;
        public Fans fans;
        List<Fans> ff = new List<Fans>();

        public Team(string team_name, Coach coach, Fans fans)
        {
            this.team_name = team_name;
            this.coach = coach;
            this.fans = fans;
        }
        public void AddFootbaler(Footballer player)
        {
            // кол-во игроков в команде должно быть не выше 11
            if (team.Count == 11)
                Console.WriteLine("Команда заполнена!");
            else
            {
                team.Add(player);
                foreach (var i in team)
                    team_level += i.level;
                // баллы фанатов прибавляются к уровню команды
                foreach (var i in ff)
                    team_level += i.level;
                team_level = (int)(team_level * coach.level);
            }
        }
        // удаление игрока
        public void DeleteFootballer(Footballer player)
        {
            if (team.Contains(player))
                team.Remove(player);
            else
                Console.WriteLine("Данного игрока нет в команде!");
            Console.WriteLine();
        }
        public void ListAll()
        {
            var t = from x in team
                    orderby x.name
                    select x;
            foreach (var i in t)
                Console.WriteLine(i.name);
        }
        public void ListOverThirty()
        {
            var t = from x in team
                    where x.age > 30
                    orderby x.name descending
                    select x;
            foreach (var i in t)
                Console.WriteLine(i.name);
        }
        // сортирую по уровню мастерства
        public void ListOfLevels()
        {
            var t = from x in team
                    orderby x.level
                    select x;
            foreach (var i in t)
                Console.WriteLine(i.name);
        }
        public void ListOfFans()
        {
            var t = from x in ff
                    group x by new
                    {
                        x.level,
                        x.name
                    };
            foreach (var i in ff)
                Console.WriteLine(i);
        }
    }
    class Game
    {
        public Team team1;
        public Team team2;
        public Referee referee;
        public Game(Team team01, Team team02, Referee referee2)
        {
            team1 = team01;
            team2 = team02;
            referee = referee2;
        }
        public void Result()
        {
            Start(referee);

            if (team1.team_level <= 0) team1.team_level = 1;
            if (team2.team_level <= 0) team2.team_level = 1;
            if (referee.level == 1) team1.team_level += 40;
            if (referee.level == 2) team2.team_level += 40;
            Console.WriteLine($"Уровень команды {team1.team_name} - {team1.team_level}");
            Console.WriteLine($"Уровень команды {team2.team_name} - {team2.team_level}");
            if ((Math.Max(team1.team_level, team2.team_level) / Math.Min(team1.team_level, team2.team_level) - 1) * 100 <= 10)
                Console.WriteLine("Ничья");
            else if (team1.team_level > team2.team_level)
                Console.WriteLine($"Победила команда {team1.team_name}");
            else
                Console.WriteLine($"Победила команда {team2.team_name}");

            Console.WriteLine();
            Console.WriteLine($"Список команды {team1.team_name} по алфовиту");
            team1.ListAll();
            Console.WriteLine();
            Console.WriteLine($"Участники команды {team2.team_name}, которые старше 30");
            team2.ListOverThirty();
            Console.WriteLine();
            Console.WriteLine($"Участники команды {team1.team_name} по их уровню");
            team1.ListOfLevels();
            Console.WriteLine();
            Console.WriteLine($"Участники команды {team2.team_name} по их уровню");
            team2.ListOfLevels();
        }
        public void Except(int c1, int c2)
        {
            try
            {
                if (c1 >= 3)
                {
                    team1.team_level -= 5;
                    Console.WriteLine($"У команды {team1.team_name} отняли 5 баллов!");
                }
                if (c2 >= 3)
                {
                    team2.team_level -= 5;
                    Console.WriteLine($"У команды {team2.team_name} отняли 5 баллов!");
                }
            }
            catch when (c1 >= 3)
            {
                Console.WriteLine($"У команды {team1.team_name} отняли 5 баллов!");
            }
            catch when (c2 >= 3)
            {
                Console.WriteLine($"У команды {team2.team_name} отняли 5 баллов!");
            }
        }
        public void Start(Referee referee)
        {
            Random r = new Random();
            int count1 = 0, count2 = 0;
            int temp1 = count1, temp2 = count2;
            for (int i = 0; i < r.Next(10, 20); i++)
            {
                int teamNum, sub;
                MyEvent evt = new MyEvent();
                sub = r.Next(0, 3);
                switch (sub)
                {
                    case 1:
                        evt.SomeEvent += new MyEventHandler(referee.Foul);
                        evt.OnSomeEvent();

                        teamNum = r.Next(1, 3);
                        if (teamNum == 1)
                        {
                            count1++;
                            Console.WriteLine($" команде {team1.team_name}!");
                        }
                        else if (teamNum == 2)
                        {
                            count2++;
                            Console.WriteLine($" команде {team2.team_name}!");
                        }
                        break;
                    case 2:
                        evt.SomeEvent += new MyEventHandler(referee.Goul);
                        evt.OnSomeEvent();

                        teamNum = r.Next(1, 3);
                        if (teamNum == 1)
                        {
                            Console.WriteLine($" команде {team1.team_name}!");
                            // добавляю за каждый гол по 2 балла
                            team1.team_level += 2;
                        }
                        else if (teamNum == 2)
                        {
                            Console.WriteLine($" команде {team2.team_name}!");
                            team2.team_level += 2;
                        }
                        break;
                }
                if (count1 - temp1 == 3 || count2 - temp2 == 3)
                {
                    Except(count1, count2);
                    temp1 = count1; temp2 = count2;
                }
            }
            Console.WriteLine();
        }
    }
    class Coach
    {
        public string name;
        public double level;
        public Coach(string name)
        {
            this.name = name;
            Random r = new Random();
            level = r.Next(1, 4) / 2;
        }
    }
    class Referee
    {
        public string name;
        public int level;
        public Referee(string name)
        {
            this.name = name;
            Random r = new Random();
            level = r.Next(0, 3);
        }
        public void Foul()
        {
            Console.Write("Судья поднял карточку");
        }
        public void Goul()
        {
            Console.Write("Судья засчитал гол");
        }
    }
    class Fans
    {
        public string name;
        public int level;
        public Fans(List<string> names)
        {
            Random r = new Random();
            foreach (string i in names)
            {
                this.name = i;
                this.level = r.Next(0, 6);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<string> f1 = new List<string>() { "Маша", "Витя", "Коля", "Саша", "Алеша", "Дима", "Оля", "Вова" };
            Fans fans1 = new Fans(f1);
            List<string> f2 = new List<string>() { "Степа", "Богдан", "Олег", "Соня", "Катя", "Сережа", "Андрей", "Егор", "Костя" };
            Fans fans2 = new Fans(f2);
            Referee referee = new Referee("Шикский");
            Coach coach1 = new Coach("Арсеньев");
            Coach coach2 = new Coach("Инокентьев");
            Team team1 = new Team("Топтыжки", coach1, fans1);
            Team team2 = new Team("Гаврики", coach2, fans2);
            Footballer player1 = new Footballer("Месси", 28);
            team1.AddFootbaler(player1);
            Footballer player2 = new Footballer("Рональду", 31);
            team1.AddFootbaler(player2);
            Footballer player3 = new Footballer("Руни", 26);
            team2.AddFootbaler(player3);
            Footballer player4 = new Footballer("Ибрагимович", 34);
            team2.AddFootbaler(player4);
            Footballer player5 = new Footballer("Вачовски", 41);
            team2.AddFootbaler(player5);

            team2.DeleteFootballer(player3);
            team2.DeleteFootballer(player1);

            Game newGame = new Game(team1, team2, referee);
            newGame.Result();
        }
    }
}