using UnityEngine;
using System.Collections;

public class AsteroidBehavior : ADieableBehavior {

    public GameObject spawnPrefab;
    public float maxSpawnSpeed;
    public float minSpawnSpeed;
    public float rotationRate;
    public int spawnCount;
    public float spawnRadius;
    public int scoreOnDeath;
    private Vector3 rotationAxis;
    private GameplayLogic gameplay;

	
    // Use this for initialization
	void Start () {

        var direction = AsteroidsMathHelper.randomDirectionXZ();
        this.GetComponent<Rigidbody>().velocity = direction * Random.Range(minSpawnSpeed, maxSpawnSpeed);

        this.rotationAxis = Random.onUnitSphere;
        this.GetComponent<Rigidbody>().rotation = Random.rotation;

        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject)
        {
            gameplay = gameControllerObject.GetComponent<GameplayLogic>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        var rb = this.GetComponent<Rigidbody>();
        rb.rotation = rb.rotation * Quaternion.AngleAxis(Time.deltaTime * this.rotationRate, this.rotationAxis);
	}

    void OnTriggerEnter(Collider other)
    {
        if (isDying)
            return;

        if (other.tag == "PlayerShip" || other.tag == "AlienShip")
        {
            var other_des = other.GetComponent<ADieableBehavior>();
            if (other_des)
            {
                other_des.Die();
                this.Die();
            }
        }
    }

    // since spawning on OnDestroy() is dirty
    public override void Die()
    {
        if (isDying)
            return;
        isDying = true;

        AudioSource.PlayClipAtPoint(this.GetComponents<AudioSource>()[0].clip, transform.position);
       

        if (this.gameplay)
        {
            gameplay.addScore(this.scoreOnDeath);
        }

        if (this.spawnPrefab)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                var spawn_pos = AsteroidsMathHelper.randomDirectionXZ() * Random.Range(0.0f, this.spawnRadius);
                var spawnling = (GameObject)Instantiate(spawnPrefab, this.transform.position + spawn_pos, Quaternion.identity);
                spawnling.GetComponent<Rigidbody>().velocity += this.GetComponent<Rigidbody>().velocity;

                var spawnling_wrap = spawnling.GetComponent<WarpingBehavior>();
                if (spawnling_wrap)
                    spawnling_wrap.boundary = this.GetComponent<WarpingBehavior>().boundary;
            }
        }

        base.Die();
    }
}
