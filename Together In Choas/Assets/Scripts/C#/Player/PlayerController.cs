using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 7f;
    [SerializeField] float colourRadius = 3;
    private Rigidbody2D rb;
    private Transform tf;

    private int dataIndex;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
        dataIndex = ColourRestorationManager.Instance.packAndUpdateNodeData(ref tf, colourRadius);
    }

    void Update()
    {
        // Move the player
        Vector2 movement = new Vector2 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (movement.magnitude > 1)
            movement.Normalize();

        if (movement != Vector2.zero)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

            ColourRestorationManager.Instance.packAndUpdateNodeData(ref tf, colourRadius, dataIndex);
        }
    }
}
