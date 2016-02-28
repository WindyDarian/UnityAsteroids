using UnityEngine;
using System.Collections;

public class BulletBehavior : ADieableBehavior
{
    public float speed;
    public float lifetime_remaining = 1;

    void Start ()
    {
        this.GetComponent<Rigidbody>().velocity += transform.forward * speed;
    }

    void Update()
    {
        if (lifetime_remaining <= 0)
        {
            Destroy(this.gameObject);
        }
        lifetime_remaining -= Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDying)
            return;

        if (other.tag == "Asteroid" ||
            (tag == "PlayerBullet" && other.tag == "AlienShip") || 
            (tag == "AlienBullet" && other.tag == "PlayerShip"))

        {
            var other_des = other.GetComponent<ADieableBehavior>();
            if (other_des)
            {
                other_des.Die();
                this.Die();
            }
        }
    }

    public override void Die()
    {
        base.Die();
    }
}
