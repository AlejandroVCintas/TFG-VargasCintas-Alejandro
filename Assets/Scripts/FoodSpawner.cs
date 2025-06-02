using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public Collider2D GridArea;
    public GameObject foodPrefab;
    public Sprite foodSprite;

    private Snake snake;
    private GameObject currentFood;

    private void Awake()
    {
        snake = Object.FindFirstObjectByType<Snake>();
    }

    private void Start()
    {
        SpawnFood();
    }

    public void SpawnFood()
    {
        if (currentFood != null)
        {
            Destroy(currentFood);
        }

        Bounds bounds = GridArea.bounds;
        int x, y;
        Vector2 spawnPosition;
        int attempts = 0;

        // Intentar evitar que aparezca sobre la serpiente
        do
        {
            x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
            y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));
            spawnPosition = new Vector2(x, y);
            attempts++;
        }
        while (snake != null && snake.Occupies(x, y) && attempts < 100);

        currentFood = Instantiate(foodPrefab, spawnPosition, Quaternion.identity);

        // Asigna el sprite si lo hemos definido
        if (foodSprite != null)
        {
            SpriteRenderer sr = currentFood.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = foodSprite;
            }
        }

        // Añadir comportamiento al recoger
        FoodTrigger trigger = currentFood.AddComponent<FoodTrigger>();
        trigger.spawner = this;
    }

    // Componente interno para gestionar la colisión
    private class FoodTrigger : MonoBehaviour
    {
        public FoodSpawner spawner;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") || other.CompareTag("Snake"))
            {
                spawner.SpawnFood();
                Destroy(gameObject);
            }
        }
    }
}
