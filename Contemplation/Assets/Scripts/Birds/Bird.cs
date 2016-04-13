using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour 
{
	// Birds' behaviour - What's the use right now ? 
	public enum BirdBehaviour
	{
		Landed = 0,
		TakeOff = 1,
		Flying = 2,
		Landing = 3
	};

	public GameObject m_RefToLandingSpotsManager;
	public BirdBehaviour m_BirdBehaviour;
	public float m_LandedDuration;

	public float m_Spd;

	public bool m_CanLand;
	private float m_LandingHeight;
	private float m_FlyHeight;
	//private bool m_BirdBehaviorIsTriggered;
	public string m_CurrentDestinationName;
	private Vector3 m_CurrentDestination;
	private Rigidbody m_Rb;



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
			m_Rb.AddForce(Vector3.up * 5);
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
			m_Rb.AddForce(_DirectionOfCurrentDestination.normalized * m_Spd);
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
			m_Rb.AddForce(Vector3.down * m_Spd);
			yield return new WaitForEndOfFrame();
		}

		m_Rb.velocity = Vector3.zero;

		DefineNewSpot();
		DefineFlyHeight();
		StartCoroutine(Landed());
	}



	// *** BIRD'S INITIALIZATION ***
	void Initialization()
	{
		m_Rb = GetComponent<Rigidbody>();

		m_BirdBehaviour = BirdBehaviour.Landed;
		//m_BirdBehaviorIsTriggered = false;
		m_CanLand = false;

		DefineFlyHeight();
		DefineNewSpot();
		StartCoroutine(Landed());
	}



	// Setting the bird's up
	void DefineNewSpot()
	{
		int _NumOfSpots = m_RefToLandingSpotsManager.GetComponent<LandingSpotManager>().m_LandingSpots.Length;
		int _SpotToReach = Random.Range(0, _NumOfSpots);

		m_CurrentDestination = m_RefToLandingSpotsManager.GetComponent<LandingSpotManager>().m_LandingSpots[_SpotToReach].transform.position;
		m_CurrentDestinationName =  m_RefToLandingSpotsManager.GetComponent<LandingSpotManager>().m_LandingSpots[_SpotToReach].name;
	}



	void DefineFlyHeight()
	{
		m_FlyHeight = Random.Range(200, 300);
	}



	void DefineLandedDuration()
	{
		
	}



	void DefineLandingSpot()
	{
		//float _Test = DetectCollision.CollisionDetection(this.transform.position, Vector3.down);
		//float _Test = CollisionDetectionA(this.transform.position, Vector3.down);
		/*float _Test = new Vector3	(this.transform.position.x, 
									this.transform.position.y - CollisionDetectionA(this.transform.position, Vector3.down), 
									this.transform.position.z
									);*/
		float _Test = this.transform.position.y - CollisionDetectionA(this.transform.position, Vector3.down);
		float _Margin = 10;
		m_LandingHeight = _Test + _Margin;
		print("LandingHeight: " + m_LandingHeight);
	}



	void OnTriggerEnter(Collider LandingSpotCollider)
	{
		if (LandingSpotCollider.gameObject.tag == "LandingSpot" && LandingSpotCollider.GetComponent<LandingSpot>().m_TriggerBirdLanding == true)
		{
			LandingSpotCollider.GetComponent<LandingSpot>().m_TriggerBirdLanding = false;
			SetCanLand(true);
		}
	}



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

	float CollisionDetectionA(Vector3 ObjectPosition, Vector3 DirectionOfDetection)
	{
		Ray _Ray = new Ray(ObjectPosition, DirectionOfDetection);
		RaycastHit _Hit = new RaycastHit();

		if (Physics.Raycast(_Ray, out _Hit))
		{
			if (_Hit.collider.tag != "LandingSpot")
			{
				Vector3 _HitPoint = _Hit.point;
				print("HIT_POSITION: " + _HitPoint);
				float _DistanceFromObject = Vector3.Distance(ObjectPosition, _HitPoint);
				print("DISTANCE_FROM_FLOOR: " + _DistanceFromObject);

				print("NAME_OF_COLLIDER: " + _Hit.collider.name);
				return _DistanceFromObject;
			}
		
			return 0;
		}
		return 0;
	}
}