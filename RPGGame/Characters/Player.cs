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
        private const int PlayerDefaultDamage = 100;
        private const int PlayerDefaultHealth = 500;
        private int experience;
        private int levelUpCap;
        private int level;
        private int levelUpCapIncreaseStep;
        private int playerMaximumHealth;
        private int damage;

        private List<Item> inventory;

        public Player(Position position, char playerSymbol, string name, PlayerRace race)
            : base(position, playerSymbol, name, PlayerDefaultDamage, PlayerDefaultHealth)
        {
            this.Race = race;
            this.inventory = new List<Item>();
            this.Experiance = 0;
            this.LevelUpCap = 200;
            this.Level = 0;
            this.LevelUpCapIncreaseStep = 50;
            this.PlayerMaximumHealth = this.GetPlayerMaximumHealth();
            this.SetPlayerStats();
        }

        public PlayerRace Race
        {
            get;
            set;
        }

        public IEnumerable<Item> Inventory
        {
            get { return this.inventory; }
        }

        public int Damage 
        {
            get
            {
                if (this.HaveSword())
                {
                    Sword equippedSword = this.GetTheSwordFromInventory();
                    return this.damage + equippedSword.SwordDamage;
                }
                else
                {
                    return this.damage;
                }                
            }
            set
            {
                this.damage = value;
            }
        }

        public int Experiance
        {
            get { return this.experience; }
            set { this.experience = value; }
        }

        public int LevelUpCap { get; set; }

        public int Level { get; set; }

        public int LevelUpCapIncreaseStep { get; set; }

        public int PlayerMaximumHealth { get; set; }

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

        public HealthPotionSize AddHealthPotionToInventory(Items.Item item)
        {
            HealthPotion currentHealthPotion = item as HealthPotion;
            this.inventory.Add(currentHealthPotion);
            return currentHealthPotion.HealthPotionSize;
        }

        public string AddSwordToInventory(Item item)
        {
            Sword currentSword = item as Sword;
            this.inventory.Add(currentSword);
            return currentSword.SwordName;
        }

        public bool HaveSword()
        {
            bool playerHaveSword = this.inventory.Exists(item => item is Sword);
            return playerHaveSword;
        }

        private Sword GetTheSwordFromInventory()
        {
            Sword swordToReturn = this.inventory.Find(item => item is Sword) as Sword;
            return swordToReturn;
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

            int maxPlayerHealth = this.PlayerMaximumHealth; //this.GetPlayerMaximumHealth();

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

        public override string ToString()
        {
            string basicPlayerInfo = string.Format("Player {0} ({1}): Damage ({2}), Health ({3}), Maximum health ({4}), Experiance ({5}), Experience needed for next level: ({6})",
                    this.Name,
                    this.Race,
                    this.Damage,
                    this.Health,
                    this.PlayerMaximumHealth,
                    this.Experiance,
                    this.LevelUpCap
                    );

            if (this.HaveSword())
            {
                Sword playerEquippedSword = this.GetTheSwordFromInventory();
                return basicPlayerInfo + System.Environment.NewLine + string.Format("Sword info: Name ({0}), Damage ({1}), Durability ({2})",
                    playerEquippedSword.SwordName,
                    playerEquippedSword.SwordDamage,
                    playerEquippedSword.SwordDurability
                    );
            }
            else
            {
                return basicPlayerInfo;
            }
        }

        public string GetPlayerPotionsInfo()
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

            return string.Format("Health potions: ({0} Large, {1} Medium, {2} Small)",
                largePotionsCount,
                mediumPotionsCount,
                smallPotionsCount
                );
        }

        public override void Attack(ICharacter enemy)
        {
            enemy.Health -= this.Damage;

            if (this.HaveSword())
            {
                Sword playerCurrentSword = this.GetTheSwordFromInventory();
                playerCurrentSword.SwordDurability--;

                if (playerCurrentSword.SwordDurability <= 0)
                {
                    this.inventory.Remove(playerCurrentSword);
                }
            }
        }

        public void GainExperience(ICharacter enemy)
        {
            if (enemy is Fairy)
            {
                this.Experiance += 100;
            }
            else if (enemy is Ninja)
            {
                this.Experiance += 200;
            }
            else
            {
                this.Experiance += 150;
            }

            if (this.Experiance >= this.LevelUpCap)
            {
                this.LevelUp();
            }
        }

        public void LevelUp()
        {
            string playerStatsBoostChoice = String.Empty;

            Console.WriteLine();
            Console.WriteLine("Level Up !!!");
            Console.WriteLine("You are level {0} !", this.Level + 1);
            Console.WriteLine("Choose what do you want to increase: 1 for Damage, 2 for Health :");
            playerStatsBoostChoice = Console.ReadLine();

            switch (playerStatsBoostChoice)
            {
                case "1":
                    this.damage += 30;
                    break;
                case "2":
                    this.PlayerMaximumHealth += 30;
                    break;
                default:
                    Console.WriteLine("Invalid choice!");
                    this.LevelUp();
                    break;
            }

            this.Level++;
            this.LevelUpCap = (this.LevelUpCap * 2) + this.LevelUpCapIncreaseStep;
            this.LevelUpCapIncreaseStep += 50;
        }
    }
}
