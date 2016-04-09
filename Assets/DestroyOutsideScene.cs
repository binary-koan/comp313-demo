using UnityEngine;
using System.Collections;

public class DestroyOutsideScene : MonoBehaviour {
	public int minY = -10;
	
	// Update is called once per frame
	void FixedUpdate() {
		if (transform.position.y < minY) {
			Destroy(gameObject);
		}
	}
}
