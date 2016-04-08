using UnityEngine;
using System.Collections;

public class PathPoint : MonoBehaviour {
	void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(transform.position, 0.5f);
	}
}
