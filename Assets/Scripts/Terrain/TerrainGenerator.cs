using UnityEngine;
using System.Collections;

enum TileType { none, ground, ladder, goalArea };

public class TerrainGenerator : MonoBehaviour {

	public GameObject tile, ladderTile, goalArea;
	public GameObject corner, oneSidedHorizontal, threeSided, middle;
	public Sprite map;

	private int rows, columns;
	private TileType[,] tileData;
	private int scaleSize;

	void LoadFromMap()
	{ 
		rows = map.texture.width;
		columns = map.texture.height;

		tileData = new TileType[rows, columns];

		// Once this is done, we'll fill it with 1s and 0s representing blocks and non-blocks,
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < columns; j++)
			{
				Color pixelColor = map.texture.GetPixel(i, j);
				if (pixelColor == new Color(1, 1, 1))
						tileData[i, j] = TileType.ground;
				else if (pixelColor == new Color(1, 0, 0))
						tileData[i, j] = TileType.ladder;
				else if (pixelColor == new Color(0, 0, 0))
						tileData[i, j] = TileType.none;
				else if (pixelColor == new Color(0, 1, 0))
						BuildGoalArea(0, i, j);
				else if (pixelColor == new Color(0, 0, 1))
						BuildGoalArea(1, i, j);
				else
				{
					tileData[i, j] = TileType.none;
					print("non-assigned color found");
					print(pixelColor);
				}
			}
		}
	}

	GameObject InstantiateAtPosition(GameObject toInstance, int i, int j)
	{
		GameObject newTile = (GameObject)Instantiate(toInstance, new Vector3((i * scaleSize) - ((rows * scaleSize) / 2) + transform.position.x, (j * scaleSize) - ((columns * scaleSize) / 2) + transform.position.y, 0), Quaternion.identity);
		newTile.transform.localScale = new Vector3(scaleSize, scaleSize, 1);
		newTile.transform.parent = transform;
		return newTile;
	}

	void BuildGoalArea(int teamIndex, int i, int j)
	{
		GameObject newGoalAreaObj = InstantiateAtPosition(goalArea, i, j);
		GoalArea newGoalArea = newGoalAreaObj.GetComponent<GoalArea>();
		SpriteRenderer goalAreaRenderer = newGoalAreaObj.GetComponent<SpriteRenderer>();
		newGoalArea.teamIndex = teamIndex;
		if (teamIndex == 0)
			goalAreaRenderer.color = new Color(1, 0, 0);
		else
			goalAreaRenderer.color = new Color(0, 1, 0);
	}

	GameObject CreateTile(GameObject prefab, int i, int j, bool flipX, bool flipY)
	{
		GameObject newTile = InstantiateAtPosition(prefab, i, j);

		if (flipX)
			newTile.GetComponent<TinyObject>().FlipX();
		if (flipY)
			newTile.GetComponent<TinyObject>().FlipY();

		return newTile;
	}

	void BuildTerrain(int i, int j) 
	{
		if (isSolid(i - 1, j) && isSolid(i + 1, j) && isSolid(i, j - 1) && isSolid(i, j + 1))   // surrounded by solid?
			CreateTile(middle, i, j, false, false);
		else if (isSolid(i - 1, j) && isSolid(i + 1, j) && isSolid(i, j + 1)) // empty top
		{
			var obj = CreateTile(oneSidedHorizontal, i, j, false, false);
			obj.transform.Rotate(0, 0, 90);
		}
		else if (isSolid(i - 1, j) && isSolid(i + 1, j) && isSolid(i, j - 1)) // empty bottom
		{
			var obj = CreateTile(oneSidedHorizontal, i, j, true, false);
			obj.transform.Rotate(0, 0, 90);
		}
		else if (isSolid(i + 1, j) && isSolid(i, j + 1) && isSolid(i, j - 1)) // empty left
		{
			CreateTile(oneSidedHorizontal, i, j, false, false);
		}
		else if (isSolid(i - 1, j) && isSolid(i, j + 1) && isSolid(i, j - 1)) // empty right
		{
			CreateTile(oneSidedHorizontal, i, j, true, false);
		}
		else if (isSolid(i + 1, j) && isSolid(i, j - 1)) // top left corner
		{
			CreateTile(corner, i, j, false, false);
		}
		else if (isSolid(i - 1, j) && isSolid(i, j - 1)) // top right corner
		{
			CreateTile(corner, i, j, true, false);
		}
		else if (isSolid(i + 1, j) && isSolid(i, j + 1)) // bottom left corner
		{
			CreateTile(corner, i, j, false, true);
		}
		else if (isSolid(i - 1, j) && isSolid(i, j + 1)) // bottom right corner
		{
			CreateTile(corner, i, j, true, true);
		}
		else {
			var obj = CreateTile(oneSidedHorizontal, i, j, true, false);
			obj.transform.Rotate(0, 0, 90);
		}
	}

	void InstanceTerrain()
	{
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < columns; j++)
			{
				if (tileData[i, j] == TileType.ground)
					BuildTerrain(i, j);
				else if (tileData[i, j] == TileType.ladder)
					InstantiateAtPosition(ladderTile, i, j);
				else if (tileData[i, j] == TileType.none)
					continue;
			}
		}
	}

	// Use this for initialization
	void Start () {
		scaleSize = 3;

		// Create TileData from Image map
		LoadFromMap();
		// Create instances of tiles
		InstanceTerrain();
	}

	bool isSolid(int row, int column)
	{
		if (row < 0 || row > rows - 1 || column < 0 || column > columns - 1 || tileData[row,column] == TileType.none || tileData[row, column] == TileType.ladder)
			return false;

		return true;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
