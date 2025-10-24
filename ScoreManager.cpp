#include "ScoreManager.hpp"

namespace GameLauncher {

    ScoreManager::ScoreManager(Label^ label)
    {
        score = 0; 
        scoreLabel = label;
        UpdateDisplay();
    }

    ScoreManager::~ScoreManager()
    {
    }

    Void ScoreManager::AddScore(Int64 points)
    {
        score += points;
        UpdateDisplay();
    }

    Void ScoreManager::ResetScore()
    {
        score = 0;
        UpdateDisplay();
    }

    Void ScoreManager::UpdateDisplay()
    {
        scoreLabel->Text = FormatScore();
    }

    String^ ScoreManager::FormatScore()
    {
        return "Score: " + score.ToString();
    }

}