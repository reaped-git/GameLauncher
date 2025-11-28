#pragma once

#include "GameLogic.hpp"
#include "ScoreManager.hpp"
#include <memory>
#include <string>

namespace GameLauncher {

    // Базовый класс для управления игрой
    class BaseGameManager
    {
    protected:
        std::string gameVersion;
        Int64 gameId;

    public:
        BaseGameManager(const std::string& version, Int64 id);
        virtual ~BaseGameManager() = default;

        virtual void Initialize() = 0;
        virtual void Shutdown() = 0;

        std::string GetGameVersion() const;
        Int64 GetGameId() const;
    };

    // Дочерний класс - расширенный менеджер игры
    class ExtendedGameManager : public BaseGameManager
    {
    private:
        std::shared_ptr<GameLogic> gameLogic;
        std::shared_ptr<ScoreManager> scoreManager;
        std::string gameMode;

    public:
        ExtendedGameManager(const std::string& version, Int64 id,
            const std::string& mode);

        // Конструктор копирования
        ExtendedGameManager(const ExtendedGameManager& other);

        // Вызов конструктора базового класса
        ExtendedGameManager(const std::string& version, Int64 id);

        ~ExtendedGameManager() = default;

        void Initialize() override;
        void Shutdown() override;

        std::shared_ptr<GameLogic> GetGameLogic() const;
        std::shared_ptr<ScoreManager> GetScoreManager() const;

        // Перегрузка оператора вывода
        friend std::ostream& operator<<(std::ostream& os, const ExtendedGameManager& manager);
    };

}