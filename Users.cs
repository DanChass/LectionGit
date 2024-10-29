using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otladka
{
    [Serializable]
    internal class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public List<int> Scores { get; set; } = new List<int>();
    }
}
