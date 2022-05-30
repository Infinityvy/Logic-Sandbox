using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour
{
    public LevelMesh levelMesh;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        interactWithLevel();
    }

    private void interactWithLevel()
    {
        if(Input.GetKeyDown(InputSettings.place))
        {
            Vector2Int cursorTilePos = getCursorTilePos();

            if(checkCursorInBounds(cursorTilePos)) setTile(cursorTilePos, 6);
        }
        else if (Input.GetKeyDown(InputSettings.interact))
        {

        }
        else if (Input.GetKeyDown(InputSettings.remove))
        {
            Vector2Int cursorTilePos = getCursorTilePos();

            if(checkCursorInBounds(cursorTilePos)) setTile(cursorTilePos, 0);
        }
    }

    private void setTile(Vector2Int pos, int id)
    {
        LevelData.world[pos.x, pos.y] = id;
        levelMesh.setUVAt(pos.x, pos.y, id);
    }

    private Vector2Int getCursorTilePos()
    {
        Vector3 cursorWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2Int((int)cursorWorldPos.x, (int)cursorWorldPos.z);
    }

    private bool checkCursorInBounds(Vector2Int cursorTilePos)
    {
        return (cursorTilePos.x >= 0 && cursorTilePos.y >= 0 && 
                cursorTilePos.x < LevelData.size && cursorTilePos.y < LevelData.size);
    }
}
