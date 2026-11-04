using System.Collections.Generic;
using UnityEngine;

public class PuzzleLaserEmitter : MonoBehaviour
{
	[SerializeField] private LayerMask mirrorLayer;
	[SerializeField] private LayerMask collidableLayers;
	[SerializeField] private LineRenderer line;
	[SerializeField][Min(0.01f)] private float maxDistance = 128.0f;
	[SerializeField][Min(0)] private int maxBounces = 32;

	readonly private List<Vector3> linePoints = new();

	private void Update()
	{
		linePoints.Clear();
		linePoints.Add(transform.position);
		ShootLaser(transform.position, transform.forward.normalized, maxBounces);
		line.positionCount = linePoints.Count;
		line.SetPositions(linePoints.ToArray());
	}

	private void ShootLaser(Vector3 position, Vector3 direction, int bounceLimit)
	{
		if (bounceLimit == 0)
			return;

		bounceLimit--;

		if (!Physics.Raycast(position, direction, out RaycastHit hit, maxDistance, collidableLayers))
		{
			linePoints.Add(position + direction * maxDistance);
			return;
		}

		linePoints.Add(hit.point);
		GameObject target = hit.collider.gameObject;

		if (ObjectIsMirror(target))
		{
			Vector3 reflectedDirection = Vector3.Reflect(direction, hit.normal);
			ShootLaser(hit.point, reflectedDirection, bounceLimit);
		}
		else if (target.TryGetComponent<PuzzleLaserReceiver>(out PuzzleLaserReceiver receiver))
			receiver.OnLaserHit();
	}

	private bool ObjectIsMirror(GameObject target)
	{
		return (mirrorLayer & (1 << target.layer)) != 0;
	}
}
