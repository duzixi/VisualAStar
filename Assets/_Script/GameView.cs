using UnityEngine;
using System.Collections;

public class GameView : MonoBehaviour {

	public GameObject normalPref;
	public GameObject obstaclePref;
	private int row;
	private int col;
	public Color start;
	public Color end;
	public Color closed;
	public Color opend;
	public Color current;


	// Use this for initialization
	void Awake () {
		row = GetComponent<GameController>().row;
		col = GetComponent<GameController>().col;
	}

	public void ShowMap() {
		for (int i = 0; i < row; i++) {
			for (int j = 0; j < col; j++) {
				GameObject g = Instantiate(normalPref, new Vector3(i, 0, j), Quaternion.identity) as GameObject;
				g.name = i + "" + j;
				if (AStar.map[i, j].gridType == GridType.Obstacle) {
					GameObject o = Instantiate(obstaclePref, new Vector3(i, 1, j), Quaternion.identity) as GameObject;
				}
				if (AStar.map[i, j].gridType == GridType.Start) {
					g.renderer.material.color = start;
				} else if (AStar.map[i, j].gridType == GridType.End) {
					g.renderer.material.color = end;
				}
			}
		}
	}

	public void ShowSearchPath() {
		// close list

		if (AStar.closeList.Count != 0) {
			foreach (AGrid grid in AStar.closeList) {
				GameObject g = GameObject.Find(grid.x + "" + grid.y);
				g.renderer.material.color = closed;
			}

		}

		// open list
		foreach (AGrid grid in AStar.openList) {

			GameObject g = GameObject.Find(grid.x + "" + grid.y);
			DrawPath(g, grid);
			g.renderer.material.color = opend;
			g.transform.GetChild(0).GetComponent<TextMesh>().text = "" + grid.F;
			g.transform.GetChild(1).GetComponent<TextMesh>().text = "" + grid.G;
			g.transform.GetChild(2).GetComponent<TextMesh>().text = "" + grid.H;
 
		}
	}

	private void DrawPath(GameObject g, AGrid grid) {
		if (grid.fatherNode != null) {
			Vector3 startPos = new Vector3(grid.x, 0.5f, grid.y);
			Vector3 endPos = new Vector3(grid.fatherNode.x, 0.5f, grid.fatherNode.y );

			LineRenderer l = g.GetComponent<LineRenderer>();
			l.SetPosition(0, Vector3.up);
			l.SetPosition(1, (endPos - startPos) + Vector3.up);

		}
	}

}
