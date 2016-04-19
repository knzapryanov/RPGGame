using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGGame.UI
{
    using Interfaces;

    public class ConsoleRenderer : IRenderer
    {
        public void WriteLine(string message, params object[] parameters)
        {
            Console.WriteLine(message, parameters);
        }

        public void Clear()
        {
            Console.Clear();
        }
    }
}
