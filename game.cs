using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class Die
{
    private static readonly Random random = new Random();
    public int CurrentValue { get; private set; }

    public int Roll()
    {
        CurrentValue = random.Next(1, 7); // Random number between 1 and 6 for dice
        return CurrentValue;
    }
}

public abstract class Game
{
    protected Statistics Stats;

    protected Game(Statistics stats)
    {
        Stats = stats;
    }

    public abstract void Play();
}

public class SevensOut : Game
{
    private Die die = new Die();

    public SevensOut(Statistics stats) : base(stats) { }

    public override void Play()
    {
        int total = 0;
        Console.WriteLine("Playing Sevens Out:");
        while (true)
        {
            int roll = die.Roll() + die.Roll();
            Debug.Assert(roll >= 2 && roll <= 12, "Invalid roll total");
            total += roll;
            Console.WriteLine($"You rolled: {roll}. Total score: {total}");
            if (roll == 7)
            {
                Console.WriteLine("Sevens out!");
                break;
            }
        }
        Stats.RecordGame("SevensOut", total);
    }
}

public class ThreeOrMore : Game
{
    private Die[] dice = new Die[5];

    public ThreeOrMore(Statistics stats) : base(stats)
    {
        for (int i = 0; i < dice.Length; i++)
        {
            dice[i] = new Die();
        }
    }

    public override void Play()
    {
        int total = 0;
        Console.WriteLine("Playing Three or More:");
        while (total < 20)
        {
            total = 0;
            Console.Write("You rolled: ");
            foreach (var die in dice)
            {
                int roll = die.Roll();
                Console.Write($"{roll} ");
                total += roll;
            }
            Console.WriteLine($". Total score: {total}");
            Debug.Assert(total >= 5 && total <= 30, "Invalid total score");
        }
        Console.WriteLine("You've reached 20 points!");
        Stats.RecordGame("ThreeOrMore", total);
    }
}

public class Statistics
{
    private Dictionary<string, List<int>> GameStats = new Dictionary<string, List<int>>();

    public void RecordGame(string gameName, int score)
    {
        if (!GameStats.ContainsKey(gameName))
        {
            GameStats[gameName] = new List<int>();
        }
        GameStats[gameName].Add(score);
    }

    public void DisplayStats()
    {
        foreach (var game in GameStats)
        {
            Console.WriteLine($"Game: {game.Key}, Games Played: {game.Value.Count}, High Score: {game.Value.Max()}");
        }
    }
}

public class Testing
{
    public static void PerformTests()
    {
        Die die = new Die();
        int roll = die.Roll();
        Debug.Assert(roll >= 1 && roll <= 6, "Die roll out of bounds");

        SevensOut sevensOut = new SevensOut(new Statistics());
        sevensOut.Play();

        ThreeOrMore threeOrMore = new ThreeOrMore(new Statistics());
        threeOrMore.Play();

        Console.WriteLine("Tests completed successfully.");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Statistics stats = new Statistics();

        while (true)
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1: Play Sevens Out");
            Console.WriteLine("2: Play Three Or More");
            Console.WriteLine("3: View Statistics");
            Console.WriteLine("4: Perform Tests");
            Console.WriteLine("5: Exit");
            Console.Write("Enter your choice: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Game sevensOut = new SevensOut(stats);
                    sevensOut.Play();
                    break;
                case "2":
                    Game threeOrMore = new ThreeOrMore(stats);
                    threeOrMore.Play();
                    break;
                case "3":
                    stats.DisplayStats();
                    break;
                case "4":
                    Testing.PerformTests();
                    break;
                case "5":
                    Console.WriteLine("Exiting the program.");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 5.");
                    break;
            }
        }
    }
}
