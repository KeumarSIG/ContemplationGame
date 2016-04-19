using UnityEngine;
using System.Collections;

public static class CustomFunctions
{
	public static float GetCollisionDistance(Vector3 ObjectPosition, Vector3 DirectionOfDetection)
	{
		Ray _Ray = new Ray(ObjectPosition, DirectionOfDetection);
		RaycastHit _Hit = new RaycastHit();

		if (Physics.Raycast(_Ray, out _Hit))
		{
			if (_Hit.collider.tag != "LandingSpot")
			{
				Vector3 _HitPoint = _Hit.point;
				float _DistanceFromObject = Vector3.Distance(ObjectPosition, _HitPoint);
				return _DistanceFromObject;
			}

			else 
			{
				return 0;
			}
		}

		else 
		{
			return 0;
		}
	}

	public static bool GetCollision(Vector3 ObjectPosition, Vector3 DirectionOfDetection)
	{
		Ray _Ray = new Ray(ObjectPosition, DirectionOfDetection);
		RaycastHit _Hit = new RaycastHit();

		if (Physics.Raycast(_Ray, out _Hit))
		{
			if (_Hit.collider.tag != "LandingSpot")
			{
				return true;
			}

			else 
			{
				return false;
			}
		}

		else 
		{
			return false;
		}
	}
}