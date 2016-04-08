using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour 
{
	// Birds' behaviour
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

	private bool m_CanLand;
	private float m_FlyHeight;
	private bool m_BirdBehaviorIsTriggered;
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
		print("Landed");
		yield return new WaitForSeconds(m_LandedDuration);
		m_CanLand = false;
		StartCoroutine(TakeOff());
	}

	// When is taking off
	IEnumerator TakeOff()
	{
		print("TakeOff");

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
		print("Flying");

		Vector3 _DirectionOfCurrentDestination = m_CurrentDestination - transform.position;
		_DirectionOfCurrentDestination = new Vector3(_DirectionOfCurrentDestination.x, 0, _DirectionOfCurrentDestination.z);

		print("CanLand_0");

		while (m_CanLand == false)
		{
			print("CanLand_1");
			m_Rb.AddForce(_DirectionOfCurrentDestination.normalized * m_Spd);
			yield return new WaitForEndOfFrame();
			print("CanLand_2");
		}

		print("CanLand_3");

		m_Rb.velocity = Vector3.zero;
	
		StartCoroutine(Landing());
	}

	// When is landing
	IEnumerator Landing()
	{
		print("Landing");

		DefineLandingSpot();

		while (transform.position.y > 10)
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
		m_BirdBehaviorIsTriggered = false;
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
	}

	void DefineFlyHeight()
	{
		m_FlyHeight = Random.Range(200, 300);
	}

	void DefineLandedDuration()
	{
		print("DefineLandedDuration");
	}

	void DefineLandingSpot()
	{

	}

	void OnTriggerEnter(Collider LandingSpotCollider)
	{
		if (LandingSpotCollider.gameObject.tag == "LandingSpot" && LandingSpotCollider.GetComponentInChildren<LandingSpot>().m_TriggerBirdLanding == true)
		{
			LandingSpotCollider.GetComponent<LandingSpot>().m_TriggerBirdLanding = false;
			m_CanLand = true;
		}
	}

	void OnTriggerExit(Collider LandingSpotCollider)
	{
		if (LandingSpotCollider.gameObject.tag == "LandingSpot" && LandingSpotCollider.GetComponentInChildren<LandingSpot>().m_TriggerBirdLanding == false)
		{
			LandingSpotCollider.GetComponent<LandingSpot>().m_TriggerBirdLanding = true;
		}
	}
}