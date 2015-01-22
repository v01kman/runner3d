var isStart = false;
var isMainMenu = false;
var isQuit=false;

var isPlaying = false;

function OnMouseEnter()
{
	//change text color
	if (!isMainMenu)
	{
		renderer.material.color=Color.red;
		
		/*var pos = Vector3;
		pos = GameObject.Find("Camera").transform.position;
		pos.z -= 5;
		GameObject.Find("Camera").transform.Rotate(0f, 0.5f, 0f);*/
	}
}

function OnMouseExit()
{
	//change color
	if (!isMainMenu)
		renderer.material.color=Color.white;
}

function OnMouseUp()
{
	//is this quit
	if (isQuit==true) 
	{
		//quit the game
		Application.Quit();
	}
	else if (isStart == true)
	{
		GameObject.Find("Start").animation.Play("Move Box Down");
		GameObject.Find("Quit").animation.Play("Move Box Down2");
	}
	else if (isMainMenu)
	{
		//var obj: GameObject = GameObject.Find("Player");
		//obj.GetComponent<Movement>().isPlaying = true;
	}
}

function Update()
{
	/*/quit game if escape key is pressed
	if (Input.GetKey(KeyCode.Escape)) 
	{ 
		Application.Quit();
	}*/
	if (GameObject.Find("Start").transform.position.y < -0.5f && !isPlaying)
	{
		//isPlaying = true;
		GameObject.Find("PlayerContainer").animation.Play("Move Player Container");
		//GameObject.Find("Camera").animation.Play("Move Camera");
	}	
	if (GameObject.Find("PlayerContainer").transform.position.x < 4.5f && !isPlaying)
	{
		isPlaying = true;
		GameObject.Find("Camera").animation.Play("Move Camera");
	}
}