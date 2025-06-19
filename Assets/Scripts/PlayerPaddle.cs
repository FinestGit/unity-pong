using UnityEngine;

public class PlayerPaddle : MonoBehaviour
{
    public float speed = 15f;
    public float minY = -8.5f;
    public float maxY = 8.5f;

    private Rigidbody2D rb;
    private float verticalInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        Vector2 movement = new Vector2(0, verticalInput);

        Vector2 targetPosition = rb.position + movement * speed * Time.fixedDeltaTime;

        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

        rb.MovePosition(targetPosition);
    }
}
