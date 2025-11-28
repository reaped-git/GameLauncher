#include "Ship.hpp"

Ship::Ship(int size, std::pair<int, int> startCoord, bool isHorizontal)
	: m_size(size)
	, m_isHorizontal(isHorizontal)
	, m_hits(size, false)
{
	m_coordinates.clear();

	// Генерация всех координат корабля
	for (int i = 0; i < m_size; i++)
	{
		if (m_isHorizontal)
		{
			m_coordinates.push_back({ startCoord.first, startCoord.second + i });
		}
		else
		{
			m_coordinates.push_back({ startCoord.first + i, startCoord.second });
		}
	}
}

// Конструктор копирования
Ship::Ship(const Ship& other)
	: m_size(other.m_size)
	, m_coordinates(other.m_coordinates)
	, m_hits(other.m_hits)
	, m_isHorizontal(other.m_isHorizontal)
{
}

// Перегрузка оператора присваивания
Ship& Ship::operator=(const Ship& other)
{
	if (this != &other) {
		m_size = other.m_size;
		m_coordinates = other.m_coordinates;
		m_hits = other.m_hits;
		m_isHorizontal = other.m_isHorizontal;
	}
	return *this;
}

// Перегрузка оператора сравнения
bool Ship::operator==(const Ship& other) const
{
	return m_size == other.m_size &&
		m_coordinates == other.m_coordinates &&
		m_isHorizontal == other.m_isHorizontal;
}

bool Ship::operator!=(const Ship& other) const
{
	return !(*this == other);
}

bool Ship::IsSunk() const
{
	for (bool hit : m_hits)
	{
		if (!hit) return false;
	}
	return true;
}

// Перегрузка оператора преобразования в строку
Ship::operator std::string() const
{
	std::stringstream ss;
	ss << "Ship[size=" << m_size << ", horizontal=" << m_isHorizontal
		<< ", sunk=" << IsSunk() << ", coordinates=" << m_coordinates.size() << "]";
	return ss.str();
}

bool Ship::TakeHit(std::pair<int, int> coord)
{
	for (int i = 0; i < m_size; i++)
	{
		if (m_coordinates[i] == coord)
		{
			m_hits[i] = true;
			return true;
		}
	}
	return false;
}