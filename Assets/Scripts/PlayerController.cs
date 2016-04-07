using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {
    public float speed;
    public Text scoreText;
    public Text winText;

    private Rigidbody physicsBody;
    private int score;

    void Start() {
        physicsBody = GetComponent<Rigidbody>();
        updateScore(0);
        winText.text = "";
    }

    void FixedUpdate() {
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        var movement = new Vector3(moveHorizontal, 0, moveVertical) * speed;

        physicsBody.AddForce(movement);
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Pickup")) {
            other.gameObject.SetActive(false);
            updateScore(score + 1);
        }
    }

    private void updateScore(int newScore) {
        score = newScore;
        scoreText.text = "Score: " + score;

        if (score >= 5) {
            winText.text = "You won!";
        }
    }
}
