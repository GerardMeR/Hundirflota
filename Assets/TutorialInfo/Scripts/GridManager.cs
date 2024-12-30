using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;   // Prefab de la celda
    public int rows = 30;           // N�mero de filas
    public int cols = 30;           // N�mero de columnas
    public float cellSize = 3;      // Tama�o horizontal de cada celda
    public float cellSizeY = 3;     // Tama�o vertical de cada celda
    public float startX = 31.5f;    // Posici�n inicial X del grid
    public float startY = 31.5f;    // Posici�n inicial Y del grid
    public int totalShips = 5;      // N�mero inicial de barcos (se ajustar� din�micamente)

    public Cell[,] grid;            // Matriz de celdas

    void Start()
    {
        totalShips = Random.Range(5, 11); // N�mero aleatorio de barcos entre 5 y 10
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        grid = new Cell[rows, cols];
        List<Cell> availableCells = new List<Cell>();  // Lista de celdas disponibles para colocar barcos
        Debug.Log($"Generando grid de {rows}x{cols}");

        // Crear las celdas
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                float posX = startX + x * cellSize;
                float posY = startY - y * cellSizeY;

                GameObject cellObject = Instantiate(cellPrefab, new Vector3(posX, posY, 0), Quaternion.identity, transform);
                Cell cell = cellObject.GetComponent<Cell>();
                if (cell != null)
                {
                    cell.Initialize(posX, posY);
                    grid[x, y] = cell;
                    availableCells.Add(cell);  // A�adir la celda a la lista de celdas disponibles
                }
            }
        }

        // Colocar barcos aleatorios en las celdas disponibles
        PlaceRandomShips();

        Debug.Log($"Grid generado correctamente con {cols * rows} celdas.");
    }

    private void PlaceRandomShips()
    {
        int[] shipSizes = { 5, 4, 3, 3, 2 }; // Tama�os de los barcos disponibles
        int shipsPlaced = 0;

        while (shipsPlaced < totalShips)
        {
            int shipSize = shipSizes[Random.Range(0, shipSizes.Length)]; // Seleccionar un tama�o de barco aleatorio
            bool horizontal = Random.Range(0, 2) == 0;                  // Determinar orientaci�n del barco (horizontal o vertical)

            int startX = horizontal ? Random.Range(0, cols - shipSize) : Random.Range(0, cols);
            int startY = horizontal ? Random.Range(0, rows) : Random.Range(0, rows - shipSize);

            if (CanPlaceShip(startX, startY, shipSize, horizontal))
            {
                PlaceShip(startX, startY, shipSize, horizontal);
                shipsPlaced++;
                Debug.Log($"Barco de tama�o {shipSize} colocado. Total barcos colocados: {shipsPlaced}/{totalShips}");
            }
        }

        Debug.Log($"Se colocaron {totalShips} barcos aleatoriamente.");
    }

    private bool CanPlaceShip(int startX, int startY, int size, bool horizontal)
    {
        for (int i = 0; i < size; i++)
        {
            int x = horizontal ? startX + i : startX;
            int y = horizontal ? startY : startY + i;

            if (x < 0 || x >= cols || y < 0 || y >= rows)
            {
                return false; // Fuera de los l�mites del grid
            }

            Cell cell = grid[x, y];
            if (cell == null || cell.hasShip)
            {
                return false; // Celda ocupada o no v�lida
            }
        }

        return true; // Todas las celdas son v�lidas
    }

    private void PlaceShip(int startX, int startY, int size, bool horizontal)
    {
        for (int i = 0; i < size; i++)
        {
            int x = horizontal ? startX + i : startX;
            int y = horizontal ? startY : startY + i;

            Cell cell = grid[x, y];
            if (cell != null)
            {
                cell.PlaceShip(); // Marcar la celda como ocupada por un barco
            }
        }
    }

    public Cell GetCell(float x, float y)
    {
        float adjustedX = (x - startX) / cellSize;
        float adjustedY = (startY - y) / cellSizeY;

        int gridX = Mathf.RoundToInt(adjustedX);
        int gridY = Mathf.RoundToInt(adjustedY);

        if (gridX >= 0 && gridX < cols && gridY >= 0 && gridY < rows)
        {
            return grid[gridX, gridY];
        }
        return null;
    }

    public Cell GetCellByIndex(int x, int y)
    {
        if (x < 0 || x >= cols || y < 0 || y >= rows)
        {
            Debug.LogError($"�ndice fuera de rango: ({x}, {y})");
            return null;
        }

        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                float posX = startX + x * cellSize;
                float posY = startY - y * cellSizeY;
                Vector3 position = new Vector3(posX, posY, 0);
                Gizmos.DrawWireCube(position, new Vector3(cellSize, cellSizeY, 0));
            }
        }
    }
}
