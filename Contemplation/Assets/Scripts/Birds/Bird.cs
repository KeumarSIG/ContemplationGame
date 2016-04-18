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
	#endregion Behavior

	#region Other
	[Header("- Other")]
	public GameObject m_RefToLandingSpotsManager;
	private bool m_CanLand;
	private int m_LastDestination;
	private Rigidbody m_Rb;
	private Vector3 m_CurrentDestination;
	#endregion Other



	void Start () 
	{
		Initialization();
	}
		


	// *** BEHAVIOR ***

	// When landed
	IEnumerator Landed()
	{
		print("1 - Landed");

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
			m_Rb.AddForce(Vector3.up * m_SpdTakeOff);
			yield return new WaitForEndOfFrame();
		}

		m_Rb.velocity = Vector3.zero;
			
		StartCoroutine(Flying());
	}



	// When is flying
	IEnumerator Flying()
	{
		print("3 - Flying");

		Vector3 _DirectionOfCurrentDestination = m_CurrentDestination - transform.position;
		_DirectionOfCurrentDestination = new Vector3(_DirectionOfCurrentDestination.x, 0, _DirectionOfCurrentDestination.z);

		while (m_CanLand == false)
		{
			m_Rb.AddForce(_DirectionOfCurrentDestination.normalized * m_SpdFly);

			CheckCollision();

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
			m_Rb.AddForce(Vector3.down * m_SpdLand);
			yield return new WaitForEndOfFrame();
		}

		m_Rb.velocity = Vector3.zero;

		DefineNewSpot();
		DefineFlyHeight();
		StartCoroutine(Landed());
	}



	//  Bird's initialization
	void Initialization()
	{
		m_Rb = GetComponent<Rigidbody>();

		m_CanLand = false;
		m_LastDestination = -1;

		DefineFlyHeight();
		DefineNewSpot();
		StartCoroutine(Landed());
	}



	// Setting the bird's up
	void DefineNewSpot()
	{
		int _NumOfSpots = m_RefToLandingSpotsManager.GetComponent<LandingSpotManager>().m_LandingSpots.Length;
		int _SpotToReach = Random.Range(0, _NumOfSpots);

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
		float _LandingHeight = this.transform.position.y - CustomFunctions.CollisionDetection(this.transform.position, Vector3.down);
		m_LandingHeight = _LandingHeight + m_LandingMargin;
		print("LandingHeight: " + m_LandingHeight);
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
		float _ForwardCollision = CustomFunctions.CollisionDetection(transform.position, Vector3.forward);
		if (_ForwardCollision <= m_MinDistanceForward)
		{
			print("Je vais mourir, FORWARD");
		}

		float _RightCollision = CustomFunctions.CollisionDetection(transform.position, Vector3.right);
		if (_RightCollision <= m_MinDistanceSide)
		{
			print("Je vais mourir, LEFT");
		}

		float _LeftCollision = CustomFunctions.CollisionDetection(transform.position, Vector3.left);
		if (_LeftCollision <= m_MinDistanceSide)
		{
			print("Je vais mourir, RIGHT");
		}

		Debug.DrawLine(transform.position, Vector3.forward * 20);
		Debug.DrawLine(transform.position, Vector3.right * 20);
		Debug.DrawLine(transform.position, Vector3.left * 20);
	}



	void OnDrawGizmos()
	{

	}
}