using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class GameController : MonoBehaviour {
	public int row = 7;
	public int col = 5;
	public Vector2 startPos;
	public Vector2 endPos;
	public Vector2[] obstaclePos;
	
	AStar a;
	GameView view;

	void Start () {
		a = GetComponent<AStar>();
		view = GetComponent<GameView>();

		a.Init(row, col, startPos, endPos);
		a.PutObstacle(obstaclePos);

		view.ShowMap();
		// view.ShowSearchPath();
	}

	void Update () {

		if (Input.GetMouseButtonDown(0)) {
			a.StartSearch();
			view.ShowSearchPath();
		}
	}
	
}
