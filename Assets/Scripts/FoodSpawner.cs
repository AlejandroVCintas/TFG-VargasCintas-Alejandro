using UnityEngine;
using System.Collections.Generic;

public class FoodSpawner : MonoBehaviour
{
    public GameObject Food;
    public Vector2Int gridSize = new Vector2Int(20, 20);
    public Transform snakeTransform;

    [HideInInspector] public GameObject currentFood;

    public void SpawnFood()
    {
        if (currentFood != null)
            Destroy(currentFood);

        Vector3 spawnPosition;
        int maxAttempts = 100;
        int attempts = 0;

        do
        {
            spawnPosition = new Vector3(
                Random.Range(0, gridSize.x),
                Random.Range(0, gridSize.y),
                0
            );
            attempts++;

        } while (IsOnSnake(spawnPosition) && attempts < maxAttempts);

        Transform gridTransform = GameObject.Find("Grid")?.transform;

        currentFood = Instantiate(Food, spawnPosition, Quaternion.identity, gridTransform);

        // Opción: animación
        Animator anim = currentFood.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Spawn");
        }
    }

    private bool IsOnSnake(Vector3 position)
    {
        if (snakeTransform == null) return false;

        // Comprobamos la cabeza y los cuerpos hijos
        if (Vector3.Distance(position, snakeTransform.position) < 0.5f)
            return true;

        foreach (Transform part in snakeTransform.GetComponentsInChildren<Transform>())
        {
            if (Vector3.Distance(position, part.position) < 0.5f)
                return true;
        }

        return false;
    }

    void Start()
    {
        if (snakeTransform == null)
            snakeTransform = GameObject.Find("Snake")?.transform;

        SpawnFood();
    }
}

