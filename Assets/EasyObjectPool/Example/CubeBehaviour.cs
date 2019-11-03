using UnityEngine;
using System.Collections;

public class CubeBehaviour : MonoBehaviour {

	// Object pool plays with the active state of the object. Reset all values in OnEnable/OnDisable
	void OnEnable() {
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
	}

}
