using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveOnPath : MonoBehaviour {
	public Transform[] movePoints;
	public float speed = 5;

	private const float STOPPING_DISTANCE_THRESHOLD = 0.2f;

	private Transform targetMovePoint;
	private NavMeshAgent navMeshAgent;

	void Start() {
		navMeshAgent = GetComponent<NavMeshAgent>();
		setMovePoint(0);
	}

	void FixedUpdate() {
		if (atTargetMovePoint()) {
			setMovePoint(nextMovePointIndex());
		}
	}

	private bool atTargetMovePoint() {
		//TODO: Is there a better way to test this?
		var xDistance = Math.Abs(transform.position.x - targetMovePoint.position.x);
		var zDistance = Math.Abs(transform.position.z - targetMovePoint.position.z);
		var threshold = navMeshAgent.stoppingDistance + STOPPING_DISTANCE_THRESHOLD;

		return xDistance <= threshold && zDistance <= threshold;
	}

	private int nextMovePointIndex() {
		var movePointIndex = Array.IndexOf(movePoints, targetMovePoint);
		var atLastMovePoint = (movePointIndex == movePoints.Length - 1);

		return atLastMovePoint ? 0 : movePointIndex + 1;
	}

	private void setMovePoint(int index) {
		targetMovePoint = movePoints[index];
		navMeshAgent.SetDestination(targetMovePoint.position);
	}
}
