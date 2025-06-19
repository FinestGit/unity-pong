using UnityEngine;

public class AIPaddle : MonoBehaviour
{
    public float speed = 8f;
    public float maxSpeedAI = 12f;
    public float struggleSpeedThreshold = 15f;
    public float strugglePenalty = 0.5f;
    public float moveThreshold = 0.05f;

    public float minY = -8.5f;
    public float maxY = 8.5f;

    public float aimRandomness = 0.5f;
    public float aimUpdateInterval = 0.5f;
    private float nextAimUpdateTime;
    private float currentAimOffset;

    private Rigidbody2D rb;
    private GameObject ball;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball == null)
        {
            Debug.LogError("Ball not found in the scene. Please ensure there is a GameObject with the 'Ball' tag.");
        }
        nextAimUpdateTime = Time.time;
        SetRandomAimOffset();
    }

    void SetRandomAimOffset()
    {
        currentAimOffset = Random.Range(-aimRandomness, aimRandomness);
    }

    void FixedUpdate()
    {
        if (ball != null)
        {
            if (Time.time >= nextAimUpdateTime)
            {
                SetRandomAimOffset();
                nextAimUpdateTime = Time.time + aimUpdateInterval;
            }

            float effectiveAISpeed = speed;
            Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();
            if (ballRb != null && ballRb.linearVelocity.magnitude >= struggleSpeedThreshold)
            {
                // If the ball is moving fast, apply a penalty to the AI speed
                effectiveAISpeed *= strugglePenalty;
            }
            effectiveAISpeed = Mathf.Min(effectiveAISpeed, maxSpeedAI);

            float targetY = ball.transform.position.y + currentAimOffset * (GetComponent<BoxCollider2D>().size.y / 2f);

            if (Mathf.Abs(rb.position.y - targetY) < moveThreshold)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            float newYPosition = Mathf.MoveTowards(rb.position.y, targetY, effectiveAISpeed * Time.fixedDeltaTime);

            newYPosition = Mathf.Clamp(newYPosition, minY, maxY);

            rb.MovePosition(new Vector2(rb.position.x, newYPosition));
        }
    }
}
