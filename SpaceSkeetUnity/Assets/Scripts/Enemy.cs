using System.Collections;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
[RequireComponent(typeof (LineRenderer))]
public class Enemy : MonoBehaviour
{
    Transform playerTransform;
    [SerializeField] int hp;
    Rigidbody rb;
    [SerializeField] float moveSpeed = 5f;
    LineRenderer lineRenderer;
    [SerializeField] Material rayMaterial;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.material = rayMaterial;
        Vector3[] rayPositions = new Vector3[2] { new Vector3(0, 0,-20), new Vector3(0, 0, -20) };
        lineRenderer.SetPositions(rayPositions);
        rb = GetComponent<Rigidbody>();
        playerTransform = FindObjectOfType<Player>().GetComponent<Transform>();
    }

    private void Start()
    {
        Vector3 direction = playerTransform.position - transform.position;
        direction.z = 0f;
        float randomSpeedMultiplier = Random.Range(0.2f, 1.5f);
        rb.velocity = direction * moveSpeed * randomSpeedMultiplier;
        StartCoroutine(KeepAttacking());
    }

    void Attack()
    {
        Vector3[] rayPositions = new Vector3[2] { transform.position, playerTransform.position };
        lineRenderer.SetPositions(rayPositions);
        EnemyManager.instance.DamagePlayer();
        StartCoroutine(ShowRayShot());
    }

    IEnumerator KeepAttacking()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            Attack();
        }
    }

    IEnumerator ShowRayShot()
    {
        yield return new WaitForSeconds(0.2f);
        Vector3[] rayPositions = new Vector3[2] { new Vector3(0, 0, -20), new Vector3(0, 0, -20) };
        lineRenderer.SetPositions(rayPositions);
    }

    public void GetDamaged(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            EnemyManager.instance.ScorePoint();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy"))
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }
}
