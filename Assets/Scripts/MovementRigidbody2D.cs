using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementRigidbody2D : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    private Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void MoveTo(Vector3 direction)
    {
        rigidbody2D.linearVelocity = direction * moveSpeed;
    }
}
