using UnityEngine;
using System.Collections;
using System;
/// <summary>
/// 脚本功能：A* 过程演示
/// 添加对象：Main Camera
/// 创建时间：2015年7月16日
///          2015年7月18日
/// 知识要点：
/// 1. 游戏算法：A*
/// 2. 接口
/// </summary>

public enum GridType
{
	Normal,     // 常规
	Obstacle,   // 障碍
	Start,      // 起点
	End		    // 终点
}
// 定义格子类，继承可比较接口
public class AGrid : IComparable {
	public int x;
	public int y;
	public int F;  // 路线经由当前格子的总评估值
	public int G;  // 从始点到当前格子的最小消耗值
	public int H;  // 从当前格子到终点的消耗评估值（恒定）
	public GridType gridType; // 格子类型
	public AGrid fatherNode;  // "我从哪里来"
	public bool isCurrent;

	// 实现接口方法：比较（用于排序）
	public int CompareTo (object obj) {
		AGrid g = (AGrid) obj;
		if (this.F < g.F) {
			return -1;
		} else if (this.F > g.F) {
			return 1;
		} else {
			return 0;
		}
	}
}

public class AStar : MonoBehaviour {
	private int col;
	private int row;
	private int xStart; // 始点x坐标
	private int yStart; // 始点y坐标
	private int xEnd;   // 终点x坐标
	private int yEnd;   // 终点y坐标

	public static AGrid[,] map;         // 地图
	
	public static ArrayList openList;  // 开启列表：已探索，且有待继续探索的格子
	public static ArrayList closeList; // 关闭列表：已探索，无需再探索的格子

	public void Init (int row, int col, Vector2 startPos, Vector2 endPos) {
		this.col = col;
		this.row = row;
		this.xStart = (int) startPos.x;
		this.yStart = (int) startPos.y;
		this.xEnd = (int) endPos.x;
		this.yEnd = (int) endPos.y;

		// 初始化地图
		map = new AGrid[row, col];
		for (int i = 0; i < row; i++) {
			for (int j = 0; j < col; j++) {
				map[i, j] = new AGrid();
				map[i, j].x = i;
				map[i, j].y = j;
			}
		}
		map[xStart, yStart].gridType = GridType.Start;
		map[xEnd, yEnd].gridType = GridType.End;

		openList = new ArrayList();
		closeList = new ArrayList();
		// 计算始点H值
		map[xStart, yStart].H = Manhattan(xStart, yStart);
		// 将始点添加到开启列表，准备搜索
		openList.Add (map[xStart, yStart]);
	}

	public void PutObstacle(Vector2[] pos){
		for (int i = 0; i < pos.Length; i++) {
			map[(int)pos[i].x, (int)pos[i].y].gridType = GridType.Obstacle;
		}
	}

	public void StartSearch() {
		NextStep();
	}

	// H值（曼哈顿估算法）
	int Manhattan(int x, int y) {
		return (int)(Mathf.Abs(xEnd - x) + Mathf.Abs(yEnd - y)) * 10;
	}

	// 搜索下一步
	void NextStep() {
		// 如果开启列表中已经无节点，表示搜索完毕
		if (openList.Count == 0) {
			Debug.Log("Over !");
			return;
		}
		// 对开启列表排序(F)
		openList.Sort();

		// 1. 选择第一个格子为当前格，进行搜索
		AGrid grid = (AGrid) openList[0];
		grid.isCurrent = true;
		
		// 2. 判断是否是终点
		if (grid.gridType == GridType.End) {
			Debug.Log("Find Way!!");
			return;
		}
		// 3. 搜索当前格周边格子
		for (int i = -1	; i <= 1; i++) {
			for (int j = -1; j <= 1; j++) {
				if (i != 0 || j != 0) {
					int x = grid.x + i;
					int y = grid.y + j;
					// 如果周边格子不越界且不是障碍且不在关闭列表中
					if (x >= 0 && x < row && y >= 0 && y < col &&
					    map[x, y].gridType != GridType.Obstacle &&
					    !closeList.Contains(map[x, y])){
						// 4. 【重点】重新计算G值并分情况比较
						int g = grid.G + (int) (Mathf.Sqrt(Mathf.Abs(i) +
						                                   Mathf.Abs(j)) * 10);
						if (!openList.Contains(map[x, y])) {
							map[x, y].G = g;
							map[x, y].H = Manhattan(x, y);
							map[x, y].F = map[x, y].G + map[x, y].H;
							// 【重点】记录“我从哪里来”
							map[x, y].fatherNode = grid;
							// 【重点】将新搜索的格子放到开启列表中
							openList.Add(map[x, y]);
						} else if (map[x, y].G > g) {
							map[x, y].G = g;
							map[x, y].F = map[x, y].G + map[x, y].H;
							map[x, y].fatherNode = grid;
						}
					}
				}
			}
		}

		grid.isCurrent = false;
		closeList.Add(grid);
		openList.Remove(grid);
	
	}
	
}
