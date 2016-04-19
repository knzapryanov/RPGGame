using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGGame
{
    using Engine;
    using Interfaces;
    using UI;

    class RPGGame
    {
        static void Main(string[] args)
        {            
            IInputReader reader = new ConsoleInputReader();
            IRenderer renderer = new ConsoleRenderer();

            GameEngine gameEngine = new GameEngine(reader, renderer);

            gameEngine.Run();
        }
    }
}
