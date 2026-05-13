using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float minSize = 0.5f;
    public float maxSize = 2.0f;
    public float minSpeed = 200f;
    public float maxSpeed = 400f;
    public float maxSpinSpeed = 10f;
    Rigidbody2D rb;
    public GameObject collisionEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3 (randomSize, randomSize, 1);

        float randomSpeed = Random.Range(minSpeed, maxSpeed) / randomSize;
        Vector2 randomDirection = Random.insideUnitCircle;
        rb.AddForce(randomDirection * randomSpeed);

        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        rb.AddTorque(randomTorque);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 collisionPoint = collision.GetContact(0).point;
        GameObject bounceEffect = Instantiate(collisionEffect, collisionPoint, Quaternion.identity);
        Destroy(bounceEffect, 1f);
    }
}
