using UnityEngine;
using System.Collections;

public class boxCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Player")
		{
			GameObject.Find("GameOver").guiText.text = "collision detected";
		}
	}
}
