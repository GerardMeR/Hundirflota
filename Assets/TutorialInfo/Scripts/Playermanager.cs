using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GridManager gridManager;
    public GameManager gameManager;

    void Start()
    {
        if (gridManager == null || gameManager == null)
        {
            Debug.LogError("Faltan referencias en el PlayerManager.");
            enabled = false; 
        }
    }

    void Update()
    {
        if (gameManager == null || gameManager.isGameOver) return;

        if (Input.GetMouseButtonDown(0)) 
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Cell cell = gridManager.GetCell(mousePos.x, mousePos.y);

            if (cell != null && !cell.isHit)
            {
                gameManager.DetectCellClick(cell);
            }
            else if (cell == null)
            {
                Debug.LogWarning("No se encontró una celda en la posición del clic.");
            }
        }
    }
}
