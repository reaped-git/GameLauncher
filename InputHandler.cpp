#include "InputHandler.hpp"
#include "GameLogic.hpp"

namespace GameLauncher {

    InputHandler::InputHandler(GameLogic^ logic)
    {
        gameLogic = logic;

        // Изначально ни одна плитка не выбрана
        selectedTile1 = nullptr;
        selectedTile2 = nullptr;
    }

    InputHandler::~InputHandler()
    {
    }

    Void InputHandler::HandleTileClick(Object^ sender, EventArgs^ e)
    {
        Button^ clicked = safe_cast<Button^>(sender);

        // Проверяет , можно ли сейчас делать ходы (только в состоянии Playing)
        if (gameLogic->GetCurrentState() != GameLogic::GameState::ePlaying)
            return;

        ProcessTileSelection(clicked);
    }

    Void InputHandler::ProcessTileSelection(Button^ clickedTile)
    {
        // Первая плитка не выбрана - выбирает текущую
        if (selectedTile1 == nullptr)
        {
            selectedTile1 = clickedTile;
            selectedTile1->FlatAppearance->BorderSize = 2; // Визуальное выделение
            OnTileSelected(clickedTile);
        }

        // Клик на уже выбранную плитку - отмена выбора
        else if (selectedTile1 == clickedTile)
        {
            ResetSelection(); 
        }

        // Выбрана вторая плитка - попытка обмена
        else
        {
            selectedTile2 = clickedTile;
            TrySwapTiles();
            ResetSelection();
        }
    }

    Void InputHandler::TrySwapTiles()
    {
        if (selectedTile1 != nullptr && selectedTile2 != nullptr)
        {
            gameLogic->HandleTileSwap(selectedTile1, selectedTile2);
        }
    }

    Void InputHandler::ResetSelection()
    {
        if (selectedTile1 != nullptr)
        {
            selectedTile1->FlatAppearance->BorderSize = 1;
        }

        selectedTile1 = nullptr;
        selectedTile2 = nullptr;
    }

}