using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct UI_Tile_Sprites
{
    public Sprite sprite;

    public Sprite inputWireSprite;
    public Sprite outputWireSprite;
}

public class InventoryController : MonoBehaviour
{
    public static InventoryController active;

    public Transform selector;
    public UI_Tile_Sprites[] uiTileSprites;

    private int selectedIndex = 0;

    private UI_Tile[] ui_tiles = { new UI_Tile_wire(), new UI_Tile_lamp(), new UI_Tile_button()};

    private GameObject[] slots;

    private GameObject editModeDisplaySlot;

    private void Awake()
    {
        active = this;
    }

    private void Start()
    {
        slots = new GameObject[ui_tiles.Length];

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = createSlot(i.ToString(), transform, uiTileSprites[ui_tiles[i].spriteID].sprite);
        }


        selector.SetParent(slots[selectedIndex].transform, true);
        Invoke("resetSelector", 0.01f);

        init_editModeDisplaySlot();
    }

    private void Update()
    {
        inventoryControls();
        selectorAnimator();
    }

    private void inventoryControls()
    {
        if (Input.GetKey(InputSettings.edit)) //edit selected tile mode
        {
            PlayerCamera.frozen = true;
            enable_editModeDisplaySlot(selectedIndex);

            if (Input.GetKeyDown(InputSettings.up))
            {
                byte data = ui_tiles[selectedIndex].metadata[0];
                ui_tiles[selectedIndex].setWire(0, (byte)((data + 1) % 3));
                updateSlotMetadataDisplay(selectedIndex);
                updateSlotMetadataDisplay(selectedIndex, editModeDisplaySlot.transform);
            }
            if (Input.GetKeyDown(InputSettings.right))
            {
                byte data = ui_tiles[selectedIndex].metadata[1];
                ui_tiles[selectedIndex].setWire(1, (byte)((data + 1) % 3));
                updateSlotMetadataDisplay(selectedIndex);
                updateSlotMetadataDisplay(selectedIndex, editModeDisplaySlot.transform);
            }
            if (Input.GetKeyDown(InputSettings.down))
            {
                byte data = ui_tiles[selectedIndex].metadata[2];
                ui_tiles[selectedIndex].setWire(2, (byte)((data + 1) % 3));
                updateSlotMetadataDisplay(selectedIndex);
                updateSlotMetadataDisplay(selectedIndex, editModeDisplaySlot.transform);
            }
            if (Input.GetKeyDown(InputSettings.left))
            {
                byte data = ui_tiles[selectedIndex].metadata[3];
                ui_tiles[selectedIndex].setWire(3, (byte)((data + 1) % 3));
                updateSlotMetadataDisplay(selectedIndex);
                updateSlotMetadataDisplay(selectedIndex, editModeDisplaySlot.transform);
            }


            if (ui_tiles[selectedIndex] is UI_Tile_wire && Input.GetAxis("Mouse ScrollWheel") != 0)
            {

                UI_Tile_wire wire = (UI_Tile_wire)ui_tiles[selectedIndex];

                wire.setDelay((byte)Mathf.Clamp(wire.getDelay() + (Input.GetAxis("Mouse ScrollWheel") > 0 ? 1 : -1), 0, 255));
            }


            return;
        }
        else if (PlayerCamera.frozen) //unfreeze camera when exiting edit mode
        {
            PlayerCamera.frozen = false;
            disable_editModeDisplaySlot();
        }

        for(int i = 0; i < InputSettings.numbers.Length && i < ui_tiles.Length; i++)
        {
            if(Input.GetKeyDown(InputSettings.numbers[i]))
            {
                setSelectedTile(i);
                break;
            }
        }

        if(Input.GetKeyDown(InputSettings.copy)) //copy hovered tile to selected tile
        {
            Tile hoveredTile = BuildController.active.hoveredTile;
            if (hoveredTile is Tile_empty) return;

            setSelectedTile(hoveredTile.simpleID);

            ui_tiles[selectedIndex].metadata = (byte[])hoveredTile.metadata.Clone();
            updateSlotMetadataDisplay(selectedIndex);
        }
    }


    private void setSelectedTile(int index)
    {
        selectedIndex = index;
        selector.SetParent(slots[index].transform, true);
        selector.localPosition = Vector3.zero;
    }
    public Tile getSelectedTile(Vector2Int position) //this creates a new tile ready to be used
    {
        return ui_tiles[selectedIndex].getTile(position); 
    }

    public UI_Tile getSelectedUI_Tile() //this returns a reference to the selected ui_tile object for getting info about the selected tile without creating a new tile
    {
        return ui_tiles[selectedIndex];
    }

    private void selectorAnimator()
    {
        float scale = Mathf.Sin(Time.time * 2.5f) * 0.15f + 0.8f;

        selector.localScale = new Vector2(scale, scale);
    }

    private GameObject createSlot(string index, Transform parent, Sprite sprite)
    {
        GameObject slot = new GameObject("Slot" + index, typeof(Image));

        slot.transform.SetParent(parent, true);
        slot.GetComponent<Image>().sprite = sprite;
        slot.transform.localScale *= transform.parent.localScale.x * 2;

        return slot;
    }

    private GameObject createDisplayWire(string index, Transform parent, Sprite sprite, Orientation orientation)
    {
        GameObject wire = new GameObject("Wire" + index, typeof(Image));

        wire.transform.SetParent(parent, false);
        wire.transform.rotation = Quaternion.Euler(0, 0, -90f * (int)orientation);
        wire.GetComponent<Image>().sprite = sprite;
        wire.GetComponent<RectTransform>().sizeDelta = parent.GetComponent<RectTransform>().sizeDelta;

        if ((int)orientation > 1) wire.transform.localScale = new Vector3(-1, 1, 1);

        return wire;
    }

    private void updateSlotMetadataDisplay(int index)
    {
        updateSlotMetadataDisplay(index, slots[index].transform);
    }

    private void updateSlotMetadataDisplay(int index, Transform slot)
    {
        UI_Tile ui_tile = ui_tiles[index];

        foreach(Transform child in slot)
        {
            if (child.name != "InventorySelector") Destroy(child.gameObject);
        }

        for (int i = 0; i < 4; i++)
        {
            if(ui_tile.metadata[i] == 1) createDisplayWire(i.ToString(), slot, uiTileSprites[index].inputWireSprite, (Orientation)i);
            else if(ui_tile.metadata[i] == 2) createDisplayWire(i.ToString(), slot, uiTileSprites[index].outputWireSprite, (Orientation)i);
        }
    }

    #region editModeDisplaySlot

    private void init_editModeDisplaySlot()
    {
        editModeDisplaySlot = createSlot("EditDisplay", transform.parent, uiTileSprites[ui_tiles[0].spriteID].sprite);

        editModeDisplaySlot.GetComponent<Image>().enabled = false;
        editModeDisplaySlot.transform.localPosition = Vector3.zero;
        editModeDisplaySlot.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
    }

    private void enable_editModeDisplaySlot(int index)
    {
        editModeDisplaySlot.GetComponent<Image>().enabled = true;
        editModeDisplaySlot.GetComponent<Image>().sprite = uiTileSprites[ui_tiles[index].spriteID].sprite;
        updateSlotMetadataDisplay(index, editModeDisplaySlot.transform);
    }

    private void disable_editModeDisplaySlot()
    {
        editModeDisplaySlot.GetComponent<Image>().enabled = false;
        foreach(Transform child in editModeDisplaySlot.transform)
        {
            Destroy(child.gameObject);
        }
    }

    #endregion

    private void resetSelector()
    {
        selector.localPosition = Vector3.zero;
    }
}