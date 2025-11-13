#include <iostream>
#include <locale>
#include "GameManager.hpp"
#include "UserInterface.hpp"

int main()
{
    while (true) {
        // Устанавливаем локаль для поддержки русского языка
        setlocale(LC_ALL, "Russian");

        std::cout << "========================================\n";
        std::cout << "           МОРСКОЙ БОЙ\n";
        std::cout << "========================================\n";
        std::cout << "Правила игры:\n";
        std::cout << "- Расставьте корабли на поле 10x10\n";
        std::cout << "- Корабли не могут касаться друг друга\n";
        std::cout << "- Стандартный набор кораблей:\n";
        std::cout << "  1 корабль - 4 клетки\n";
        std::cout << "  2 корабля - 3 клетки\n";
        std::cout << "  3 корабля - 2 клетки\n";
        std::cout << "  4 корабля - 1 клетка\n";
        std::cout << "- Вы можете выбрать ручную или автоматическую расстановку\n";
        std::cout << "========================================\n\n";

        try
        {
            const int BOARD_SIZE = 10;
            GameManager gameManager(BOARD_SIZE);

            // Настройка игры
            gameManager.SetupGame();

            std::cout << "\n========================================\n";
            std::cout << "           НАЧАЛО ИГРЫ!\n";
            std::cout << "========================================\n";
            std::cout << "После каждого хода будут показаны оба поля.\n";
            std::cout << "Нажмите Enter для продолжения...";
            std::cin.ignore();
            std::cin.get();

            // Запуск игрового цикла
            gameManager.RunGameLoop();

            std::cout << "\nЕще раз? (y/n)\n";
            
            char again;
            std::cin >> again;
            if (again == 'y') continue;
            else break;
        }
        catch (const std::exception& e)
        {
            std::cerr << "Произошла ошибка: " << e.what() << std::endl;
            return 1;
        }
        catch (...)
        {
            std::cerr << "Произошла неизвестная ошибка!" << std::endl;
            return 1;
        }
    }
    return 0;
}