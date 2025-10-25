#ifndef HUMANPLAYER_HPP
#define HUMANPLAYER_HPP

#include "Player.hpp"
#include <iostream>

class HumanPlayer : public Player
{
public:
	// конструкторы и деконструктор
	HumanPlayer(std::string name, int boardSize);
	~HumanPlayer() override = default;

	// публичные методы
	void PlaceShips() override;
	MoveType MakeMove() override;

private:
	// приватные методы
	void DisplayBoardState();
	void ManualPlacement();
	void AutomaticPlacement();
	bool TryPlaceShip(int size, int row, int col, bool horizontal);
};

#endif // HUMANPLAYER_HPP