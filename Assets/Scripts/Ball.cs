using UnityEngine;

public class Ball : MonoBehaviour
{
    public float initialSpeed = 8f;
    public float maxBounceAngle = 75f;
    public float speedIncreasePerHit = 0.25f;
    public float maxSpeed = 20f;

    private Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        LaunchBall();
    }

    public void LaunchBall()
    {
        rb.position = Vector2.zero;
        rb.linearVelocity = Vector2.zero;

        float xDirection = Random.Range(0, 2) == 0 ? -1f : 1f;

        float yDirection = Random.Range(-1f, 1f);

        Vector2 initialDirection = new Vector2(xDirection, yDirection).normalized;

        rb.linearVelocity = initialDirection * initialSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerPaddle") || collision.gameObject.CompareTag("AIPaddle"))
        {
            Vector2 currentVelocity = rb.linearVelocity;

            float currentMagnitude = currentVelocity.magnitude;
            float newMagnitude = Mathf.Min(currentMagnitude + speedIncreasePerHit, maxSpeed);

            float paddleCenterY = collision.transform.position.y;

            float hitPointY = Mathf.Clamp(transform.position.y - paddleCenterY, -collision.collider.bounds.extents.y, collision.collider.bounds.extents.y);

            float normalizedHitPoint = hitPointY / collision.collider.bounds.extents.y;

            float bounceAngle = normalizedHitPoint * maxBounceAngle;

            float bounceAngleRad = bounceAngle * Mathf.Deg2Rad;

            float newXDirection = -Mathf.Sign(currentVelocity.x);

            float newYDirection = Mathf.Sin(bounceAngleRad);

            Vector2 newDirection = new Vector2(newXDirection, newYDirection).normalized;

            rb.linearVelocity = newDirection * newMagnitude;
        }
    }

    void Update()
    {

    }
}
