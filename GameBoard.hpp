#ifndef GAMEBOARD_HPP
#define GAMEBOARD_HPP

#include <vector>
#include <set>
#include <utility>
#include <string>
#include "Ship.hpp"

class GameBoard
{
public:
	// публичные: переопределение типом
	using ShipsType = std::vector<Ship>;
	using ShotsType = std::set<std::pair<int, int>>;
	using MissesType = std::vector<std::pair<int, int>>;
	using BoardStateType = std::vector<std::vector<char>>;

public:
	// конструкторы и деконструктор
	GameBoard(int size);
	~GameBoard() = default;

	// публичные методы
	bool PlaceShip(const Ship& ship);
	std::string ReceiveShot(std::pair<int, int> coord);
	bool IsAllShipsSunk() const;
	BoardStateType GetVisibleState(bool forOwner) const;

	// геттеры
	int GetSize() const { return m_size; }
	const ShipsType& GetShips() const { return m_ships; }

private:
	// приватные переменные
	int m_size;
	ShipsType m_ships;
	ShotsType m_shots;
	MissesType m_misses;
};

#endif // GAMEBOARD_HPP