using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class UiController : MonoBehaviour
{
    public GameObject InventoryButton;
    public int EqHoldingItem;
    public RawImage EqHoldingItemImg;

    #region Controllers
    public PlayerController playerController;
    public BaseController baseController;
    public TimeController timeController;
    public SceneController sceneController;
    #endregion

    #region TopBar
    public TMP_Text TimeText;
    public TMP_Text DayText;
    public TMP_Text MoneyText;
    public TMP_Text ScienceText;
    #endregion

    #region BottomBar
    public UnityEngine.UI.Image StaminaBar;
    public UnityEngine.UI.Image HealthBar;
    #endregion

    #region Images
    public Texture Maczuga;
    public Texture Sword;
    public Texture Hammer;
    public Texture Axe;
    public Texture Hoe;
    public Texture Bucket;
    public Texture Seeds;
    public Texture Carrot;

    #endregion

    #region InfoPanel
    public GameObject InfoPanelPrefab;
    public GameObject InfoPanel;
    public GameObject DropdownCanvas;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        baseController = GameObject.Find("BaseController").GetComponent<BaseController>();
        timeController = GameObject.Find("EnvironmentController").GetComponent<TimeController>();
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        EqHoldingItemImg= GameObject.Find("EqHoldingItemImg").GetComponent<RawImage>();

        TimeText = GameObject.Find("TimeTxt").GetComponent<TextMeshProUGUI>();
        DayText = GameObject.Find("DateTxt").GetComponent<TextMeshProUGUI>();
        MoneyText = GameObject.Find("MoneyTxt").GetComponent<TextMeshProUGUI>();
        ScienceText = GameObject.Find("ScienceTxt").GetComponent<TextMeshProUGUI>();

        HealthBar = GameObject.Find("HealthBar").GetComponent<UnityEngine.UI.Image>();
        StaminaBar = GameObject.Find("StaminaBar").GetComponent<UnityEngine.UI.Image>();

        DropdownCanvas = GameObject.Find("ActionCanvas");

        EqHoldingItem = -1;
        WriteEQ();
        ToggleInventory();
    }

    public void WriteEQ()
    {
        for (int i = 0; i < 8; i++)
        {
            int k = i;
            var temp = GameObject.Find("QuickSlot" + i);
            temp.transform.Find("SlotBtnImg").GetComponent<RawImage>().texture = ItemImage(playerController.PlayerEQ[i].itemType);
            temp.GetComponent<Button>().onClick.AddListener(delegate { GameObject.Find("UI").GetComponent<UiController>().MooveItem(k); });
        }

        for (int i=0; i < 6;i++)
        {
            for(int j=0; j < 6;j++)
            {
                int k=6*i+j;
                var temp=Instantiate(InventoryButton, GameObject.Find("InventoryPanelBack").transform);
                temp.transform.localPosition = new Vector3(-325+125*j, 325 - 125 * i, 0);
                temp.name = "Slot" + k;
                temp.GetComponent<Button>().onClick.AddListener(delegate { GameObject.Find("UI").GetComponent<UiController>().MooveItem(k + 8); });
                temp.transform.Find("SlotBtnImg").GetComponent<RawImage>().texture = ItemImage(playerController.PlayerEQ[k+8].itemType);
                if (playerController.PlayerEQ[k + 8].itemCount > 1)
                {
                    temp.transform.Find("SlotTxt").GetComponent<TextMeshProUGUI>().text = playerController.PlayerEQ[k + 8].itemCount.ToString();
                }
            }
        }
    }

    public void UpdateEQ()
    {
        for(int i=0 ; i < 8 ; i++)
        {
            var temp = GameObject.Find("QuickSlot" + i);
            temp.transform.Find("SlotBtnImg").GetComponent<RawImage>().texture = ItemImage(playerController.PlayerEQ[i].itemType);
        }

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                int k = 6 * i + j;
                var temp = GameObject.Find("Slot" + k);
                temp.transform.Find("SlotBtnImg").GetComponent<RawImage>().texture = ItemImage(playerController.PlayerEQ[k+8].itemType);
                if(playerController.PlayerEQ[k + 8].itemCount > 1)
                {
                    temp.transform.Find("SlotTxt").GetComponent<TextMeshProUGUI>().text = playerController.PlayerEQ[k + 8].itemCount.ToString();
                }
            }
        }
    }

    public Texture ItemImage(int id)
    {
        Texture temp=null;
        switch (id)
        {
            case 0: temp = null; break;
            case 1: temp = Maczuga; break;
            case 2: temp = Sword; break;
            case 3: temp = Axe; break;
            case 4: temp = Hammer; break;
            case 5: temp = Hoe; break;
            case 6: temp = Bucket; break;
            case 7: temp = Seeds; break;
            case 8: temp = Carrot; break;
        }
        return temp;

    }

    public void MooveItem(int slotID)
    {
        Debug.Log("Move");
        if (EqHoldingItem == -1 && playerController.PlayerEQ[slotID].itemType != 0)
        {
            EqHoldingItemImg.enabled = true;
            EqHoldingItem = slotID;
            EqHoldingItemImg.texture = ItemImage(playerController.PlayerEQ[slotID].itemType);
            Debug.Log("Holding item at id:" + EqHoldingItem);

        }
        else if(EqHoldingItem != -1 && playerController.PlayerEQ[slotID].itemType == 0)
        {
            Debug.Log("No longer holding" + EqHoldingItem);
            playerController.MoveItem(EqHoldingItem, slotID);
            UpdateEQ();
            EqHoldingItem = -1;
            EqHoldingItemImg.texture = ItemImage(0);
            EqHoldingItemImg.enabled = false;
        }
    }

    public void ToggleInventory()
    {
        GameObject.Find("Inventory").GetComponent<Canvas>().enabled=!GameObject.Find("Inventory").GetComponent<Canvas>().enabled;
        if (GameObject.Find("Inventory").GetComponent<Canvas>().enabled == true)
        {
            playerController.LockControls = true;
            Cursor.visible = true;
        }
        else
        {
            playerController.LockControls = false;
            Cursor.visible = true;
        }


    }

    public void ToggleBuildMenu()
    {
        GameObject.Find("BuildMenu").GetComponent<Canvas>().enabled = !GameObject.Find("BuildMenu").GetComponent<Canvas>().enabled;
        if (GameObject.Find("BuildMenu").GetComponent<Canvas>().enabled == true)
        {
            playerController.LockControls = true;
            Cursor.visible = true;
        }
        else
        {
            playerController.LockControls = false;
            Cursor.visible = true;
        }
    }

    public void NotEnoughCoins()
    {
        MoneyText.color = Color.red;
        Invoke("CoinsColor", 1f);
    }

    public void CoinsColor()
    {
        MoneyText.color = Color.black;
    }

    public void GameOverScreen()
    {
        GameObject.Find("GameOver").GetComponent<Canvas>().enabled = true;
    }

    public void InGameMenu()
    {
        if (GameObject.Find("InGameMenu").GetComponent<Canvas>().enabled == true)
        {
            GameObject.Find("InGameMenu").GetComponent<Canvas>().enabled = false;
            timeController.TimeFlow = true;
            sceneController.TimeFlow = true;
        }
        else
        {
            GameObject.Find("InGameMenu").GetComponent<Canvas>().enabled = true;
            timeController.TimeFlow = false;
            sceneController.TimeFlow = false;
        }
    }

    public void StructureDataSpawn(GameObject structure)
    {
        if (InfoPanel == null)
        {
            InfoPanel = Instantiate(InfoPanelPrefab, new Vector3(structure.transform.position.x, structure.transform.position.y+2, structure.transform.position.z), Quaternion.Euler(0,0,0));
        }
        InfoPanel.transform.Find("Level1Txt").transform.Find("Bar1").GetComponent<Image>().rectTransform.sizeDelta = new Vector2(structure.transform.parent.GetComponent<GridElement>().StructureProgress/200, 0.1f);
        InfoPanel.transform.Find("Level1Txt").transform.Find("Bar1").GetComponent<Image>().rectTransform.localPosition = new Vector3(0.35f+structure.transform.parent.GetComponent<GridElement>().StructureProgress / 400, 0.075f, 0);
        InfoPanel.transform.Find("Level2Txt").transform.Find("Bar2").GetComponent<Image>().rectTransform.sizeDelta = new Vector2(structure.transform.parent.GetComponent<GridElement>().WaterLevel / 200, 0.1f);
        InfoPanel.transform.Find("Level2Txt").transform.Find("Bar2").GetComponent<Image>().rectTransform.localPosition = new Vector3(0.35f + structure.transform.parent.GetComponent<GridElement>().WaterLevel / 400, 0.075f, 0);

    }
    public void StructureDataDelete()
    {
        Destroy(InfoPanel);
    }


    public void ExitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        string hourTxt = timeController.GameHour.ToString();
        string minuteTxt = timeController.GameMinute.ToString();
        if (timeController.GameHour < 10)
        {
            hourTxt="0"+ timeController.GameHour.ToString();
        }
        if(timeController.GameMinute < 10)
        {
            minuteTxt="0"+ timeController.GameMinute.ToString();
        }
        TimeText.text = hourTxt + ":" + minuteTxt;
        DayText.text = "Day: "+timeController.GameDay.ToString();
        MoneyText.text=baseController.Coins.ToString();
        ScienceText.text=baseController.SciencePoints.ToString();

        StaminaBar.rectTransform.sizeDelta = new Vector2(250*playerController.PlayerStamina/100, 75);
        StaminaBar.rectTransform.localPosition = new Vector2(-125 * (100-playerController.PlayerStamina) / 100, 0);
        HealthBar.rectTransform.sizeDelta = new Vector2(250 * playerController.PlayerHP / 100, 75);
        HealthBar.rectTransform.localPosition = new Vector2(-125 * (100 - playerController.PlayerHP) / 100, 0);
        EqHoldingItemImg.gameObject.transform.position = Input.mousePosition;

    }
}
