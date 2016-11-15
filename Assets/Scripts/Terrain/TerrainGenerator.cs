﻿using UnityEngine;
using System.Collections;

enum TileType { none, ground, ladder, goalArea };

public class TerrainGenerator : MonoBehaviour {

	public GameObject tile, ladderTile, goalArea;
	public Sprite corner, oneSidedVertical, oneSidedHorizontal, threeSided, middle;
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

    void BuildTerrain(int i, int j) 
    {
        GameObject newTile = InstantiateAtPosition(tile, i, j);
        SpriteRenderer tileRenderer = ((SpriteRenderer)newTile.GetComponent("SpriteRenderer"));

        if (isSolid(i - 1, j) && isSolid(i + 1, j) && isSolid(i, j - 1) && isSolid(i, j + 1))   // surrounded by solid?
            tileRenderer.sprite = middle;
        else if (isSolid(i - 1, j) && isSolid(i + 1, j) && isSolid(i, j + 1)) // empty top
        {
            tileRenderer.sprite = oneSidedVertical;
            tileRenderer.flipY = true;
        }
        else if (isSolid(i - 1, j) && isSolid(i + 1, j) && isSolid(i, j - 1)) // empty bottom
        {
            tileRenderer.sprite = oneSidedVertical;
        }
        else if (isSolid(i + 1, j) && isSolid(i, j + 1) && isSolid(i, j - 1)) // empty left
        {
            tileRenderer.sprite = oneSidedHorizontal;
        }
        else if (isSolid(i - 1, j) && isSolid(i, j + 1) && isSolid(i, j - 1)) // empty right
        {
            tileRenderer.sprite = oneSidedHorizontal;
            tileRenderer.flipX = true;
        }
        else if (isSolid(i + 1, j) && isSolid(i, j - 1)) // top left corner
        {
            tileRenderer.sprite = corner;
        }
        else if (isSolid(i - 1, j) && isSolid(i, j - 1)) // top right corner
        {
            tileRenderer.sprite = corner;
            tileRenderer.flipX = true;
        }
        else if (isSolid(i + 1, j) && isSolid(i, j + 1)) // bottom left corner
        {
            tileRenderer.sprite = corner;
            tileRenderer.flipY = true;
        }
        else if (isSolid(i - 1, j) && isSolid(i, j + 1)) // bottom right corner
        {
            tileRenderer.sprite = corner;
            tileRenderer.flipX = true;
            tileRenderer.flipY = true;
        }
        else
            tileRenderer.sprite = oneSidedVertical;
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
