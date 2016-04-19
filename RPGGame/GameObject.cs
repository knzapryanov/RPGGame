using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGGame
{
    using Exceptions;
    using Engine;

    public abstract class GameObject
    {
        private Position position;
        private char objectSymbol;

        protected GameObject(Position position, char objectSymbol)
        {
            this.Position = position;
            this.ObjectSymbol = objectSymbol;
        }

        public Position Position 
        {
            get
            {
                return this.position;
            }
            set 
            {
                if (value.X < 0 || value.Y < 0 || value.X > GameEngine.mapWidth - 1 || value.Y > GameEngine.mapHeight - 1)
                {
                    throw new ObjectOutOfBoundsException("Object coordinates are out of the map range.");
                }

                this.position = value;
            }
        }

        public char ObjectSymbol 
        {
            get
            {
                return this.objectSymbol;
            }
            set
            {
                if (!char.IsUpper(value))
                {
                    throw new ArgumentOutOfRangeException("Object symbol must be an upper-case letter.");
                }

                this.objectSymbol = value;
            }
        }
    }
}
