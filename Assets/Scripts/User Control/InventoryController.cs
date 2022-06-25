using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public static InventoryController active;

    public Transform selector;
    public Sprite[] uiTileSprites;

    private int selectedIndex = 0;

    private UI_Tile[] ui_tiles = { new UI_Tile_button(), new UI_Tile_wire(), new UI_Tile_lamp()};

    private GameObject[] slots;

    private GameObject slotEditDisplay;

    private void Awake()
    {
        active = this;
    }

    private void Start()
    {
        slots = new GameObject[ui_tiles.Length];

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = createSlot(i.ToString(), transform, uiTileSprites[ui_tiles[i].spriteID]);
        }


        selector.SetParent(slots[selectedIndex].transform, true);
        Invoke("resetSelector", 0.01f);

        initSlotEditDisplay();
    }

    private void Update()
    {
        inventoryControls();
        selectorAnimator();
    }

    private void inventoryControls()
    {
        if (Input.GetKey(InputSettings.edit))
        {
            PlayerCamera.frozen = true;
            enabledSlotEditDisplay(ui_tiles[selectedIndex].spriteID);

            if (Input.GetKeyDown(InputSettings.up))
            {
                byte data = ui_tiles[selectedIndex].simple_metadata[0];
                ui_tiles[selectedIndex].setWire(0, (byte)((data + 1) % 3));
            }
            if (Input.GetKeyDown(InputSettings.right))
            {
                byte data = ui_tiles[selectedIndex].simple_metadata[1];
                ui_tiles[selectedIndex].setWire(1, (byte)((data + 1) % 3));
            }
            if (Input.GetKeyDown(InputSettings.down))
            {
                byte data = ui_tiles[selectedIndex].simple_metadata[2];
                ui_tiles[selectedIndex].setWire(2, (byte)((data + 1) % 3));
            }
            if (Input.GetKeyDown(InputSettings.left))
            {
                byte data = ui_tiles[selectedIndex].simple_metadata[3];
                ui_tiles[selectedIndex].setWire(3, (byte)((data + 1) % 3));
            }


            if (ui_tiles[selectedIndex] is UI_Tile_wire && Input.GetAxis("Mouse ScrollWheel") != 0)
            {

                UI_Tile_wire wire = (UI_Tile_wire)ui_tiles[selectedIndex];

                wire.setDelay((byte)Mathf.Clamp(wire.getDelay() + (Input.GetAxis("Mouse ScrollWheel") > 0 ? 1 : -1), 0, 255));
            }


            return;
        }
        else if (PlayerCamera.frozen)
        {
            PlayerCamera.frozen = false;
            disableSlotEditDisplay();
        }

        for(int i = 0; i < InputSettings.numbers.Length && i < ui_tiles.Length; i++)
        {
            if(Input.GetKeyDown(InputSettings.numbers[i]))
            {
                selectedIndex = i;
                selector.SetParent(slots[i].transform, true);
                selector.localPosition = Vector3.zero;

                break;
            }
        }
    }

    public Tile GetSelectedTile(Vector2Int position)
    {
        return ui_tiles[selectedIndex].getTile(position);
    }

    private void selectorAnimator()
    {
        float scale = Mathf.Sin(Time.time * 2.5f) * 0.15f + 0.8f;

        selector.localScale = new Vector2(scale, scale);
    }

    private GameObject createSlot(string index, Transform parent, Sprite sprite)
    {
        GameObject slot = new GameObject("Slots" + index, typeof(Image));

        slot.transform.SetParent(parent, true);
        slot.GetComponent<Image>().sprite = sprite;

        return slot;
    }

    #region SlotEditDisplay

    private void initSlotEditDisplay()
    {
        slotEditDisplay = createSlot("EditDisplay", transform.parent, uiTileSprites[ui_tiles[0].spriteID]);

        slotEditDisplay.GetComponent<Image>().enabled = false;
        slotEditDisplay.transform.localPosition = Vector3.zero;
        slotEditDisplay.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
    }

    private void enabledSlotEditDisplay(int spriteID)
    {
        slotEditDisplay.GetComponent<Image>().enabled = true;
        slotEditDisplay.GetComponent<Image>().sprite = uiTileSprites[spriteID];
    }

    private void disableSlotEditDisplay()
    {
        slotEditDisplay.GetComponent<Image>().enabled = false;
    }

    #endregion

    private void resetSelector()
    {
        selector.localPosition = Vector3.zero;
    }
}

public static class MySystem
{
    public static Transform getChildByName(this Transform tr, string name)
    {
        for (int i = 0; i < tr.childCount; i++)
        {
            if (tr.GetChild(i).name == name) return tr.GetChild(i);
        }

        return null;
    }
}
