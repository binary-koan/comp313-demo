using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCPredictiveMovement : MonoBehaviour {
    public Transform partnerTransform;
    public Transform playerTransform;

    private NavMeshAgent navMeshAgent;
    private Animator animator;

    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        animator.SetBool("IsMoving", true);
        animator.SetFloat("MovementSpeed", navMeshAgent.speed * 1.5f);
    }

    void Update() {
        // The idea with this positioning is that this NPC will try to get on the opposite side of the
        // player to its partner - as the partner moves closer to the player the NPC will move closer
        // on the other side so that they trap the player with a pincer movement
        var targetOffset = -(partnerTransform.position - playerTransform.position);

        navMeshAgent.SetDestination(playerTransform.position + targetOffset);
    }

    void OnCollisionEnter(Collision hit) {
        if (hit.gameObject.CompareTag("Pickup")) {
            PauseMovement();
        }
    }

    public void PauseMovement() {
        navMeshAgent.Stop();

        animator.SetBool("IsMoving", false);
        StartCoroutine(resumeAfterPause());
    }

    private IEnumerator resumeAfterPause() {
        yield return new WaitForSeconds(2);

        animator.SetBool("IsMoving", true);
        navMeshAgent.Resume();
    }
}
