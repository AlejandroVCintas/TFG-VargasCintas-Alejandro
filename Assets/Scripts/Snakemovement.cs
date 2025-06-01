using UnityEngine;
using System.Collections.Generic;

public class SnakeMovement : MonoBehaviour
{
    public float moveRate = 0.2f;
    private float timer;
    private Vector2 direction = Vector2.right;
    private Vector2 nextDirection;

    public GameObject bodyPrefab; // Prefab del cuerpo
    private List<Transform> bodyParts = new List<Transform>();
    private List<Vector3> positions = new List<Vector3>();

    void Start()
    {
        positions.Insert(0, transform.position); // Guardamos la posición inicial de la cabeza
    }

    void Update()
    {
        // Teclado (PC)
        if (Input.GetKeyDown(KeyCode.UpArrow) && direction != Vector2.down)
            nextDirection = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow) && direction != Vector2.up)
            nextDirection = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction != Vector2.right)
            nextDirection = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow) && direction != Vector2.left)
            nextDirection = Vector2.right;
    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (timer >= moveRate)
        {
            timer = 0f;
            direction = nextDirection;
            Move();
        }
    }

    void Move()
    {
        Vector3 prevPos = transform.position;
        transform.position = new Vector3(
            Mathf.Round(transform.position.x + direction.x),
            Mathf.Round(transform.position.y + direction.y),
            0f
        );

        positions.Insert(0, transform.position);
        if (bodyParts.Count > 0)
        {
            for (int i = 0; i < bodyParts.Count; i++)
            {
                Vector3 temp = bodyParts[i].position;
                bodyParts[i].position = positions[i + 1];
            }
        }

        if (positions.Count > bodyParts.Count + 1)
            positions.RemoveAt(positions.Count - 1);
    }

    public void Grow()
    {
        GameObject segment = Instantiate(bodyPrefab);
        Vector3 spawnPos = bodyParts.Count > 0 ? bodyParts[bodyParts.Count - 1].position : transform.position;
        segment.transform.position = spawnPos;
        bodyParts.Add(segment.transform);
    }

    // Llamado desde botones UI en móvil
    public void SetDirection(string dir)
    {
        if (dir == "Up" && direction != Vector2.down) nextDirection = Vector2.up;
        else if (dir == "Down" && direction != Vector2.up) nextDirection = Vector2.down;
        else if (dir == "Left" && direction != Vector2.right) nextDirection = Vector2.left;
        else if (dir == "Right" && direction != Vector2.left) nextDirection = Vector2.right;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
    if (!other.CompareTag("Food"))
        {
        // Si no hemos tocado comida, significa que chocamos con un muro o el cuerpo
            Debug.Log("Game Over");
            Time.timeScale = 0f; // Pausa el juego
        }
    }

}
