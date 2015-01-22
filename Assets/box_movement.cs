using UnityEngine;
using System.Collections;

public class box_movement : MonoBehaviour 
{
	public float Speed = 1;
	public float addSpeed = 0;
	public float SpeedMax = 0;
	float lastPos = 0;
	// Update is called once per frame
	void Update () 
    {
		Vector3 BoxPos = gameObject.transform.position;
		gameObject.transform.Translate(0,0,1*Speed*Time.deltaTime);
		if( Speed >= SpeedMax)
		{
			Speed += addSpeed * Time.deltaTime;
		}
        if (BoxPos.z < 0)
        {
            /*Vector3 BoxRotation = new Vector3(0, 0, 0);
            BoxRotation.x = 0;
            BoxRotation.y = 0;
            BoxRotation.z = 0;*/
			BoxPos.x = Random.Range(-1, 1);
			if (BoxPos.x == lastPos)
			{
				BoxPos.x += 1;
			}
			if (BoxPos.x > 1)
			{
				BoxPos.x = -1;
			}
			lastPos = BoxPos.x;
			BoxPos.y = (float)0.5;
            BoxPos.z = 25;
            gameObject.transform.position = BoxPos;
        }
		gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
		/*BoxPos.x = 0;
		BoxPos.y = (float)0.5;
		gameObject.transform.position = BoxPos;*/
	}
}