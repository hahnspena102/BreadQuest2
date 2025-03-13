using UnityEngine;

public class DoNot : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f; 
    private Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        // Set an initial velocity, tried to make it random
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        Vector2 initialVelocity = new Vector2(randomX, randomY).normalized * moveSpeed;

        body.linearVelocity = new Vector2(moveSpeed, moveSpeed);
        Debug.Log($"Initial Velocity: {body.linearVelocity}");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with a wall
        if (collision.gameObject.name == "Walls")
        {
            // Calculate reflected velocity
            //Vector2 newVelocity = Vector2.Reflect(body.linearVelocity, collision.contacts[0].normal);
            //body.linearVelocity = newVelocity;
            //Debug.Log($"Contact with Wall. New Velocity: {body.linearVelocity}");
        }
        else
        {
            Debug.Log($"Contact with {collision.gameObject.name}");
        }
    }
}