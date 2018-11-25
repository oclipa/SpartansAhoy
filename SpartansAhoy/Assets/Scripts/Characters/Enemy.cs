using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	
    [Range(0f,10f)]
	public float moveSpeed = 4f;  // enemy move speed when moving
	public int damageAmount = 10; // probably deal a lot of damage to kill player immediately

    [Tooltip("Child game object for detecting stun.")]
	public GameObject stunnedCheck; // what gameobject is the stunnedCheck

	public float stunnedTime = 3f;   // how long to be stunned for

    public float killDelayTime = 8f;   // how long to wdelay before destroying this gameobject

    public string stunnedLayer = "StunnedEnemy";  // name of the layer to put enemy on when stunned
	public string playerLayer = "Player";  // name of the player layer to ignore collisions with when stunned
	
    [HideInInspector]
	public bool isStunned = false;  // flag for isStunned

    public GameObject prefabMovingEnemy;
    public GameObject prefabWaypoint;

    public GameObject[] myWaypoints; // to define the movement waypoints

    [Tooltip("How much time in seconds to wait at each waypoint.")]
    public float waitAtWaypointTime = 1f;   // how long to wait at a waypoint
	
	public bool loopWaypoints = true; // should it loop through the waypoints

    // LayerMask to determine what is considered ground for the enemy
    public LayerMask whatIsGround;

    // Transform just below feet for checking if player is grounded
    public Transform groundCheck;

    // SFXs
    public AudioClip stunnedSFX;
	public AudioClip attackSFX;
    public AudioClip explosionSFX;

    // private variables below

    // store references to components on the gameObject
    Transform _transform;
	Rigidbody2D _rigidbody;
	Animator _animator;
	AudioSource _audio;
    SpriteRenderer _spriteRenderer;
    BoxCollider2D _boxCollider;
    CircleCollider2D _circleCollider;

    // movement tracking
    [SerializeField]
	int _myWaypointIndex = 0; // used as index for My_Waypoints
	float _moveTime; 
	float _vx = 0f;
	bool _moving = true;
    bool _isGrounded = false;


    // store the layer number the enemy is on (setup in Awake)
    int _enemyLayer;

	// store the layer number the enemy should be moved to when stunned
	int _stunnedLayer;
	
	void Awake() {
		// get a reference to the components we are going to be changing and store a reference for efficiency purposes
		_transform = GetComponent<Transform> ();
		
		_rigidbody = GetComponent<Rigidbody2D> ();
		if (_rigidbody==null) // if Rigidbody is missing
			Debug.LogError("Rigidbody2D component missing from this gameobject");
		
		_animator = GetComponent<Animator>();
		if (_animator==null) // if Animator is missing
			Debug.LogError("Animator component missing from this gameobject");
		
		_audio = GetComponent<AudioSource> ();
		if (_audio==null) { // if AudioSource is missing
			Debug.LogWarning("AudioSource component missing from this gameobject. Adding one.");
			// let's just add the AudioSource component dynamically
			_audio = gameObject.AddComponent<AudioSource>();
		}

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _circleCollider = GetComponent<CircleCollider2D>();


        if (stunnedCheck==null) {
			Debug.LogError("stunnedCheck child gameobject needs to be setup on the enemy");
		}
		
		// setup moving defaults
		_moveTime = 0f;
		_moving = true;
		
		// determine the enemies specified layer
		_enemyLayer = this.gameObject.layer;

		// determine the stunned enemy layer number
		_stunnedLayer = LayerMask.NameToLayer(stunnedLayer);

		// make sure collision are off between the playerLayer and the stunnedLayer
		// which is where the enemy is placed while stunned
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(playerLayer), _stunnedLayer, true); 
	}
	
	// if not stunned then move the enemy when time is > _moveTime
	void Update () {
		if (!isStunned)
		{
			if (Time.time >= _moveTime) {
				EnemyMovement();
			} else {
				_animator.SetBool("Moving", false);
			}
		}

        // Check to see if character is grounded by raycasting from the middle of the player
        // down to the groundCheck position and see if collected with gameobjects on the
        // whatIsGround layer
        _isGrounded = Physics2D.Linecast(_transform.position, groundCheck.position, whatIsGround);

        // if on ground, ensure waypoints move along ground
        if (_isGrounded)
        {
            if (myWaypoints == null || myWaypoints.Length == 0 && prefabMovingEnemy != null)
            {
                myWaypoints = new GameObject[2];

                GameObject movingEnemy = Object.Instantiate(prefabMovingEnemy, transform.position, Quaternion.identity);

                int i = 0;
                foreach(Transform child in movingEnemy.transform)
                {
                    if (child.CompareTag("Enemy"))
                    {
                        Destroy(child.gameObject);
                    }
                    else if (child.CompareTag("Waypoint"))
                    {
                        myWaypoints[i] = child.gameObject;
                        i++;
                    }
                }
                this.transform.SetParent(movingEnemy.transform);

                //GameObject wayPoint2 = Object.Instantiate(prefabWaypoint, transform.position, Quaternion.identity);

                //myWaypoints = new GameObject[] { wayPoint1, wayPoint2 };
            }

            if (myWaypoints.Length > 1)
            {
                Vector3 groundLeft = new Vector3(ScreenUtils.GetCameraLeftEdge(Camera.main) + 1, -1.982f, 0f);
                Vector3 groundRight = new Vector3(ScreenUtils.GetCameraRightEdge(Camera.main) - 1, -1.982f, 0f);

                myWaypoints[0].transform.position = groundLeft;
                myWaypoints[1].transform.position = groundRight;
            }
        }
    }
	
	// Move the enemy through its rigidbody based on its waypoints
	void EnemyMovement() {
		// if there isn't anything in My_Waypoints
		if ((myWaypoints.Length != 0) && (_moving)) {
			
			// make sure the enemy is facing the waypoint (based on previous movement)
			Flip (_vx);
			
			// determine distance between waypoint and enemy
			_vx = myWaypoints[_myWaypointIndex].transform.position.x-_transform.position.x;
			
			// if the enemy is close enough to waypoint, make it's new target the next waypoint
			if (Mathf.Abs(_vx) <= 0.05f) {
				// At waypoint so stop moving
				_rigidbody.velocity = new Vector2(0, 0);
				
				// increment to next index in array
				_myWaypointIndex++;
				
				// reset waypoint back to 0 for looping
				if(_myWaypointIndex >= myWaypoints.Length) {
					if (loopWaypoints)
						_myWaypointIndex = 0;
					else
						_moving = false;
				}
				
				// setup wait time at current waypoint
				_moveTime = Time.time + waitAtWaypointTime;
			} else {
				// enemy is moving
				_animator.SetBool("Moving", true);
				
				// Set the enemy's velocity to moveSpeed in the x direction.
				_rigidbody.velocity = new Vector2(_transform.localScale.x * moveSpeed, _rigidbody.velocity.y);
			}
			
		}
	}
	
	// flip the enemy to face torward the direction he is moving in
	void Flip(float _vx) {
		
		// get the current scale
		Vector3 localScale = _transform.localScale;
		
		if ((_vx>0f)&&(localScale.x<0f))
			localScale.x*=-1;
		else if ((_vx<0f)&&(localScale.x>0f))
			localScale.x*=-1;
		
		// update the scale
		_transform.localScale = localScale;
	}
	
	// Attack player
	void OnTriggerEnter2D(Collider2D collision)
	{
		if ((collision.tag == "Player") && !isStunned)
		{
			CharacterController2D player = collision.gameObject.GetComponent<CharacterController2D>();
			if (player.playerCanMove) {
				// Make sure the enemy is facing the player on attack
				Flip(collision.transform.position.x-_transform.position.x);
				
				// attack sound
				playSound(attackSFX);
				
				// stop moving
				_rigidbody.velocity = new Vector2(0, 0);
				
				// apply damage to the player
				player.ApplyDamage (damageAmount);
				
				// stop to enjoy killing the player
				_moveTime = Time.time + stunnedTime;
			}
		}
	}
	
	// if the Enemy collides with a MovingPlatform, then make it a child of that platform
	// so it will go for a ride on the MovingPlatform
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag=="MovingPlatform")
		{
			this.transform.parent = other.transform;
		}
	}
	
	// if the enemy exits a collision with a moving platform, then unchild it
	void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.tag=="MovingPlatform")
		{
			this.transform.parent = null;
		}
	}
	
	// play sound through the audiosource on the gameobject
	void playSound(AudioClip clip)
	{
        _audio.PlayOneShot(clip);
	}

    public GameObject explosion;

    // setup the enemy to be stunned
    public void Explode()
    {
        if (explosion)
        {
            playSound(explosionSFX);

            Vector3 explosionPosition = transform.position;
            Instantiate(explosion, explosionPosition, transform.rotation);

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < enemies.Length; i++)
            {
                GameObject otherEnemy = enemies[i];

                if (otherEnemy != gameObject)
                {
                    Vector3 enemyPosition = otherEnemy.transform.position;

                    float distance = Vector3.Distance(explosionPosition, enemyPosition);

                    if (distance < 5f && otherEnemy != null)
                        StartCoroutine(otherEnemy.GetComponent<Enemy>().Kill());
                }
            }


            // have to delay destroying this enemy until the sound has played
            StartCoroutine(Kill());
        }
    }

    IEnumerator Kill()
    {
        //hide this enemy while we wait for it to be destroyed
        Color color = _spriteRenderer.color;
        color.a = 0f;
        _spriteRenderer.color = color;

        // and ensure we cannot hit it
        _boxCollider.enabled = false;
        _circleCollider.enabled = false;

        yield return new WaitForSeconds(killDelayTime);

        if (this.transform.parent.CompareTag("Enemy"))
        {
            Transform parent = this.transform.parent;
            foreach (Transform child in parent)
                Destroy(child.gameObject);
            Destroy(parent.gameObject);
        } 
        else
        {
            Destroy(this.gameObject);
        }
    }

    // setup the enemy to be stunned
    public void Stunned()
	{
		if (!isStunned) 
		{
			isStunned = true;

            // provide the player with feedback that enemy is stunned
            playSound(stunnedSFX);
            _animator.SetTrigger("Stunned");

            // stop moving
            _rigidbody.velocity = new Vector2(0, 0);

            // switch layer to stunned layer so no collisions with the player while stunned
            this.gameObject.layer = _stunnedLayer;
            stunnedCheck.layer = _stunnedLayer;

            // start coroutine to stand up eventually
            StartCoroutine (Stand ());
        }
    }
	
	// coroutine to unstun the enemy and stand back up
	IEnumerator Stand()
	{
		yield return new WaitForSeconds(stunnedTime); 
		
		// no longer stunned
		isStunned = false;
		
		// switch layer back to regular layer for regular collisions with the player
		this.gameObject.layer = _enemyLayer;
		stunnedCheck.layer = _enemyLayer;
		
		// provide the player with feedback
		_animator.SetTrigger("Stand");
	}
}
