using UnityEngine;
using System.Collections;

public class AlienShipBehavior : ADieableBehavior {


    public float sinePeriod;
    public float sineAmplitude;
    public float travelSpeed;
    public GameBoundary2D boundary;

    public float rotation_speed;

    public GameObject bulletPrefab;
    public float shotCooldown;
    public float initShotTime;
   

    Vector3 origin;
    Vector3 destination;
    float timeAlive;
    Vector3 vertDirecton;
    Vector3 direction;
    float sineFrequency;
    float shotCooldownRemain;

    // Use this for initialization
    void Start () {
        var path = Random.Range(0, 3);

        var right_vector = Vector3.right;
        var top_vector = Vector3.forward;
        var left = boundary.left - 2;
        var right = boundary.right + 2;
        var top = boundary.top + 2;
        var down = boundary.down - 2;

        switch (path)
        {
            case 0:
                // right->left
                this.origin = right_vector *  right + top_vector * Random.Range( down,  top);
                this.destination = right_vector *  left + top_vector * Random.Range( down,  top);
                break;
            case 1:
                // left->right
                this.origin = right_vector *  left + top_vector * Random.Range( down,  top);
                this.destination = right_vector *  right + top_vector * Random.Range( down,  top);
                break;
            case 2:
                // top->down
                this.origin = top_vector *  top + right_vector * Random.Range( left,  right);
                this.destination = top_vector *  down + right_vector * Random.Range( left,  right);
                break;
            case 3:
                // down->top
                this.origin = top_vector *  down + right_vector * Random.Range( left,  right);
                this.destination = top_vector *  top + right_vector * Random.Range( left,  right);
                break;
        }
        this.transform.position = this.origin;
        this.timeAlive = 0.0f;
        this.direction = Vector3.Normalize(this.destination - this.origin);
        this.vertDirecton = Vector3.Cross(this.direction , Vector3.up);

        this.sineFrequency = 1 / sinePeriod;
        this.shotCooldownRemain = this.initShotTime;
	}

    void Update()
    {
        if (this.isDying)
            return;

        var rb = this.GetComponent<Rigidbody>();

        this.timeAlive += Time.deltaTime;

        this.transform.position = this.origin + this.direction * travelSpeed * timeAlive
            + this.vertDirecton * Mathf.Sin(this.timeAlive * this.sineFrequency * Mathf.PI * 2) * sineAmplitude;

        this.shotCooldownRemain -= Time.deltaTime;
        if (this.shotCooldownRemain <= 0)
            this.Shoot();

        rb.rotation = Quaternion.AngleAxis(this.rotation_speed * Time.deltaTime, Vector3.up) * rb.rotation;

    }

    private void Shoot()
    {

        AudioSource.PlayClipAtPoint(this.GetComponents<AudioSource>()[1].clip, transform.position);
        shotCooldownRemain += shotCooldown;
        var bullet = (GameObject)Instantiate(bulletPrefab, transform.position, Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.up));
        bullet.GetComponent<Rigidbody>().velocity += this.GetComponent<Rigidbody>().velocity;
        var bullet_wrap = bullet.GetComponent<WarpingBehavior>();
        if (bullet_wrap)
            bullet_wrap.boundary = this.boundary;

    }
	
	void FixedUpdate () {
	
	}

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Boundary")
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    public override void Die()
    {

        AudioSource.PlayClipAtPoint(this.GetComponents<AudioSource>()[0].clip, transform.position);

        base.Die();
    }
}
