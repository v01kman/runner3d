using UnityEngine;
using System.Collections;

public class goright : MonoBehaviour {

	private bool isPressed = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		foreach (Touch touch in Input.touches)
		{
			if (guiTexture.HitTest(touch.position) && touch.phase != TouchPhase.Ended)
			{
				if (!isPressed)
				{
					Vector3 CarPos = GameObject.Find("Player").transform.position;
					if(CarPos.x < 1)
					{
						CarPos.x += 1;
						GameObject.Find("Player").transform.position = CarPos;
					}
					isPressed = true;
				}
			}
			else if (guiTexture.HitTest(touch.position) && touch.phase == TouchPhase.Ended)
			{
				isPressed = false;
			}
		}
	}
}
