using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
	public bool isStart = false;
	public bool isMainMenu = false;
	public bool isContinue = false;
	public bool isGoMainMenu = false;
	public bool isQuit=false;	

	bool isPlaying = false;
	
	void Start () 
	{	
		GameObject.Find("Distance Text").guiText.enabled = false;
		GameObject.Find("Scores Text").guiText.enabled = false;
		GameObject.Find("Coin Text").guiText.enabled = false;
		GameObject.Find("Menu Text").guiText.enabled = false;
		GameObject.Find ("Continue Text").guiText.enabled = false;
		GameObject.Find("Go Main Menu Text").guiText.enabled = false;
	}
	
	void Update () 
	{	
		if (GameObject.Find("Start").transform.position.y < -0.5f && !isPlaying)
		{
			GameObject.Find("Player Container").animation.Play("Move Player");
		}	
		if (GameObject.Find("Player").transform.position.x < 4.5f && !isPlaying)
		{
			isPlaying = true;
			GameObject.Find("Camera").animation.Play("Move Camera");
		}
	}
	
	void OnMouseEnter()
	{
		//change text color
		if (!isMainMenu)
		{
			renderer.material.color=Color.red;
		}
	}
	
	void OnMouseExit()
	{
		//change color
		if (!isMainMenu)
			renderer.material.color=Color.white;
	}
	
	void OnMouseUp()
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

			GameObject.Find("Player").GetComponent<movement>().isPlaying = true;
			GameObject.Find("Player").GetComponent<movement>().isMoving = false;
			GameObject.Find("Player").GetComponent<Swipe>().isMoving = false;
			GameObject.Find("Player").GetComponent<movement>().check = true;
			GameObject.Find("Player").GetComponent<movement>().FirstPosition = 40f;
			GameObject.Find("Player").GetComponent<movement>().SecondPosition = GameObject.Find("Player").GetComponent<movement>().FirstPosition + (float)GameObject.Find("Player").GetComponent<movement>().ObjectsCount;			
			GameObject.Find("Player").GetComponent<movement>().ClearFirst();
			GameObject.Find("Player").GetComponent<movement>().ClearSecond();			
			GameObject.Find("Player").GetComponent<movement>().FillFirst();
			GameObject.Find("Player").GetComponent<movement>().FillSecond();	
			GameObject.Find("Player").GetComponent<movement>().coin = 0;
			GameObject.Find("Player").GetComponent<movement>().CoinText.guiText.text = "0";
			GameObject.Find("Player").GetComponent<movement>().score = 0;
			GameObject.Find("Player").GetComponent<movement>().CurrentScore = 0;
			GameObject.Find("Player").GetComponent<movement>().ScoreText.guiText.text = "0";

			if (GameObject.Find("Player").transform.position.z == 3)
			{
				GameObject.Find("Distance Text").guiText.enabled = true;
				GameObject.Find("Scores Text").guiText.enabled = true;
				GameObject.Find("Coin Text").guiText.enabled = true;
				GameObject.Find("Menu Text").guiText.enabled = true;
			}
		}
		else if (isMainMenu)
		{
			GameObject.Find("Player").GetComponent<movement>().isPlaying = false;
			GameObject.Find("Distance Text").guiText.enabled = false;
			GameObject.Find("Scores Text").guiText.enabled = false;
			GameObject.Find("Coin Text").guiText.enabled = false;
			GameObject.Find("Menu Text").guiText.enabled = false;

			GameObject.Find("Continue Text").guiText.enabled = true;
			GameObject.Find("Go Main Menu Text").guiText.enabled = true;
			GameObject.Find("Player").animation.Stop("Take 001");
		}
		else if (isGoMainMenu)
		{			
			GameObject.Find("Player").GetComponent<movement>().ClearFirst();
			GameObject.Find("Player").GetComponent<movement>().ClearSecond();

			GameObject.Find("Distance Text").guiText.enabled = false;
			GameObject.Find("Scores Text").guiText.enabled = false;
			GameObject.Find("Coin Text").guiText.enabled = false;
			GameObject.Find("Menu Text").guiText.enabled = false;		
			GameObject.Find("Continue Text").guiText.enabled = false;
			GameObject.Find("Go Main Menu Text").guiText.enabled = false;

			Vector3 pos = new Vector3(6f, 1.3f, -3f);
			GameObject.Find("Camera").transform.position = pos;
			pos = new Vector3(4f, 0.5f, -8f);
			GameObject.Find("Start").transform.position = pos;
			pos = new Vector3(1.5f, 0.5f, -5.5f);
			GameObject.Find("Quit").transform.position = pos;
			GameObject.Find("Camera").transform.Rotate(-25f, 0f, 0f);
			GameObject.Find("Camera").transform.Rotate(0f, 220f, 0f);
			pos = new Vector3(7.1f, 0f, -10f);
			GameObject.Find("Player Container").transform.position = pos;
			GameObject.Find("Player").transform.position = pos;
			GameObject.Find("Player").animation.Play("Take 001");

			isPlaying = false;
		}
		else if (isContinue)
		{			
			GameObject.Find("Continue Text").guiText.enabled = false;
			GameObject.Find("Go Main Menu Text").guiText.enabled = false;

			GameObject.Find("Player").GetComponent<movement>().isPlaying = true;
			GameObject.Find("Distance Text").guiText.enabled = true;
			GameObject.Find("Scores Text").guiText.enabled = true;
			GameObject.Find("Coin Text").guiText.enabled = true;
			GameObject.Find("Menu Text").guiText.enabled = true;
			GameObject.Find("Player").animation.Play("Take 001");
		}
	}
}