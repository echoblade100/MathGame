namespace MathGame;

public class Game
{
    private int[][][] _gameLevels;
    private int _currentLevel;
    private const int MAX_LEVEL_OF_DIFFUCULTY = 5;
    private readonly Random _rnd = new Random();
    private readonly List<Stat> _stats = new List<Stat>();

    public Game()
    {
        this._currentLevel = 0;
    }

    public void Run()
    {
        this.PrepareGameData(5, 10);
        this.DisplayWelcomeMessage();
        this.DisplayMenu();
        this.HotkeyHandler();
    }

    private void DisplayWelcomeMessage()
    {
        Console.WriteLine($"Welcome to the math game!");
    }
    
    private void DisplayEndGameMessage()
    {
        Console.WriteLine($"Current game was end. Press |m| for display menu.");
    }

    private void DisplayMenu()
    {
        Console.WriteLine($"Menu:{Environment.NewLine}" +
                          $"+ | addition game{Environment.NewLine}" +
                          $"- | subtraction game{Environment.NewLine}" +
                          $"* | multiplication game{Environment.NewLine}" +
                          $"/ | division game{Environment.NewLine}" +
                          $"d | change level of difficulty{Environment.NewLine}" +
                          $"s | display stats{Environment.NewLine}" +
                          $"m | display menu{Environment.NewLine}" +
                          $"q | quite game{Environment.NewLine}");
    }

    private void HotkeyHandler()
    {
        bool quite = false;
        do
        {
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();
            switch (key.KeyChar)
            {
                case '+':
                    Console.WriteLine($"Start addition game!");
                    this.StartAdditionGame();
                    break;
                case '-':
                    Console.WriteLine($"Start subtraction game!");
                    this.StartSubtractionGame();
                    break;
                case '*':
                    Console.WriteLine($"Start multiplication game!");
                    this.StartMultiplicationGame();
                    break;
                case '/':
                    Console.WriteLine($"Start division game!");
                    this.StartDivisionGame();
                    break;
                case 'd':
                    this.DisplayCurrentLevelOfDifficulty();
                    this.ChangeLevelOfDifficulty();
                    break;
                case 's':
                    Console.WriteLine($"Your session game stats!");
                    this.DisplayStats();
                    break;
                case 'm':
                    this.DisplayMenu();
                    break;
                case 'q':
                    Console.WriteLine($"Bye, bye!");
                    quite = true;
                    break;
                default:
                    Console.WriteLine($"Wrong command! Try again enter hotkey! Display menu: m");
                    break;
            }
        } while (!quite);
    }

    private int InputNumberHandler()
    {
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int number))
            {
                return number;
            }
            Console.WriteLine("Need input a number!");
        }
    }   

    private void StartAdditionGame()
    {
        int[][] pairs = this.SelectRandomPairsForGame();
        this.StartSaveStats("Addition", pairs.Length);
        for (int i = 0; i < pairs.Length; i++)
        {
            this.DisplayGameData(i + 1, pairs.Length, pairs[i][0], pairs[i][1], "+");
            int answer = this.InputNumberHandler();
            bool answerResult = answer == pairs[i][0] + pairs[i][1];
            
            this.SaveCorrectAnswer(answerResult);
            this.DisplayReplyState(answerResult);
        }

        this.DisplayEndGameMessage();
    }
    
    private void StartSubtractionGame()
    {
        int[][] pairs = this.SelectRandomPairsForGame();
        this.StartSaveStats("Subtraction", pairs.Length);
        for (int i = 0; i < pairs.Length; i++)
        {
            this.DisplayGameData(i + 1, pairs.Length, pairs[i][0] + pairs[i][1], pairs[i][1], "-");
            int answer = this.InputNumberHandler();
            bool answerResult = answer == pairs[i][0];

            this.SaveCorrectAnswer(answerResult);
            this.DisplayReplyState(answerResult);
        }

        this.DisplayEndGameMessage();
    }
    
    private void StartMultiplicationGame()
    {
        int[][] pairs = this.SelectRandomPairsForGame();
        this.StartSaveStats("Multiplication", pairs.Length);
        for (int i = 0; i < pairs.Length; i++)
        {
            this.DisplayGameData(i + 1, pairs.Length, pairs[i][0], pairs[i][1], "*");
            int answer = this.InputNumberHandler();
            bool answerResult = answer == pairs[i][0] * pairs[i][1];

            this.SaveCorrectAnswer(answerResult);
            this.DisplayReplyState(answerResult);
        }

        this.DisplayEndGameMessage();
    }
    
    private void StartDivisionGame()
    {
        int[][] pairs = this.SelectRandomPairsForGame();
        this.StartSaveStats("Division", pairs.Length);
        for (int i = 0; i < pairs.Length; i++)
        {
            this.DisplayGameData(i + 1, pairs.Length, pairs[i][0] * pairs[i][1], pairs[i][1], "/");
            int answer = this.InputNumberHandler();
            
            bool answerResult = answer == pairs[i][0];
            this.SaveCorrectAnswer(answerResult);
            this.DisplayReplyState(answerResult);
        }

        this.DisplayEndGameMessage();
    }

    private void StartSaveStats(string gameName, int totalQuestions)
    {
        this._stats.Add(new Stat() { GameName = gameName, TotalQuestions = totalQuestions, CorrectAnswers = 0});
    }
    
    private void SaveCorrectAnswer(bool answer)
    {
        if (answer)
        {
            this._stats[^1].CorrectAnswers++;
        }
    }

    private void ChangeLevelOfDifficulty()
    {
        int level = 0;
        do
        {
            Console.Write($"Enter level 1-{MAX_LEVEL_OF_DIFFUCULTY}: ");
            level = InputNumberHandler() - 1;
        } while (level < 0 || level >= MAX_LEVEL_OF_DIFFUCULTY);

        this._currentLevel = level;
        this.DisplayCurrentLevelOfDifficulty();
    }
    
    private void DisplayCurrentLevelOfDifficulty()
    {
        Console.WriteLine($"Current level of difficulty: {this._currentLevel + 1}");
    }

    private void DisplayStats()
    {
        foreach (var stat in this._stats)
        {
            Console.WriteLine($"Game: {stat.GameName}, answers: {stat.CorrectAnswers}/{stat.TotalQuestions}");
        }
    }

    private int[][] SelectRandomPairsForGame(int pairsCount = 10)
    {
        int[][] randomPairs = new int[pairsCount][];
        int[] randomIndexes = new int[pairsCount];

        for (int i = 0; i < pairsCount; i++)
        {
            int rndIndex = this._rnd.Next(0, _gameLevels[_currentLevel].Length);
            if (!randomIndexes.Contains(rndIndex))
            {
                randomIndexes[i] = rndIndex;
                randomPairs[i] = _gameLevels[_currentLevel][rndIndex];
            }
            else
            {
                i--;
            }
        }

        return randomPairs;
    }

    private void DisplayGameData(int currStage, int maxStage, int first, int second, string operation)
    {
        Console.Write($"Stage:{currStage}/{maxStage}| {first} {operation} {second} = ");
    }

    private void DisplayReplyState(bool answer)
    {
        if (answer)
        {
            Console.WriteLine("Correct!");
        }
        else
        {
            Console.WriteLine("Wrong!");
        }
    }

    private void PrepareGameData(int levels, int deltaNumberForGen)
    {
        _gameLevels = new int[levels][][];
        for (int level = 0; level < levels; level++)
        {
            _gameLevels[level] = new int[deltaNumberForGen * deltaNumberForGen][];
            int upperLimit = (level + 1) * deltaNumberForGen;
            int lowerLimit = upperLimit - deltaNumberForGen + 1;
            int pairsCounter = 0;
            
            for (int first = lowerLimit; first <= upperLimit; first++)
            {
                for (int second = lowerLimit; second <= upperLimit; second++)
                {
                    _gameLevels[level][pairsCounter] = new[] { first, second };
                    pairsCounter++;
                }
            }
        }
    }
}