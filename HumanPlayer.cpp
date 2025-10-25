#include "HumanPlayer.hpp"
#include <random>
#include <algorithm>

HumanPlayer::HumanPlayer(std::string name, int boardSize)
	: Player(name, boardSize)
{
}

void HumanPlayer::PlaceShips()
{
	std::cout << m_name << ", выберите способ расстановки кораблей:\n";
	std::cout << "1 - Ручная расстановка\n";
	std::cout << "2 - Автоматическая расстановка\n";

	int choice;
	while (true)
	{
		std::cout << "Ваш выбор (1 или 2): ";
		std::cin >> choice;

		if (choice == 1 || choice == 2)
		{
			break;
		}
		std::cout << "Неверный выбор. Попробуйте снова.\n";
	}

	if (choice == 1)
	{
		ManualPlacement();
	}
	else
	{
		AutomaticPlacement();
	}
}

void HumanPlayer::ManualPlacement()
{
	std::cout << "\n=== РУЧНАЯ РАССТАНОВКА КОРАБЛЕЙ ===\n";

	// Стандартные размеры кораблей для морского боя
	std::vector<int> shipSizes = { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };

	for (int size : shipSizes)
	{
		bool placed = false;
		while (!placed)
		{
			DisplayBoardState();
			std::cout << "Разместите корабль размером " << size << "\n";

			std::cout << "Введите начальную координату (ряд столбец, 0-" << m_myBoard.GetSize() - 1 << "): ";
			int row, col;
			std::cin >> row >> col;

			std::cout << "Ориентация (0 - горизонтально, 1 - вертикально): ";
			int orientation;
			std::cin >> orientation;

			placed = TryPlaceShip(size, row, col, orientation == 0);

			if (!placed)
			{
				std::cout << "Невозможно разместить корабль здесь. Попробуйте снова.\n";
			}
		}
	}

	std::cout << "Все корабли успешно расставлены!\n";
	DisplayBoardState();
}

void HumanPlayer::AutomaticPlacement()
{
	std::cout << "\n=== АВТОМАТИЧЕСКАЯ РАССТАНОВКА КОРАБЛЕЙ ===\n";

	std::vector<int> shipSizes = { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };

	std::random_device rd;
	std::mt19937 gen(rd());

	for (int size : shipSizes)
	{
		bool placed = false;
		int attempts = 0;

		while (!placed && attempts < 1000) // Ограничение на попытки
		{
			int row = gen() % m_myBoard.GetSize();
			int col = gen() % m_myBoard.GetSize();
			bool horizontal = gen() % 2 == 0;

			placed = TryPlaceShip(size, row, col, horizontal);
			attempts++;
		}

		if (!placed)
		{
			std::cout << "Не удалось автоматически разместить корабль размером " << size << ". Попробуйте ручную расстановку.\n";
			ManualPlacement();
			return;
		}
	}

	std::cout << "Все корабли успешно расставлены автоматически!\n";
	DisplayBoardState();
}

bool HumanPlayer::TryPlaceShip(int size, int row, int col, bool horizontal)
{
	Ship ship(size, { row, col }, horizontal);
	return m_myBoard.PlaceShip(ship);
}

Player::MoveType HumanPlayer::MakeMove()
{
	std::cout << m_name << ", ваш ход:\n";
	int row, col;
	std::cout << "Введите координаты выстрела (ряд столбец): ";
	std::cin >> row >> col;
	return { row, col };
}

void HumanPlayer::DisplayBoardState()
{
	// При расстановке показываем корабли (forOwner = true)
	auto state = m_myBoard.GetVisibleState(true);
	std::cout << "Ваше поле:\n";

	// Вывод номеров столбцов
	std::cout << "  ";
	for (int j = 0; j < m_myBoard.GetSize(); j++)
	{
		std::cout << j << " ";
	}
	std::cout << "\n";

	for (int i = 0; i < m_myBoard.GetSize(); i++)
	{
		std::cout << i << " ";
		for (int j = 0; j < m_myBoard.GetSize(); j++)
		{
			std::cout << state[i][j] << " ";
		}
		std::cout << "\n";
	}
	std::cout << "\n";
}