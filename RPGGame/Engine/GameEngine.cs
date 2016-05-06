using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGGame.Engine
{
    using Interfaces;
    using Items;
    using Characters;
    using System.IO;
    using System.Reflection;
    using Attributes;
    using Exceptions;

    public class GameEngine
    {
        public const int mapWidth = 10;
        public const int mapHeight = 10;
        private const int InitalNumberOfEnemies = 10;
        private const int InitialNumberOfItems = 7;

        private static Random Rand = new Random();

        private readonly IInputReader reader;
        private readonly IRenderer renderer;

        private readonly string[] enemiesNames = 
        {
            "Hyptu",
            "Teshi",
            "Zulyafi",
            "Wanjin",
            "Maalik",
            "Vuzembi",
            "Tazingo"
        };

        private readonly IList<Character> enemies;
        private readonly IList<Item> items;

        private IPlayer player;

        public GameEngine(IInputReader reader, IRenderer renderer)
        {
            this.reader = reader;
            this.renderer = renderer;
            this.enemies = new List<Character>();
            this.items = new List<Item>();
        }

        public bool IsRunning { get; private set; }

        public void Run() 
        {
            this.IsRunning = true;

            string playerName = this.GetPlayerName();
            PlayerRace playerRace = this.GetPlayerRace();

            this.player = new Player(new Position(0, 0), 'P', playerName, playerRace);

            this.PopulateEnemies();
            this.PopulateItems();

            this.ExecuteHelpCommand();
            while (this.IsRunning)
            {                
                this.renderer.WriteLine(Environment.NewLine + "Enter command:");
                string command = this.reader.ReadLine();

                try
                {
                    this.ExecuteCommand(command);
                }
                catch (ObjectOutOfBoundsException exception)
                {
                    this.renderer.WriteLine(exception.Message);
                }
                catch (NotEnoughHealthPotionsException exception)
                {
                    this.renderer.WriteLine(exception.Message);
                }
                catch (Exception exception)
                {
                    this.renderer.WriteLine(exception.Message);
                }
            }
        }

        private void ExecuteCommand(string command)
        {
            switch (command)
            {
                case "help":
                    this.ExecuteHelpCommand();
                    break;
                case "map":
                    this.PrintMap();
                    break;
                case "move":
                    this.MovePlayer();                    
                    break;
                case "heal":
                    this.HealPlayer();
                    break;
                case "status":
                    this.ShowPlayerStatus();
                    break;
                case "enemies":
                    this.ShowEnemiesStatus();
                    break;
                case "clear":
                    this.renderer.Clear();
                    break;
                case "exit":
                    this.IsRunning = false;
                    this.renderer.WriteLine("Bye!");
                    break;
                default:
                    throw new ArgumentException("Unknown command!");
            }
        }

        private void ExecuteHelpCommand()
        {
            string helpCommandInfo = File.ReadAllText("../../HelpInfo.txt");

            Console.WriteLine(helpCommandInfo);
        }

        private string GetPlayerName()
        {
            this.renderer.WriteLine("Please enter player name:");

            string playerName = this.reader.ReadLine();
            while (string.IsNullOrWhiteSpace(playerName))
            {
                this.renderer.WriteLine("Player name cannot be empty! Please re-enter.");
                playerName = this.reader.ReadLine();
            }

            return playerName;
        }

        private PlayerRace GetPlayerRace()
        {
            this.renderer.WriteLine("Please choose player race (1, 2, 3 or 4):");
            this.renderer.WriteLine("1. Elf (damage: 200, health: 200)");
            this.renderer.WriteLine("2. Human (damage: 250, health: 150)");
            this.renderer.WriteLine("3. Dwarf (damage: 220, health: 250)");
            this.renderer.WriteLine("4. Alcoholic (damage: 170, health: 100)");

            string choice = this.reader.ReadLine();

            string[] validChoices = { "1", "2", "3", "4" };

            while (!validChoices.Contains(choice))
            {
                this.renderer.WriteLine("Invalid player race choice! Please re-enter.");
                choice = this.reader.ReadLine();
            }

            PlayerRace race = (PlayerRace)int.Parse(choice);

            return race;
        }

        private Character CreateEnemy()
        {
            int currentX = Rand.Next(1, mapWidth);
            int currentY = Rand.Next(1, mapHeight);

            bool containsEnemy = this.enemies
                .Any(e => e.Position.X == currentX && e.Position.Y == currentY);

            while (containsEnemy)
            {
                currentX = Rand.Next(1, mapWidth);
                currentY = Rand.Next(1, mapHeight);

                containsEnemy = this.enemies
                    .Any(e => e.Position.X == currentX && e.Position.Y == currentY);
            }

            int nameIndex = Rand.Next(0, enemiesNames.Length);
            string name = enemiesNames[nameIndex];

            // Using reflection to get all enemy types during program execution:
            var enemyTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass)
                .Where(t => t.CustomAttributes.Any(a => a.AttributeType == typeof(EnemyAttribute)))
                .ToArray();

            var currentEnemyType = enemyTypes[Rand.Next(0, enemyTypes.Length)];

            Character currentEnemy = Activator.CreateInstance(currentEnemyType, new Position(currentX, currentY), name) as Character;

            return currentEnemy;
        }

        private void PopulateEnemies()
        {
            for (int i = 0; i < InitalNumberOfEnemies; i++)
            {
                Character enemy = this.CreateEnemy();
                this.enemies.Add(enemy);
            }
        }

        private Item CreateHealthPotion()
        {
            int currentX = Rand.Next(1, mapWidth);
            int currentY = Rand.Next(1, mapHeight);

            bool containsEnemy = this.enemies
                .Any(e => e.Position.X == currentX && e.Position.Y == currentY);

            bool containsItem = this.items
                .Any(i => i.Position.X == currentX && i.Position.Y == currentY);

            while (containsEnemy || containsItem)
            {
                currentX = Rand.Next(1, mapWidth);
                currentY = Rand.Next(1, mapHeight);

                containsEnemy = this.enemies
                    .Any(e => e.Position.X == currentX && e.Position.Y == currentY);

                containsItem = this.items
                    .Any(i => i.Position.X == currentX && i.Position.Y == currentY);
            }

            int healthPotionType = Rand.Next(0, 3);

            HealthPotionSize healthPotionSize;

            switch (healthPotionType)
	        {
                case 0:
                    healthPotionSize = HealthPotionSize.Small;
                    break;
                case 1:
                    healthPotionSize = HealthPotionSize.Medium;
                    break;
                case 2:
                    healthPotionSize = HealthPotionSize.Large;
                    break;
		        default:
                    throw new ArgumentException("Wrong health potions size!");
	        }

            return new HealthPotion(new Position(currentX, currentY), healthPotionSize);            
        }

        private void PopulateItems()
        {
            for (int i = 0; i < InitialNumberOfItems; i++)
            {
                Item item = this.CreateHealthPotion();
                this.items.Add(item);
            }
        }

        private void PrintMap()
        {
            StringBuilder sb = new StringBuilder();

            for (int row = 0; row < mapHeight; row++)
            {
                for (int col = 0; col < mapWidth; col++)
                {
                    if (this.player.Position.X == col && this.player.Position.Y == row)
                    {
                        sb.Append('P');
                        continue;
                    }

                    Character enemy = this.enemies.FirstOrDefault(e => e.Position.X == col && e.Position.Y == row);

                    Item item = this.items.FirstOrDefault(i => i.Position.X == col && i.Position.Y == row);

                    if (enemy != null && enemy.Health > 0)
                    {
                        sb.Append(enemy.ObjectSymbol);
                    }
                    else if (item != null && item.ItemState == ItemState.Available)
                    {
                        sb.Append(item.ObjectSymbol);
                    }
                    else
                    {
                        sb.Append('.');
                    }
                }

                sb.AppendLine();
            }

            this.renderer.WriteLine(sb.ToString());
        }

        private void MovePlayer()
        {
            this.renderer.WriteLine(String.Empty);
            this.renderer.WriteLine(this.player.ToString());
            this.renderer.WriteLine(String.Empty);
            this.PrintMap();
            string moveCommand = String.Empty;

            ConsoleKeyInfo userInput = Console.ReadKey();
            bool isArrowKeyPressed = userInput.Key == ConsoleKey.RightArrow ||
                                        userInput.Key == ConsoleKey.LeftArrow ||
                                            userInput.Key == ConsoleKey.DownArrow ||
                                                userInput.Key == ConsoleKey.UpArrow;
            while(userInput.Key != ConsoleKey.Escape && isArrowKeyPressed)
            {
                if (userInput.Key == ConsoleKey.RightArrow)
                {
                    moveCommand = "right";
                }
                else if (userInput.Key == ConsoleKey.LeftArrow)
                {
                    moveCommand = "left";
                }
                else if (userInput.Key == ConsoleKey.DownArrow)
                {
                    moveCommand = "down";
                }
                else if (userInput.Key == ConsoleKey.UpArrow)
                {
                    moveCommand = "up";
                }

                this.player.Move(moveCommand);

                Character enemy = this.enemies.FirstOrDefault(
                    e =>
                        e.Position.X == this.player.Position.X
                        && e.Position.Y == this.player.Position.Y
                        && e.Health > 0
                        );

                if (enemy != null)
                {
                    // Battle
                    this.EnterBattle(enemy);
                    if (this.IsRunning == false)
                    {
                        // Player died. Break the arrow keys movement loop.
                        break;
                    }
                }

                Item item = this.items.FirstOrDefault(
                    i => 
                        i.Position.X == this.player.Position.X 
                        && i.Position.Y == this.player.Position.Y
                        && i.ItemState == ItemState.Available
                        );

                if (item != null)
                {
                    if (item is HealthPotion)
                    {
                        HealthPotionSize currentHealthPotionSize = this.player.AddHealthPotionToInventory(item);
                        item.ItemState = ItemState.Collected;
                        this.renderer.WriteLine("{0} health potion added to inventory!", currentHealthPotionSize);                   
                    }
                }

                this.renderer.WriteLine(String.Empty);
                this.renderer.WriteLine(this.player.ToString());
                this.renderer.WriteLine(String.Empty);
                this.PrintMap();
                userInput = Console.ReadKey();
                isArrowKeyPressed = userInput.Key == ConsoleKey.RightArrow ||
                                        userInput.Key == ConsoleKey.LeftArrow ||
                                            userInput.Key == ConsoleKey.DownArrow ||
                                                userInput.Key == ConsoleKey.UpArrow;
            }
        }

        private void ShowPlayerStatus()
        {
            this.renderer.WriteLine(this.player.GetPlayerCurrentStatus());
        }

        private void EnterBattle(ICharacter enemy)
        {
            this.player.Attack(enemy);

            if (enemy.Health <= 0)
            {
                this.renderer.WriteLine("Enemy killed!");
                this.player.GainExperience(enemy);

                this.enemies.Remove(enemy as Character);

                if (enemies.Count == 0)
                {
                    this.IsRunning = false;
                    this.renderer.WriteLine(String.Empty);
                    this.renderer.WriteLine("Victory! All enemies are dead.");
                    this.renderer.WriteLine("Press any key to quit!");
                    Console.ReadKey();
                    this.renderer.WriteLine(String.Empty);
                }
                return;
            }
            else
            {
                enemy.Attack(this.player);
            }

            if (player.Health > 0)
            {
                this.renderer.WriteLine("Enemy not killed! You must attack again.");
            }
            else
            {
                this.IsRunning = false;
                this.renderer.WriteLine("You died !");
                this.renderer.WriteLine("Press any key to quit!");
                Console.ReadKey();
            }
        }

        private void ShowEnemiesStatus()
        {
            List<Character> fairies = new List<Character>();
            List<Character> ninjas = new List<Character>();
            List<Character> orks = new List<Character>();
            foreach (var enemy in enemies)
            {
                if (enemy is Fairy)
                {
                    fairies.Add(enemy);
                }
                else if (enemy is Ninja)
                {
                    ninjas.Add(enemy);
                }
                else
                {
                    orks.Add(enemy);
                }
            }

            this.renderer.WriteLine(String.Empty);
            this.renderer.WriteLine("Fairies:");
            foreach (var fairy in fairies)
            {
                this.renderer.WriteLine("Position: Col = {0}, Row = {1} Name: {2} Health: {3} Damage: {4}",
                    fairy.Position.X,
                    fairy.Position.Y,
                    fairy.Name,
                    fairy.Health,
                    fairy.Damage
                    );
            }
            this.renderer.WriteLine(String.Empty);
            this.renderer.WriteLine("Ninjas:");
            foreach (var ninja in ninjas)
            {
                this.renderer.WriteLine("Position: Col = {0}, Row = {1} Name: {2} Health: {3} Damage: {4}",
                    ninja.Position.X,
                    ninja.Position.Y,
                    ninja.Name,
                    ninja.Health,
                    ninja.Damage
                    );
            }
            this.renderer.WriteLine(String.Empty);
            this.renderer.WriteLine("Orks:");
            foreach (var ork in orks)
            {
                this.renderer.WriteLine("Position: Col = {0}, Row = {1} Name: {2} Health: {3} Damage: {4}",
                    ork.Position.X,
                    ork.Position.Y,
                    ork.Name,
                    ork.Health,
                    ork.Damage
                    );
            }
            this.renderer.WriteLine(String.Empty);
            this.renderer.WriteLine("{0} enemies left.", enemies.Count);
        }

        private void HealPlayer()
        {
            this.renderer.WriteLine("What type of potion do you want to use ? (small, medium or large) :");
            string potionType = this.reader.ReadLine();
            this.player.Heal(potionType);
        }
    }
}
