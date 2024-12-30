using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public GridManager gridManager;

    [Range(5, 10)] // Controla la cantidad de barcos en el inspector
    public int numberOfShips = 5;

    private List<List<Cell>> ships = new List<List<Cell>>(); // Lista de barcos (cada barco es una lista de celdas)

    public int PlaceShipsRandomly()
    {
        int totalPlacedShips = 0;

        for (int i = 0; i < numberOfShips; i++)
        {
            int size = Random.Range(2, 5); // Tamaño aleatorio entre 2 y 4
            if (PlaceShip(size))
            {
                totalPlacedShips++;
            }
        }

        Debug.Log($"Barcos colocados: {totalPlacedShips}");
        return totalPlacedShips;
    }

    bool PlaceShip(int size)
    {
        List<Cell> affectedCells = new List<Cell>();
        bool placed = false;
        int attempts = 0;

        while (!placed)
        {
            if (attempts > 1000) // Evitar bucle infinito
            {
                Debug.LogError($"No se pudo colocar el barco de tamaño {size} tras 1000 intentos.");
                return false;
            }

            bool horizontal = Random.Range(0, 2) == 0;
            int startX = horizontal ? Random.Range(0, gridManager.cols - size) : Random.Range(0, gridManager.cols);
            int startY = horizontal ? Random.Range(0, gridManager.rows) : Random.Range(0, gridManager.rows - size);

            if (CanPlaceShip(startX, startY, size, horizontal, affectedCells))
            {
                foreach (Cell cell in affectedCells)
                {
                    cell.PlaceShip();
                }

                ships.Add(new List<Cell>(affectedCells)); // Agregar el barco a la lista
                placed = true;
            }

            attempts++;
        }

        return true;
    }

    bool CanPlaceShip(int startX, int startY, int size, bool horizontal, List<Cell> affectedCells)
    {
        affectedCells.Clear();

        for (int i = 0; i < size; i++)
        {
            int x = horizontal ? startX + i : startX;
            int y = horizontal ? startY : startY + i;

            if (x < 0 || x >= gridManager.cols || y < 0 || y >= gridManager.rows)
            {
                return false;
            }

            Cell cell = gridManager.GetCellByIndex(x, y);
            if (cell == null || cell.hasShip)
            {
                return false;
            }

            affectedCells.Add(cell);
        }

        return true;
    }

    public int GetTotalShipCells()
    {
        int totalCells = 0;
        foreach (var ship in ships) // Asegúrate de que 'ships' contiene todos los barcos correctamente colocados
        {
            totalCells += ship.Count; // Cada barco debería reportar correctamente sus celdas
        }
        return totalCells;
    }

    public bool CheckIfShipSunk(Cell hitCell)
    {
        foreach (var ship in ships)
        {
            if (ship.Contains(hitCell))
            {
                // Si todas las celdas del barco están golpeadas, se considera hundido
                if (ship.TrueForAll(cell => cell.isHit))
                {
                    Debug.Log("¡Un barco ha sido completamente hundido!");
                    return true;
                }
            }
        }
        return false;
    }

}
