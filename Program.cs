using System.Runtime.CompilerServices;

namespace Otladka
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "leaderboard.json";
            LeaderBoard leaderBoard = new LeaderBoard(filePath);
            string login = null;
            string password = null;
            bool auth = false;

            while (!auth)
            {
                Console.WriteLine("\nВыберите действие: 1 - Регистрация, 2 - Вход");
                int choice = int.Parse(Console.ReadLine());

                if (choice == 1)
                {
                    Console.Write("Введите логин: ");
                    login = Console.ReadLine();
                    Console.Write("Введите пароль: ");
                    password = Console.ReadLine();

                    if (leaderBoard.RegisterUser(login, password))
                    {
                        Console.WriteLine("Регистрация прошла успешно.");
                        auth = true;
                    }
                }
                else if (choice == 2)
                {
                    Console.Write("Введите логин: ");
                    login = Console.ReadLine();
                    Console.Write("Введите пароль: ");
                    password = Console.ReadLine();

                    if (leaderBoard.AuthUser(login, password))
                    {
                        Console.WriteLine("Вход выполнен.");
                        auth = true;
                       
                    }
                    else
                    {
                        Console.WriteLine("Неверный логин или пароль.");
                    }
                }
                else
                {
                    Console.WriteLine("Некорректный выбор.");
                }
            }
            
            int rightPlace = 0;
            int guessed = 0;
            int res = 0;
            Random random = new Random();

            //массив цифр от 0 до 9
            int[] digits = Enumerable.Range(0, 10).ToArray();

            //рандомно перемешиваю массив
            digits = digits.OrderBy(x => random.Next()).ToArray();

            //Проверяю, чтобы первая цифра не была 0
            if (digits[0] == 0)
            {
                //Меняю местами ноль с первой ненулевой цифрой
                for (int i = 1; i < digits.Length; i++)
                {
                    if (digits[i] != 0)
                    {
                        int temp = digits[0];
                        digits[0] = digits[i];
                        digits[i] = temp;
                        break;
                    }
                }
            }
            //первые 4 цифры - загаданное число
            string number = string.Join("", digits.Take(4));
            Console.WriteLine("\nЗагадано 4-значное число с неповторяющимися цифрами.");

            //Цикл для отгадывания числа
            while (true)
            {
                rightPlace = 0;
                guessed = 0;
                Console.Write("Попробуйте угадать 4-значное число: ");
                string check = Console.ReadLine();

                //проверка длины введенной пользователем строки
                if (check.Length != 4)
                {
                    Console.WriteLine("В числе должно быть 4 знака!\n");
                    continue;
                }

                //Проверка введеной пользователем строки на целое число
                if (!int.TryParse(check, out int parsed))
                {
                    Console.WriteLine("Запись числа должна состоять только из цифр!\n");
                    continue;
                }

                //засчитываю попытку
                res++;

                //перебираю символы загаданного числа, сравнивая с введенным пользователем
                foreach (char c in number)
                {
                    foreach (char chr in check)
                    {
                        if (chr.Equals(c))
                        {
                            guessed++;
                            break;
                        }
                    }
                }
                
                //проверка верного расположения символов
                for (int i = 0; i < 4; i++)
                {
                    if (number[i].Equals(check[i]))
                        rightPlace++;
                }
                
                //если число угадано
                if (rightPlace == 4)
                {
                    leaderBoard.AddScore(login, res);
                    int percent = leaderBoard.ShowPlace(login, res);
                    Console.WriteLine("Вы угадали число за " + res + " попыток(ки)! Этот результат входит в " + percent + "% лучших!");
                    break;
                }
                else
                    Console.WriteLine(guessed + " угадано, " + rightPlace + " на верном месте! Пробуйте еще.\n");
                
            }
            Console.ReadLine();
        }
    }
}
