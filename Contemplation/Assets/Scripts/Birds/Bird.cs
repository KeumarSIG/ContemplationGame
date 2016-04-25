using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour 
{
	#region Speed
	[Header("- Speed related")]

	public float m_SpdTakeOff;
	public float m_SpdFly;
	public float m_SpdLand;
	#endregion Speed



	#region Behavior
	[Header("- Behaviour related")]

	public float m_LandingMargin;
	public float m_LandedDurationMin;
	public float m_LandedDurationMax;
	private float m_LandedDuration;

	public float m_FlyHeightMin;
	public float m_FlyHeightMax;
	private float m_FlyHeight;

	private float m_LandingHeight;

	public float m_MinDistanceForward;
	public float m_MinDistanceSide;

	private GameObject m_CollisionChecker;
	public bool m_StopBecauseWall;
	public bool m_IsChangingDirection;
	#endregion Behavior



	#region Other
	[Header("- Other")]

	public bool m_Debug;
	public GameObject m_RefToLandingSpotsManager;
	private bool m_CanLand;
	private int m_LastDestination;
	private Rigidbody m_Rb;
	private Vector3 m_CurrentDestination;
	#endregion Other



	void Start () 
	{
		//m_Debug = true;
		Initialization();
	}
		


	//  Bird's initialization
	void Initialization()
	{
		m_Rb = GetComponent<Rigidbody>();
		m_CollisionChecker = transform.GetChild(0).gameObject; // It has to be child(0), check in the hierarchy

		m_CanLand = false; 
		m_LastDestination = -1; // The last destination has to be initialized to -1 (so that later, we can make sure the player doesn't target the same landing spot)
		m_IsChangingDirection = false;

		DefineFlyHeight();
		DefineNewSpot();
		StartCoroutine(Landed());
	}



	// *** BEHAVIOR ***

	// When landed
	IEnumerator Landed()
	{
		print("1 - Landed");

		DefineLandedDuration();

		yield return new WaitForSeconds(m_LandedDuration);

		SetCanLand(false);
		StartCoroutine(TakeOff());
	}



	// When is taking off
	IEnumerator TakeOff()
	{
		print("2 - TakeOff");

		while (transform.position.y < m_FlyHeight)
		{
			GoUpwards();
			yield return new WaitForEndOfFrame();
		}

		m_Rb.velocity = Vector3.zero;
			
		StartCoroutine(Flying());
	}



	// When is flying
	IEnumerator Flying()
	{
		print("3 - Flying");

		// Define direction to reach the current destination
		Vector3 _DirectionOfCurrentDestination = m_CurrentDestination - transform.position;
		_DirectionOfCurrentDestination = new Vector3(_DirectionOfCurrentDestination.x, 0, _DirectionOfCurrentDestination.z);


		// While the bird doesn't have to land
		while (m_CanLand == false)
		{
			// If no collision has been detected
			if (m_IsChangingDirection == false) 
			{
				while (true)
				{
					ChangeOrientation(_DirectionOfCurrentDestination);
					GoForward(_DirectionOfCurrentDestination);
					CheckCollision();

					if (m_IsChangingDirection == true)
					{
						bool _Avoidance = CustomFunctions.RandomBool();
						print("avoidance = " + _Avoidance);

						while (m_IsChangingDirection == true)
						{
							if (_Avoidance == false)
							{
								GoForward(_DirectionOfCurrentDestination);
								GoUpwards();
							}

							else if (_Avoidance == true)
							{
								GoForward(_DirectionOfCurrentDestination);
								GoUpwards();
							}

							yield return new WaitForEndOfFrame();
						}
					}
					yield return new WaitForEndOfFrame();
				}
			}
			yield return new WaitForEndOfFrame();
		}

		m_Rb.velocity = Vector3.zero;
		StartCoroutine(Landing());
	}



	// When is landing
	IEnumerator Landing()
	{
		print("4 - Landing");

		DefineLandingSpot();

		while (transform.position.y > m_LandingHeight)
		{
			if (m_StopBecauseWall == false)
			{
				m_Rb.AddForce(Vector3.down * m_SpdLand);
				yield return new WaitForEndOfFrame();
			}
		}

		m_Rb.velocity = Vector3.zero;

		DefineNewSpot();
		DefineFlyHeight();
		StartCoroutine(Landed());
	}







	// Setting the bird's up
	void DefineNewSpot()
	{
		int _NumOfSpots = m_RefToLandingSpotsManager.GetComponent<LandingSpotManager>().m_LandingSpots.Length;
		int _SpotToReach = Random.Range(0, _NumOfSpots);

		// If the SPOT TO REACH is the same than the last destination, we keep on changing the SPOT TO REACH. -1 is to make sure it doesn't work while there's no spot to reach
		if (m_LastDestination != -1)
		{
			while (_SpotToReach == m_LastDestination)
			{
				_SpotToReach = Random.Range(0, _NumOfSpots);
			}
		}

		m_LastDestination = _SpotToReach;
		m_CurrentDestination = m_RefToLandingSpotsManager.GetComponent<LandingSpotManager>().m_LandingSpots[_SpotToReach].transform.position;
	}



	void ChangeOrientation(Vector3 DirectionOrientation)
	{
		// Rotate the bird towards its direction
		float _RotationSpd = Time.deltaTime;
		Vector3 _NewDir = Vector3.RotateTowards(transform.forward, DirectionOrientation, _RotationSpd, 0);
		//Debug.DrawRay(transform.position, _NewDir, Color.red);
		transform.rotation = Quaternion.LookRotation(_NewDir);
	}



	void DefineFlyHeight()
	{
		m_FlyHeight = Random.Range(m_FlyHeightMin, m_FlyHeightMax);
	}



	void DefineLandedDuration()
	{
		m_LandedDuration = Random.Range(m_LandedDurationMin, m_LandedDurationMax);
	}



	void DefineLandingSpot()
	{
		float _LandingHeight = this.transform.position.y - CustomFunctions.GetCollisionDistance(this.transform.position, Vector3.down);
		m_LandingHeight = _LandingHeight + m_LandingMargin;
		//print("LandingHeight: " + m_LandingHeight);
	}



	// When entering a landing spot
	void OnTriggerEnter(Collider LandingSpotCollider)
	{
		if (LandingSpotCollider.gameObject.tag == "LandingSpot" && LandingSpotCollider.GetComponent<LandingSpot>().m_TriggerBirdLanding == true)
		{
			LandingSpotCollider.GetComponent<LandingSpot>().m_TriggerBirdLanding = false;
			SetCanLand(true);
		}
	}



	// When leaving a landing spot
	void OnTriggerExit(Collider LandingSpotCollider)
	{
		if (LandingSpotCollider.gameObject.tag == "LandingSpot" && LandingSpotCollider.GetComponent<LandingSpot>().m_TriggerBirdLanding == false)
		{
			LandingSpotCollider.GetComponent<LandingSpot>().m_TriggerBirdLanding = true;
		}
	}



	void SetCanLand(bool CanLand)
	{
		m_CanLand = CanLand;
	}



	void CheckCollision()
	{
		float _ForwardCollision = CheckCollisionForward();
		float _LeftCollision = CheckCollisionLeft();
		float _RightCollision = CheckCollisionRight();

		if (_ForwardCollision != 0 || _LeftCollision != 0 || _RightCollision != 0)
		{
			StopBird();
		}

		else 
		{
			//ChangeDirection(true);
		}
	}
		


	float CheckCollisionForward()
	{
		float _ForwardCollision = CustomFunctions.GetCollisionDistance(transform.position, this.transform.forward);

		if (_ForwardCollision <= m_MinDistanceForward && _ForwardCollision != 0)
		{
			//StopBird();
			return _ForwardCollision;
		}

		else 
		{	
			return 0;
		}
	}



	float CheckCollisionRight()
	{
		float _RightCollision = CustomFunctions.GetCollisionDistance(transform.position, this.transform.right);

		if (_RightCollision <= m_MinDistanceSide && _RightCollision != 0)
		{
			//StopBird();
			return _RightCollision;
		}

		else 
		{
			return 0;
		}
	}



	float CheckCollisionLeft()
	{
		float _LeftCollision = CustomFunctions.GetCollisionDistance(transform.position, -this.transform.right);

		if (_LeftCollision <= m_MinDistanceSide && _LeftCollision != 0)
		{
			//StopBird();
			return _LeftCollision;
		}

		else 
		{
			return 0;
		}
	}



	void StopBird()
	{
		m_Rb.velocity = Vector3.zero;
		ChangeDirection(true);
	}

	void ChangeDirection(bool Direction)
	{
		m_IsChangingDirection = Direction;
	}


	// What corresponds to "CheckCollisionBottom()" is "DefineLandingSpot()"



	void OnDrawGizmos()
	{
		if (m_Debug == true)
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawLine(transform.position, transform.position + transform.forward * m_MinDistanceForward);

			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(transform.position, transform.position + transform.right * m_MinDistanceSide);

			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(transform.position, transform.position - transform.right * m_MinDistanceSide);

			Gizmos.color = Color.black;
			Gizmos.DrawLine(transform.position, transform.position + Vector3.down * m_FlyHeight);
		}
	}



	IEnumerator AvoidMovement()
	{
		yield return new WaitForEndOfFrame();

		//RayUpwards();
		//RaySideway();
	}



	void RayUpwards()
	{
		//bool _Collision = CustomFunctions.GetCollision(m_CollisionChecker.transform.position, m_CollisionChecker.transform.forward);
		m_CollisionChecker.transform.Rotate(Vector3.up * Time.deltaTime * 50);
	}



	void RaySideway()
	{

	}



	void GoForward(Vector3 DirectionOfCurrentDestination)
	{
		m_Rb.AddForce(DirectionOfCurrentDestination.normalized * m_SpdFly);
	}

	void GoUpwards()
	{
		m_Rb.AddForce(Vector3.up * m_SpdTakeOff);
	}
}