using UnityEngine;
using System.Collections;

public class MoveCube : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.position = new Vector3 (-10, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (new Vector3 (0.01f, 0, 0));
	}
}
