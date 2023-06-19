using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class Enemy : MonoBehaviour
{
    Transform playerTransform;
    [SerializeField] int hp;
    Rigidbody rb;
    [SerializeField] float moveSpeed = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = FindObjectOfType<Player>().GetComponent<Transform>();
    }

    private void Start()
    {
        Vector3 direction = playerTransform.position - transform.position;
        direction.z = 0f;
        float randomSpeedMultiplier = Random.Range(0.2f, 2f);
        rb.velocity = direction * moveSpeed * randomSpeedMultiplier;
    }


    public void GetDamaged(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            Destroy(gameObject);
            //score point
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
