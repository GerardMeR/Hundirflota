using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager;
    public ShipManager shipManager;

    private int attempts = 0;
    private int aciertos = 0;
    private int totalShipCells = 0;
    public bool isGameOver = false;

    void Start()
    {
        if (gridManager == null || shipManager == null)
        {
            Debug.LogError("Faltan referencias en el GameManager.");
            enabled = false; // Deshabilitamos el script para evitar errores.
            return;
        }

        gridManager.GenerateGrid();
        shipManager.PlaceShipsRandomly();
        totalShipCells = shipManager.GetTotalShipCells();
    }

    public void DetectCellClick(Cell cell)
    {
        if (isGameOver || cell == null || cell.isHit) return;

        attempts++;

        if (cell.hasShip)
        {
            cell.MarkAsHit(Color.red);
            aciertos++;
            Debug.Log($"¡Acierto! Aciertos: {aciertos}/{totalShipCells}");

            if (aciertos == totalShipCells)
            {
                EndGame(); // Llama al fin del juego cuando se hunden todos los barcos
            }
        }
        else
        {
            cell.MarkAsHit(Color.blue);
            Debug.Log("Agua.");
        }
    }

    void EndGame()
    {
        isGameOver = true;
        int score = CalculateScore();
        Debug.Log($"¡Ganaste! Hundiste todos los barcos en {attempts} intentos.\nPuntuación: {score}");
    }

    int CalculateScore()
    {
        if (attempts == 0) return 0; 

        float efficiency = (float)totalShipCells / attempts;
        if (efficiency >= 1.0f) return 100;
        if (efficiency >= 0.75f) return 75;
        if (efficiency >= 0.5f) return 50;
        return 25;
    }
}
