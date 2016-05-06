using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGGame.Characters
{
    using Interfaces;

    public abstract class Character : GameObject, ICharacter
    {
        private string name;

        public Character(Position position, char characterSymbol, string name, int damage, int health)
            :base(position, characterSymbol)
        {
            this.Name = name;
            this.Damage = damage;
            this.Health = health;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Player name cannot be empty, null or whitespace!");
                }

                this.name = value;
            }
        }

        public int Damage
        {
            get;
            set;
        }

        public int Health
        {
            get;
            set;
        }

        public virtual void Attack(ICharacter enemy)
        {
            enemy.Health -= this.Damage;
        }
    }
}
