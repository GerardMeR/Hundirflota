using UnityEngine;

public class Ship : MonoBehaviour
{
    public int size; // Tamaño del barco
    private int hits = 0;

    public void Hit()
    {
        hits++;
        if (hits >= size)
        {
            Debug.Log("Barco hundido");
        }
    }
}
