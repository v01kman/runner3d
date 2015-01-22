using UnityEngine;
using System.Collections;

public class Swipe : MonoBehaviour 
{	
	public float minSwipeDistY;	
	public float minSwipeDistX;	
	private Vector2 startPos;	

	public int MaxXLeft = -1;
	public int MaxXRight = 1;
	public float SideSpeed = 1;

	private Vector3 tr;
	private Vector3 jmp;
	private int target = -1;
	public bool isMoving;
	public bool isJumping;

	private float gravity = -15f;

	void Start ()
	{
		isMoving = false;
	}

	void Update()
	{
		Vector3 CarPos = GameObject.Find("Player").transform.position;

		if (Input.touchCount > 0) 			
		{			
			Touch touch = Input.touches[0];
			switch (touch.phase) 		
			{				
			case TouchPhase.Began:				
				startPos = touch.position;				
				break;			
			case TouchPhase.Ended:

				//vertical swipe
				float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;				
				if (swipeDistVertical > minSwipeDistY) 					
				{					
					float swipeValue = Mathf.Sign(touch.position.y - startPos.y);					
					if (swipeValue > 0)//up swipe						
					{
						if (!isMoving && !isJumping) 
						{
							jmp = new Vector3 (0, 5, 0);
							isMoving = true;
							isJumping = true;
						}
					}
					/*else if (swipeValue < 0)//down swipe							
					{

					}*/
				}		

				//horizontal swipe
				float swipeDistHorizontal = (new Vector3(touch.position.x,0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;
				if (swipeDistHorizontal > minSwipeDistX) 					
				{	
					CarPos = GameObject.Find("Player").transform.position;
					float swipeValue = Mathf.Sign(touch.position.x - startPos.x);					
					if (swipeValue > 0)//right swipe	
					{
						if ((CarPos.x+SideSpeed < 1) && !isMoving)  
						{
							CarPos.x += SideSpeed;
							tr = new Vector3 (-6, 0, 0);
							target = Mathf.RoundToInt(CarPos.x);
							isMoving = true;
						}
					}
					else if (swipeValue < 0)//left swipe
					{	
						if ((CarPos.x-SideSpeed > -3) && !isMoving) 
						{
							CarPos.x -= SideSpeed;
							tr = new Vector3 (6, 0, 0);
							target = Mathf.RoundToInt(CarPos.x);
							isMoving = true;
						}
					}
				}		
				break;
			}
		}

		if(Mathf.Abs(gameObject.transform.position.x - target) < 0.1f)
		{
			tr = new Vector3 (0, 0, 0);
			CarPos.x = Mathf.RoundToInt(CarPos.x);
			gameObject.transform.position = CarPos;
			isMoving = false;
		}
		gameObject.transform.Translate (tr*Time.deltaTime);

		if(isJumping)
		{
			float tempJump = jmp.y;
			tempJump += gravity*Time.deltaTime;
			jmp = new Vector3 (0, tempJump, 0);
		}
		if(gameObject.transform.position.y < 0 && isJumping)
		{
			isJumping = false;
			isMoving = false;
			jmp = new Vector3(0, 0, 0);
			//gameObject.transform.Translate(jmp*Time.deltaTime);
			CarPos.y = 0;
			gameObject.transform.position = CarPos;
		}
		gameObject.transform.Translate (jmp*Time.deltaTime);
	}
}