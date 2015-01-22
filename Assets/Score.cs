using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
	public GUIText ScoreText;
	private int CurrentScore;
	public float score = 0;
	public int AddScore = 0;
	void Update () 
	{
	score += AddScore*Time.deltaTime;
		CurrentScore = (int)score;
		ScoreText.text = CurrentScore.ToString();
	}
}