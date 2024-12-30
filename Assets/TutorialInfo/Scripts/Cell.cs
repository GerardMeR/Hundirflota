using UnityEngine;

public class Cell : MonoBehaviour
{
    public float x;
    public float y;
    public bool hasShip = false; // Indica si hay un barco
    public bool isHit = false;   // Indica si la celda ha sido golpeada

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public void MarkAsHit(Color color)
    {
        if (isHit) return; // Evitar que la celda cambie de color más de una vez

        isHit = true;
        spriteRenderer.color = color;  // Cambia el color a rojo (barco) o azul (agua)
    }

    public void PlaceShip()
    {
        hasShip = true;
        Debug.Log($"Barco colocado en celda ({x}, {y}).");
    }
}
