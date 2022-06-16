using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildController : MonoBehaviour
{
    public static BuildController active;

    public LevelMesh levelMesh;

    public TextMeshProUGUI tooltip;
    public Transform selectionOutline;

    private Vector2Int cursorTilePos;


    private Camera cam;


    private void Awake()
    {
        active = this;
    }

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        cursorTilePos = getCursorTilePos();
        if (!checkCursorInBounds(cursorTilePos)) return;

        interactWithLevel();
        manageUI();
    }

    private void interactWithLevel()
    {

        if (Input.GetKeyDown(InputSettings.place))
        {
            setTile(cursorTilePos, InventoryController.active.GetSelectedTile(cursorTilePos));
        }
        else if (Input.GetKeyDown(InputSettings.interact))
        {
            LevelData.world[cursorTilePos.x, cursorTilePos.y].interact();
        }
        else if (Input.GetKeyDown(InputSettings.remove))
        {
            byte[] old_metadata = LevelData.world[cursorTilePos.x, cursorTilePos.y].metadata;
            setTile(cursorTilePos, new Tile_empty(cursorTilePos, old_metadata));
        }
    }

    public void setTile(Vector2Int pos, Tile tile)
    {
        LevelData.world[pos.x, pos.y] = tile;
        levelMesh.setUVAt(pos.x, pos.y, tile.id);

        byte[] metadata = tile.metadata;

        for (int i = 0; i < metadata.Length && i < 4; i++)
        {
            if (metadata[i] != 0) levelMesh.setUVAt(i + 1, pos.x, pos.y, metadata[i], (Orientation)i, i > 1);
            else levelMesh.setUVAt(i + 1, pos.x, pos.y, 64, Orientation.north, false);
        }
    }

    private Tile getTile(Vector2Int pos)
    {
        return LevelData.world[pos.x, pos.y];
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

    private void manageUI()
    {
        selectionOutline.position = new Vector3(cursorTilePos.x + 0.5f, 1, cursorTilePos.y + 0.5f);

        Tile hoveredTile = getTile(cursorTilePos);
        if (hoveredTile is Tile_wire) // sets the cursor tooltip text
        {
            tooltip.text = hoveredTile.metadata[4].ToString();

            tooltip.transform.position = Input.mousePosition;
        }
        else tooltip.text = "";
    }
}
