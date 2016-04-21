using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCStandardMovement : MonoBehaviour {
    public enum State {
        MOVING, FOLLOWING_PLAYER, PAUSED
    }

    public GameObject player;
    public Transform[] movePoints;
	public float speed = 5;

	private const float STOPPING_DISTANCE_THRESHOLD = 0.2f;
    private const float PAUSE_SECONDS = 2;

    private State state = State.MOVING;
    private float pausedTime = 0;

	private Transform targetMovePoint;
	private NavMeshAgent navMeshAgent;
    private Animator modelAnimator;

	void Start() {
		navMeshAgent = GetComponent<NavMeshAgent>();
        modelAnimator = GetComponentInChildren<Animator>();
        modelAnimator.SetFloat("MovementSpeed", speed);
		setMovePoint(0);
	}

	void Update() {
        switch (state) {
        case State.MOVING:
            moveOnPath();
            break;
        case State.FOLLOWING_PLAYER:
            moveTowardsPlayer();
            break;
        case State.PAUSED:
            maybeResume();
            break;
        }
	}

	void OnCollisionEnter(Collision hit) {
		if (hit.gameObject.CompareTag("Pickup")) {
            SetState(State.PAUSED);
		}
	}

    public void SetState(State newState) {
        if (state == newState) {
            return;
        }

        state = newState;

        switch (state) {
        case State.MOVING:
            navMeshAgent.Resume();
            setNextMovePoint();
            break;

        case State.FOLLOWING_PLAYER:
            navMeshAgent.Resume();
            break;

        case State.PAUSED:
            pausedTime = 0;

            navMeshAgent.Stop();
            modelAnimator.SetBool("IsMoving", false);
            break;
        }
    }

    private void moveOnPath() {
        if (Input.GetKeyDown(KeyCode.F)) {
            SetState(State.FOLLOWING_PLAYER);
        }
        else if (atTargetMovePoint()) {
            setNextMovePoint();
        }
    }

    private void moveTowardsPlayer() {
        if (Input.GetKeyDown(KeyCode.F)) {
            SetState(State.MOVING);
        }
        else {
            navMeshAgent.destination = player.transform.position;
        }
    }

    private void maybeResume() {
        pausedTime += Time.deltaTime;

        if (pausedTime >= PAUSE_SECONDS) {
            SetState(State.MOVING);
        }
    }

	private bool atTargetMovePoint() {
		var xDistance = Math.Abs(transform.position.x - targetMovePoint.position.x);
		var zDistance = Math.Abs(transform.position.z - targetMovePoint.position.z);
		var threshold = navMeshAgent.stoppingDistance + STOPPING_DISTANCE_THRESHOLD;

		return xDistance <= threshold && zDistance <= threshold;
	}

	private void setNextMovePoint() {
		var movePointIndex = Array.IndexOf(movePoints, targetMovePoint);
		var atLastMovePoint = (movePointIndex == movePoints.Length - 1);

		setMovePoint(atLastMovePoint ? 0 : movePointIndex + 1);
	}

	private void setMovePoint(int index) {
		targetMovePoint = movePoints[index];

        navMeshAgent.destination = targetMovePoint.position;
        modelAnimator.SetBool("IsMoving", true);
    }

    private float distanceToPlayer() {
        return (player.transform.position - transform.position).magnitude;
    }
}
