using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Otladka
{
    [Serializable]
    internal class LeaderBoard
    {

        private List<User> users;
        private string filePath;

        public LeaderBoard(string filePath)
        {
            this.filePath = filePath;
            LoadUsers();
        }
        public bool AuthUser(string login, string password)
        {
            //Поиск пользователя по логину и проверка пароля
            var user = users.FirstOrDefault(u => u.Login == login && u.Password == password);
            return (user != null);
        }

        public bool RegisterUser(string login, string password)
        {
            // Проверяем, существует ли пользователь с таким логином
            if (users.Any(u => u.Login == login))
            {
                Console.WriteLine("Пользователь с таким логином уже существует!");
                return false;
            }

            //Создаю нового пользователя и добавляю его в список пользователей (после сразу сериализация)
            users.Add(new User { Login = login, Password = password });
            SaveUsers();
            return true;
        }

        //Добавление результата для конкретного пользователя
        public void AddScore(string login, int score)
        {
            var user = users.FirstOrDefault(u => u.Login == login);
            if (user != null)
            {
                user.Scores.Add(score);
                SaveUsers();
            }
            else
            {
                Console.WriteLine("Пользователь не найден!");
            }
        }

        public int ShowPlace(string login, int score)
        {
            var user = users.FirstOrDefault(u => u.Login == login);
            if (user == null || user.Scores.Count == 0)
            {
                Console.WriteLine("Результаты пользователя не найдены.");
                return 0;
            }

            //Собираю все результаты в сортированный по возрастанию список
            var allScores = users.SelectMany(u => u.Scores).OrderBy(u => u).ToList();

            int place = allScores.IndexOf(score) + 1;
            return 100 * place / allScores.Count;
        }

        //Десериализация данных из файла
        private void LoadUsers()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            else
            {
                users = new List<User>();
            }
        }

        //Сериализация данных в JSON и запись в файл
        private void SaveUsers()
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
