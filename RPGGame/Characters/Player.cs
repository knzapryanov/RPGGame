using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGGame.Characters
{
    using Interfaces;
    using Items;
    using Exceptions;

    class Player : Character, IPlayer
    {
        private const int PlayerDamage = 100;
        private const int PlayerHealth = 500;

        private List<Item> inventory;

        public Player(Position position, char playerSymbol, string name, PlayerRace race)
            : base(position, playerSymbol, name, PlayerDamage, PlayerHealth)
        {
            this.Race = race;
            this.inventory = new List<Item>();
            this.SetPlayerStats();
        }

        private void SetPlayerStats()
        {
            switch (this.Race)
            {
                case PlayerRace.Elf:
                    this.Damage = 200;
                    this.Health = 200;
                    break;
                case PlayerRace.Human:
                    this.Damage = 250;
                    this.Health = 150;
                    break;
                case PlayerRace.Dwarf:
                    this.Damage = 220;
                    this.Health = 250;
                    break;
                case PlayerRace.Alcoholic:
                    this.Damage = 170;
                    this.Health = 130;
                    break;
                default:
                    throw new ArgumentException("Unknown player race!");
            }
        }

        public PlayerRace Race { get; set; }

        public void Move(string direction)
        {
            switch (direction)
            {
                case "up":
                    this.Position = new Position(this.Position.X, this.Position.Y - 1);
                    break;
                case "right":
                    this.Position = new Position(this.Position.X + 1, this.Position.Y);
                    break;
                case "down":
                    this.Position = new Position(this.Position.X, this.Position.Y + 1);
                    break;
                case "left":
                    this.Position = new Position(this.Position.X - 1, this.Position.Y);
                    break;
                default:
                    throw new ArgumentException("Wrong direction!");
            }
        }

        public IEnumerable<Item> Inventory
        {
            get { return this.inventory; }
        }

        public HealthPotionSize AddHealthPotionToInventory(Items.Item item)
        {
            HealthPotion currentHealthPotion = item as HealthPotion;
            this.inventory.Add(currentHealthPotion);
            return currentHealthPotion.HealthPotionSize;
        }

        public void Heal(string potionType)
        {
            // Get from inventory health potion which have size specified by the player
            var healthPotion = this.inventory.FirstOrDefault(
                item => item is HealthPotion &&
                    (item as HealthPotion).HealthPotionSize == (HealthPotionSize)System.Enum.Parse(typeof(HealthPotionSize), potionType, true)
                    ) as HealthPotion;            
            
            if (healthPotion == null)
            {
                throw new NotEnoughHealthPotionsException("Not enough health potions!");
            }

            int maxPlayerHealth = this.GetPlayerMaximumHealth();

            int currentPlayerHealth = this.Health;
            if ((currentPlayerHealth += healthPotion.HealthRestoreAmount) >= maxPlayerHealth)
            {
                Console.WriteLine("You healed {0} points!", maxPlayerHealth - this.Health);
                this.Health = maxPlayerHealth;                
            }
            else
            {
                Console.WriteLine("You healed {0} points!", healthPotion.HealthRestoreAmount);
                this.Health += healthPotion.HealthRestoreAmount;                
            }            
                        
            this.inventory.Remove(healthPotion);
        }

        public int GetPlayerMaximumHealth()
        {
            int maxPlayerHealth = 0;
            switch (this.Race)
            {
                case PlayerRace.Elf:
                    maxPlayerHealth = 200;
                    break;
                case PlayerRace.Human:
                    maxPlayerHealth = 150;
                    break;
                case PlayerRace.Dwarf:
                    maxPlayerHealth = 250;
                    break;
                case PlayerRace.Alcoholic:
                    maxPlayerHealth = 130;
                    break;
                default:
                    break;
            }

            return maxPlayerHealth;
        }

        public int Experiance
        {
            get { throw new NotImplementedException(); }
        }

        public void LevelUp()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return string.Format("Player {0} ({1}): Damage ({2}), Health ({3}), Number of health potions ({4})",
                this.Name,
                this.Race,
                this.Damage,
                this.Health,
                this.Inventory.Count()
                );
        }

        public string GetPlayerCurrentStatus()
        {
            int smallPotionsCount = 0;
            int mediumPotionsCount = 0;
            int largePotionsCount = 0;

            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i] is HealthPotion)
                {
                    HealthPotion currentHealthPotion = inventory[i] as HealthPotion;
                    switch (currentHealthPotion.HealthPotionSize)
                    {
                        case HealthPotionSize.Small:
                            smallPotionsCount++;
                            break;
                        case HealthPotionSize.Medium:
                            mediumPotionsCount++;
                            break;
                        case HealthPotionSize.Large:
                            largePotionsCount++;
                            break;
                        default:
                            break;
                    }
                }
            }

            return string.Format("Player {0} ({1}): Damage ({2}), Health ({3}), Health potions: ({4} Large, {5} Medium, {6} Small)",
                this.Name,
                this.Race,
                this.Damage,
                this.Health,
                largePotionsCount,
                mediumPotionsCount,
                smallPotionsCount
                );
        }
    }
}
