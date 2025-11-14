namespace BattleshipCS;
public class GameManager
{
    private readonly Player player1;
    private readonly Player player2;
    private Player currentPlayer;
    private bool gameOver;
    private readonly UserInterface userInterface;

    public GameManager(int boardSize)
    {
        gameOver = false;
        userInterface = new UserInterface(this);

        player1 = new HumanPlayer("Игрок 1", boardSize);
        player2 = new AIPlayer("Компьютер", boardSize);

        currentPlayer = player1;

        // Устанавливаем ссылки на поля противников
        player1.SetEnemyBoard(player2.MyBoard);
        player2.SetEnemyBoard(player1.MyBoard);
    }

    public void SetupGame()
    {
        Console.WriteLine($"Расстановка кораблей для {player1.Name}:");
        player1.PlaceShips();

        Console.WriteLine($"Расстановка кораблей для {player2.Name}:");
        player2.PlaceShips();

        Console.WriteLine("Игра начинается!");
    }

    public void RunGameLoop()
    {
        while (!gameOver)
        {
            var aiPlayer = currentPlayer as AIPlayer;

            // Показываем состояние после хода
            if (aiPlayer == null)
            {
                DisplayGameState();
            }

            // Ход текущего игрока
            var move = currentPlayer.MakeMove();

            // Обработка выстрела
            var enemyBoard = currentPlayer.EnemyBoard;
            var result = enemyBoard!.ReceiveShot(move);

            // Обновление состояния ИИ если нужно
            if (aiPlayer != null)
            {
                aiPlayer.UpdateAIState(result, move);
            }

            // Отображение результата
            Console.WriteLine($"{currentPlayer.Name} стреляет в ({move.Item1}, {move.Item2}) - {GetResultMessage(result)}");

            // Показываем состояние после хода
            if (aiPlayer == null)
            {
                DisplayGameState();
            }

            // Проверка окончания игры
            if (enemyBoard.IsAllShipsSunk())
            {
                gameOver = true;
                Console.WriteLine("\n=== ИГРА ОКОНЧЕНА ===");
                Console.WriteLine($"{currentPlayer.Name} ПОБЕДИЛ!");
                userInterface.ShowGameOver(currentPlayer.Name);
                break;
            }

            // Смена хода если не попадание
            if (result != Ship.ShotResult.Hit && result != Ship.ShotResult.Sunk)
            {
                SwitchTurn();
            }

            // Пауза для удобства восприятия
            if (aiPlayer == null)
            {
                Console.WriteLine("Нажмите Enter для продолжения...");
                Console.ReadLine();
            }
        }
    }

    private string GetResultMessage(Ship.ShotResult result)
    {
        return result switch
        {
            Ship.ShotResult.Hit => "ПОПАДАНИЕ!",
            Ship.ShotResult.Sunk => "КОРАБЛЬ ПОТОПЛЕН!",
            Ship.ShotResult.Miss => "ПРОМАХ!",
            Ship.ShotResult.AlreadyShot => "Уже стреляли сюда!",
            _ => "Неизвестный результат"
        };
    }

    private void SwitchTurn()
    {
        currentPlayer = currentPlayer == player1 ? player2 : player1;
    }

    private void DisplayGameState()
    {
        Console.WriteLine();
        userInterface.DisplayBoards(currentPlayer);
    }

    public Player CurrentPlayer => currentPlayer;
    public Player Player1 => player1;
    public Player Player2 => player2;
}