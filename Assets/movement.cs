using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour
{
	public GUIText ScoreText;
	public GUIText CoinText;
	public int AddScore = 0;
	public float Speed = 1;
	public float addSpeed = 0;
	public float SpeedMax = 0;
	public float SideSpeed = 1;
	public int MaxXLeft = -1;
	public int MaxXRight = 1;
	public Vector3 spawnLocation;
	public GameObject myCube;
	public GameObject myCoin;
	public GameObject myFence;
	public bool isPlaying = false;

	public float score = 0;
	public int coin = 0;

	public GameObject[,] FirstPart;
	public GameObject[,] SecondPart;
	public GameObject[] parts;
	public int nextParts;
	public float FirstPosition;
	public float SecondPosition;
	public int ObjectsCount;
	public GameObject clone;

	public int CurrentScore;
	public bool check;

	private Vector3 tr;
	private Vector3 jmp;
	private int target;
	public bool isMoving;
	public bool isJumping;

	private float gravity = -15f;
	
	void Start ()
	{
		target = -3;
		isMoving = false;
		isJumping = false;
		check = true;
		ObjectsCount = 30;
		FirstPart = new GameObject[ObjectsCount, 3];
		SecondPart = new GameObject[ObjectsCount, 3];
		parts = new GameObject[10];
		parts[0] = GameObject.Find("ParticleSystem");
		parts[1] = GameObject.Find("ParticleSystem1");
		parts[2] = GameObject.Find("ParticleSystem2");
		parts[3] = GameObject.Find("ParticleSystem3");
		parts[4] = GameObject.Find("ParticleSystem4");
		parts[5] = GameObject.Find("ParticleSystem5");
		parts[6] = GameObject.Find("ParticleSystem6");
		parts[7] = GameObject.Find("ParticleSystem7");
		parts[8] = GameObject.Find("ParticleSystem8");
		parts[9] = GameObject.Find("ParticleSystem9");
		nextParts = 0;
		FirstPosition = 30f;
		SecondPosition = FirstPosition + (float)ObjectsCount;
		FillFirst ();
		FillSecond ();
	}

	void Update ()
	{
		Speed = 1 + score / 5000;
		if (GameObject.Find ("Camera").transform.position.x == 0 && check) 
		{
			isPlaying = true;
			
			//GameObject.Find("Player").GetComponent<movement>().isPlaying = true;
			GameObject.Find("Distance Text").guiText.enabled = true;
			GameObject.Find("Scores Text").guiText.enabled = true;
			GameObject.Find("Coin Text").guiText.enabled = true;
			GameObject.Find("Menu Text").guiText.enabled = true;

			check = false;
		}

		//move player and objects
		if (isPlaying) 
		{
			MoveParts();
			if (FirstPosition < -ObjectsCount)
			{
				ClearFirst();
				FirstPosition = SecondPosition + (float)ObjectsCount;
				FillFirst();
			}
			if(SecondPosition < -ObjectsCount)
			{
				ClearSecond();
				SecondPosition = FirstPosition + (float)ObjectsCount;
				FillSecond();
			}


			//player movement here
			//Vector3 CarPos = gameObject.transform.position;
			Vector3 CarPos = GameObject.Find("Player").transform.position;

			if (Input.GetKeyDown (KeyCode.D) && (CarPos.x+SideSpeed < 1) && !isMoving)  
			{
				CarPos.x += SideSpeed;
				tr = new Vector3 (-6, 0, 0);
				target = Mathf.RoundToInt(CarPos.x);
				isMoving = true;
			}
			if (Input.GetKeyDown (KeyCode.A) && (CarPos.x-SideSpeed > -3) && !isMoving) 
			{
				CarPos.x -= SideSpeed;
				tr = new Vector3 (6, 0, 0);
				target = Mathf.RoundToInt(CarPos.x);
				isMoving = true;
			}

			if (Input.GetKeyDown (KeyCode.W) && !isMoving && !isJumping) 
			{
				GameObject.Find("Player").animation.Stop("steveRun");
				GameObject.Find("Player").animation.Play("steveJump");
				jmp = new Vector3 (0, 5, 0);
				isMoving = true;
				isJumping = true;
			}
			if(isJumping)
			{
				float tempJump = jmp.y;
				tempJump += gravity*Time.deltaTime;
				jmp = new Vector3 (0, tempJump, 0);
			}

			if(Mathf.Abs(gameObject.transform.position.x - target) < 0.1f)
			{
				tr = new Vector3 (0, 0, 0);
				//gameObject.transform.Translate (tr*Time.deltaTime);
				CarPos.x = Mathf.RoundToInt(CarPos.x);
				gameObject.transform.position = CarPos;
				isMoving = false;
			}
			gameObject.transform.Translate (tr*Time.deltaTime);

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

			//scores
			score += AddScore * Speed * Time.deltaTime / 3f;
			CurrentScore = (int)score;
			ScoreText.text = CurrentScore.ToString ();
		}
	}

	void OnTriggerEnter(Collider collision) 
	{
		if (collision.gameObject.name == "CoinFirst" || collision.gameObject.name == "CoinSecond") 
		{
			Destroy (collision.gameObject);

			parts[nextParts].transform.position = new Vector3(GameObject.Find("Player").transform.position.x+0.75f, 0.55f, 2.75f);
			parts[nextParts].particleSystem.Play();
			nextParts++;
			if (nextParts > 9)
			{
				nextParts = 0;
			}

			//GameObject.Find("ParticleSystem").particleSystem.Play();
			coin++;
			CoinText.text = coin.ToString();
		}
		
		if (collision.gameObject.name == "BlockFirst" || collision.gameObject.name == "BlockSecond") 
		{
			GameObject.Find("GameOver Text").guiText.text = "BLOCK";
		}
		
		if (collision.gameObject.name == "FenceFirst" || collision.gameObject.name == "FenceSecond") 
		{
			GameObject.Find("GameOver Text").guiText.text = "FENCE";
		}
	}

	public void FillFirst()
	{
		spawnLocation = new Vector3 (-1, 0.5f, FirstPosition);
		clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;				
		clone.name = "CoinFirst";
		clone.tag = "First";
		FirstPart [0, 0] = clone;

		spawnLocation = new Vector3 (0, 0.5f, FirstPosition);
		clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;				
		clone.name = "CoinFirst";
		clone.tag = "First";
		FirstPart [0, 1] = clone;

		spawnLocation = new Vector3 (1, 0.5f, FirstPosition);
		clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;				
		clone.name = "CoinFirst";
		clone.tag = "First";
		FirstPart [0, 2] = clone;

		spawnLocation = new Vector3 (-1, 0.5f, FirstPosition+ObjectsCount-1);
		clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;				
		clone.name = "CoinFirst";
		clone.tag = "First";
		FirstPart [ObjectsCount-1, 0] = clone;

		spawnLocation = new Vector3 (0, 0.5f, FirstPosition+ObjectsCount-1);
		clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;				
		clone.name = "CoinFirst";
		clone.tag = "First";
		FirstPart [ObjectsCount-1, 1] = clone;

		spawnLocation = new Vector3 (1, 0.5f, FirstPosition+ObjectsCount-1);
		clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;				
		clone.name = "CoinFirst";
		clone.tag = "First";
		FirstPart [ObjectsCount-1, 2] = clone;

		int lastObstruction = -1;
		int lastCoin = -1;
		for(int i=1; i<ObjectsCount-1; i++)
		{
			int rnd = Random.Range(0, 4);
			switch (rnd)
			{
			case 0: //space
				break;
			case 1: //block
				if (i > lastObstruction + 3)
				{
					int rndPos = Random.Range(0, 6);
					int rndCount = Random.Range(0, 5);
					rndCount++;
					if ((i + rndCount) > (ObjectsCount - 2))
					{
						rndCount = ObjectsCount - 2 - i;
					}
					lastObstruction = i + rndCount;

					switch (rndPos)
					{
					case 0: //left
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (-1, 0.5f, FirstPosition+j);
							clone = Instantiate (myCube, spawnLocation, Quaternion.identity) as GameObject;				
							clone.name = "BlockFirst";
							clone.tag = "First";
							FirstPart [j, 0] = clone;
						}
						break;
					case 1: //mid
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (0, 0.5f, FirstPosition+j);
							clone = Instantiate (myCube, spawnLocation, Quaternion.identity) as GameObject;				
							clone.name = "BlockFirst";
							clone.tag = "First";
							FirstPart [j, 1] = clone;
						}
						break;
					case 2: //right
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (1, 0.5f, FirstPosition+j);
							clone = Instantiate (myCube, spawnLocation, Quaternion.identity) as GameObject;				
							clone.name = "BlockFirst";
							clone.tag = "First";
							FirstPart [j, 2] = clone;
						}
						break;
					case 3: //left+mid
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (-1, 0.5f, FirstPosition+j);
							clone = Instantiate (myCube, spawnLocation, Quaternion.identity) as GameObject;				
							clone.name = "BlockFirst";
							clone.tag = "First";
							FirstPart [j, 0] = clone;

							spawnLocation = new Vector3 (0, 0.5f, FirstPosition+j);
							clone = Instantiate (myCube, spawnLocation, Quaternion.identity) as GameObject;				
							clone.name = "BlockFirst";
							clone.tag = "First";
							FirstPart [j, 1] = clone;
						}
						break;
					case 4: //left+right
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (-1, 0.5f, FirstPosition+j);
							clone = Instantiate (myCube, spawnLocation, Quaternion.identity) as GameObject;				
							clone.name = "BlockFirst";
							clone.tag = "First";
							FirstPart [j, 0] = clone;
							
							spawnLocation = new Vector3 (1, 0.5f, FirstPosition+j);
							clone = Instantiate (myCube, spawnLocation, Quaternion.identity) as GameObject;				
							clone.name = "BlockFirst";
							clone.tag = "First";
							FirstPart [j, 2] = clone;
						}
						break;
					case 5: //mid+right
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (1, 0.5f, FirstPosition+j);
							clone = Instantiate (myCube, spawnLocation, Quaternion.identity) as GameObject;				
							clone.name = "BlockFirst";
							clone.tag = "First";
							FirstPart [j, 2] = clone;
							
							spawnLocation = new Vector3 (0, 0.5f, FirstPosition+j);
							clone = Instantiate (myCube, spawnLocation, Quaternion.identity) as GameObject;					
							clone.name = "BlockFirst";
							clone.tag = "First";
							FirstPart [j, 1] = clone;
						}
						break;
					}
				}
				break;
			case 2: //coin
				if (i > lastCoin)
				{
					int rndPos = Random.Range(0, 6);
					int rndCount = Random.Range(0, 5);
					rndCount++;
					if ((i + rndCount) > (ObjectsCount - 2))
					{
						rndCount = ObjectsCount - 2 - i;
					}
					lastCoin = i + rndCount;
					
					switch (rndPos)
					{
					case 0: //left
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (-1, 0.5f, FirstPosition+j);
							clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "CoinFirst";
							clone.tag = "First";
							FirstPart [j, 0] = clone;
						}
						break;
					case 1: //mid
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (0, 0.5f, FirstPosition+j);
							clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "CoinFirst";
							clone.tag = "First";
							FirstPart [j, 1] = clone;
						}
						break;
					case 2: //right
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (1, 0.5f, FirstPosition+j);
							clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "CoinFirst";
							clone.tag = "First";
							FirstPart [j, 2] = clone;
						}
						break;
					case 3: //left+mid
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (-1, 0.5f, FirstPosition+j);
							clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "CoinFirst";
							clone.tag = "First";
							FirstPart [j, 0] = clone;
							
							spawnLocation = new Vector3 (0, 0.5f, FirstPosition+j);
							clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "CoinFirst";
							clone.tag = "First";
							FirstPart [j, 1] = clone;
						}
						break;
					case 4: //left+right
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (-1, 0.5f, FirstPosition+j);
							clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "CoinFirst";
							clone.tag = "First";
							FirstPart [j, 0] = clone;
							
							spawnLocation = new Vector3 (1, 0.5f, FirstPosition+j);
							clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "CoinFirst";
							clone.tag = "First";
							FirstPart [j, 2] = clone;
						}
						break;
					case 5: //mid+right
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (1, 0.5f, FirstPosition+j);
							clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "CoinFirst";
							clone.tag = "First";
							FirstPart [j, 2] = clone;
							
							spawnLocation = new Vector3 (0, 0.5f, FirstPosition+j);
							clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "CoinFirst";
							clone.tag = "First";
							FirstPart [j, 1] = clone;
						}
						break;
					}
				}
				break;
			case 3: //fence
				if (i > lastObstruction + 4)
				{
					lastObstruction = i;

					spawnLocation = new Vector3 (-1, 0.25f, FirstPosition+i);
					clone = Instantiate (myFence, spawnLocation, Quaternion.identity) as GameObject;					
					clone.name = "FenceFirst";
					clone.tag = "First";
					FirstPart [i, 0] = clone;

					spawnLocation = new Vector3 (0, 0.25f, FirstPosition+i);
					clone = Instantiate (myFence, spawnLocation, Quaternion.identity) as GameObject;					
					clone.name = "FenceFirst";
					clone.tag = "First";
					FirstPart [i, 1] = clone;

					spawnLocation = new Vector3 (1, 0.25f, FirstPosition+i);
					clone = Instantiate (myFence, spawnLocation, Quaternion.identity) as GameObject;					
					clone.name = "FenceFirst";
					clone.tag = "First";
					FirstPart [i, 2] = clone;
				}
				break;
			}
		}
	}

	public void FillSecond()
	{
		spawnLocation = new Vector3 (-1, 0.5f, SecondPosition);
		clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;		
		clone.name = "CoinSecond";
		clone.tag = "Second";
		SecondPart [0, 0] = clone;
		
		spawnLocation = new Vector3 (0, 0.5f, SecondPosition);
		clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;		
		clone.name = "CoinSecond";
		clone.tag = "Second";
		SecondPart [0, 1] = clone;
		
		spawnLocation = new Vector3 (1, 0.5f, SecondPosition);
		clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;		
		clone.name = "CoinSecond";
		clone.tag = "Second";
		SecondPart [0, 2] = clone;
		
		spawnLocation = new Vector3 (-1, 0.5f, SecondPosition+ObjectsCount-1);
		clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;		
		clone.name = "CoinSecond";
		clone.tag = "Second";
		SecondPart [ObjectsCount-1, 0] = clone;
		
		spawnLocation = new Vector3 (0, 0.5f, SecondPosition+ObjectsCount-1);
		clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;		
		clone.name = "CoinSecond";
		clone.tag = "Second";
		SecondPart [ObjectsCount-1, 1] = clone;
		
		spawnLocation = new Vector3 (1, 0.5f, SecondPosition+ObjectsCount-1);
		clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;		
		clone.name = "CoinSecond";
		clone.tag = "Second";
		SecondPart [ObjectsCount-1, 2] = clone;
	
		int lastObstruction = -1;
		int lastCoin = -1;
		for(int i=1; i<ObjectsCount-1; i++)
		{
			int rnd = Random.Range(0, 4);
			switch (rnd)
			{
			case 0: //space
				break;
			case 1: //block
				if (i > lastObstruction + 3)
				{
					int rndPos = Random.Range(0, 6);
					int rndCount = Random.Range(0, 5);
					rndCount++;
					if ((i + rndCount) > (ObjectsCount - 2))
					{
						rndCount = ObjectsCount - 2 - i;
					}
					lastObstruction = i + rndCount;
					
					switch (rndPos)
					{
					case 0: //left
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (-1, 0.5f, SecondPosition+j);
							clone = Instantiate (myCube, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "BlockSecond";
							clone.tag = "Second";
							SecondPart [j, 0] = clone;
						}
						break;
					case 1: //mid
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (0, 0.5f, SecondPosition+j);
							clone = Instantiate (myCube, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "BlockSecond";
							clone.tag = "Second";
							SecondPart [j, 1] = clone;
						}
						break;
					case 2: //right
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (1, 0.5f, SecondPosition+j);
							clone = Instantiate (myCube, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "BlockSecond";
							clone.tag = "Second";
							SecondPart [j, 2] = clone;
						}
						break;
					case 3: //left+mid
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (-1, 0.5f, SecondPosition+j);
							clone = Instantiate (myCube, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "BlockSecond";
							clone.tag = "Second";
							SecondPart [j, 0] = clone;
							
							spawnLocation = new Vector3 (0, 0.5f, SecondPosition+j);
							clone = Instantiate (myCube, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "BlockSecond";
							clone.tag = "Second";
							SecondPart [j, 1] = clone;
						}
						break;
					case 4: //left+right
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (-1, 0.5f, SecondPosition+j);
							clone = Instantiate (myCube, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "BlockSecond";
							clone.tag = "Second";
							SecondPart [j, 0] = clone;
							
							spawnLocation = new Vector3 (1, 0.5f, SecondPosition+j);
							clone = Instantiate (myCube, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "BlockSecond";
							clone.tag = "Second";
							SecondPart [j, 2] = clone;
						}
						break;
					case 5: //mid+right
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (1, 0.5f, SecondPosition+j);
							clone = Instantiate (myCube, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "BlockSecond";
							clone.tag = "Second";
							SecondPart [j, 2] = clone;
							
							spawnLocation = new Vector3 (0, 0.5f, SecondPosition+j);
							clone = Instantiate (myCube, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "BlockSecond";
							clone.tag = "Second";
							SecondPart [j, 1] = clone;
						}
						break;
					}
				}
				break;
			case 2: //coin
				if (i > lastCoin + 2)
				{
					int rndPos = Random.Range(0, 6);
					int rndCount = Random.Range(0, 5);
					rndCount++;
					if ((i + rndCount) > (ObjectsCount - 2))
					{
						rndCount = ObjectsCount - 2 - i;
					}
					lastCoin = i + rndCount;
					
					switch (rndPos)
					{
					case 0: //left
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (-1, 0.5f, SecondPosition+j);
							clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "CoinSecond";
							clone.tag = "Second";
							SecondPart [j, 0] = clone;
						}
						break;
					case 1: //mid
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (0, 0.5f, SecondPosition+j);
							clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "CoinSecond";
							clone.tag = "Second";
							SecondPart [j, 1] = clone;
						}
						break;
					case 2: //right
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (1, 0.5f, SecondPosition+j);
							clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "CoinSecond";
							clone.tag = "Second";
							SecondPart [j, 2] = clone;
						}
						break;
					case 3: //left+mid
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (-1, 0.5f, SecondPosition+j);
							clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "CoinSecond";
							clone.tag = "Second";
							SecondPart [j, 0] = clone;
							
							spawnLocation = new Vector3 (0, 0.5f, SecondPosition+j);
							clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "CoinSecond";
							clone.tag = "Second";
							SecondPart [j, 1] = clone;
						}
						break;
					case 4: //left+right
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (-1, 0.5f, SecondPosition+j);
							clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "CoinSecond";
							clone.tag = "Second";
							SecondPart [j, 0] = clone;
							
							spawnLocation = new Vector3 (1, 0.5f, SecondPosition+j);
							clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "CoinSecond";
							clone.tag = "Second";
							SecondPart [j, 2] = clone;
						}
						break;
					case 5: //mid+right
						for(int j=i; j<(i+rndCount); j++)
						{
							spawnLocation = new Vector3 (1, 0.5f, SecondPosition+j);
							clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "CoinSecond";
							clone.tag = "Second";
							SecondPart [j, 2] = clone;
							
							spawnLocation = new Vector3 (0, 0.5f, SecondPosition+j);
							clone = Instantiate (myCoin, spawnLocation, Quaternion.identity) as GameObject;
							clone.name = "CoinSecond";
							clone.tag = "Second";
							SecondPart [j, 1] = clone;
						}
						break;
					}
				}
				break;
			case 3: //fence
				if (i > lastObstruction + 4)
				{
					lastObstruction = i;

					spawnLocation = new Vector3 (-1, 0.25f, SecondPosition+i);
					clone = Instantiate (myFence, spawnLocation, Quaternion.identity) as GameObject;
					clone.name = "FenceSecond";
					clone.tag = "Second";
					SecondPart [i, 0] = clone;
					
					spawnLocation = new Vector3 (0, 0.25f, SecondPosition+i);
					clone = Instantiate (myFence, spawnLocation, Quaternion.identity) as GameObject;
					clone.name = "FenceSecond";
					clone.tag = "Second";
					SecondPart [i, 1] = clone;
					
					spawnLocation = new Vector3 (1, 0.25f, SecondPosition+i);
					clone = Instantiate (myFence, spawnLocation, Quaternion.identity) as GameObject;
					clone.name = "FenceSecond";
					clone.tag = "Second";
					SecondPart [i, 2] = clone;
				}
				break;
			}
		}
	}

	public void ClearFirst()
	{
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("First");		
		for(int i=0 ; i<gameObjects.Length ; i++)
		{
			Destroy(gameObjects[i]);
		}
	}
	
	public void ClearSecond()
	{
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("Second");		
		for(int i=0 ; i<gameObjects.Length ; i++)
		{
			Destroy(gameObjects[i]);
		}
	}

	void MoveParts()
	{
		FirstPosition -= 0.1f * Speed;
		SecondPosition -= 0.1f * Speed;
		for(int i=0; i<ObjectsCount; i++)
		{
			for(int j=0; j<3; j++)
			{
				if(FirstPart[i, j] != null)
				{
					Vector3 cubePos = FirstPart[i, j].transform.position;
					cubePos.z = FirstPosition + i;
					FirstPart[i, j].transform.position = cubePos;
				}

				if(SecondPart[i, j] != null)
				{
					Vector3 cubePos = SecondPart[i, j].transform.position;
					cubePos.z = SecondPosition + i;
					SecondPart[i, j].transform.position = cubePos;
				}
			}
		}
	}
}