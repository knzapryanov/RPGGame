using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGGame.UI
{
    using Interfaces;

    public class ConsoleInputReader : IInputReader
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
