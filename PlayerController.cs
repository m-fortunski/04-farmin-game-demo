using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public UiController UI;
    public SceneController sceneController;
    public ItemsController itemsController;
    public Building building;

    #region Player
    public Animator PlayerAnimator;
    public Vector3 MovementVector;
    float PlayerSpeed = 5f;
    bool sprint;

    public GameObject CurrentPlot;
    public float PlayerHP = 100;
    public float PlayerStamina = 100;
    float RegenerateStaminaRate = 0.5f;
    float RunStaminaCost = 0.75f;
    float AttackStaminaCost = 10;
    public bool Blocking;


    public bool LockControls=true;
    #endregion

    #region Camera
    public float CameraZoom;
    public GameObject PlayerCamera;
    public Camera camera;
    #endregion

    #region Items
    public GameObject Equipped;
    public Item[] PlayerEQ = new Item[44];
    public int ItemInHandID;

    public GameObject Hand;
    public GameObject Club;
    public GameObject Sword;
    public GameObject Axe;
    public GameObject Hammer;
    public GameObject Hoe;
    public GameObject Bucket;

    public GameObject CarrotSeed;
    public GameObject Carrot;
    #endregion

    #region build
    public int GridX;
    public int GridZ;
    public int PlotX;
    public int PlotZ;
    #endregion

    #region Interact
    public LayerMask Interactable;
    public Material previousMaterial;
    public Material HighlightedMaterial;
    public GameObject previousObject;
    public float InteractDistance = 6;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        building = this.gameObject.GetComponent<Building>();
        sprint = false;
        MovementVector = new Vector3(0, 0, 0);
        PlayerCamera = GameObject.Find("PlayerCamera");
        PlayerAnimator = GetComponent<Animator>();
        Equipped = GameObject.Find("Equipped");
        UI = GameObject.Find("UI").GetComponent<UiController>();
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        itemsController = GameObject.Find("EnvironmentController").GetComponent<ItemsController>();
        itemsController.AddCoins(5, this.transform.position);
        for(int i = 0; i < 44; i++)
        {
            PlayerEQ[i] = new Item();
            PlayerEQ[i].itemType = 0;
            PlayerEQ[i].itemCount = 0;
        }
        PlayerEQ[0].itemType = 0;
        PlayerEQ[1].itemType = 1;
        PlayerEQ[2].itemType = 2;
        PlayerEQ[3].itemType = 3;
        PlayerEQ[4].itemType = 4;
        PlayerEQ[5].itemType = 5;
        PlayerEQ[6].itemType = 6;
        PlayerEQ[7].itemType = 7;
        PlayerEQ[8].itemType = 8;
        CurrentPlot = GameObject.Find("Plot.0.0");
        ChangeItemInHand(0);
        ToggleGrid(false);
        camera = PlayerCamera.GetComponent<Camera>();
    }

    #region PlayerActions
    public void ChangeZoom(InputAction.CallbackContext ctx)
    {
        CameraZoom -= ctx.ReadValue<Vector2>().y * Time.deltaTime;
        if (CameraZoom < 1)
        {
            CameraZoom = 1;
        }
        else if (CameraZoom > 10)
        {
            CameraZoom = 10;
        }
    }
    public void Sprint(InputAction.CallbackContext ctx)
    {
        if (LockControls == false)
        {
            if (ctx.ReadValue<float>() > 0)
            {
                sprint = true;
            }
            else
            {
                sprint = false;
            }
        }

    }
    public void Move(InputAction.CallbackContext ctx)
    {
        if (LockControls == false)
        {
            MovementVector = new Vector3(ctx.ReadValue<Vector2>().x, 0, ctx.ReadValue<Vector2>().y);
            if (MovementVector != new Vector3(0, 0, 0))
            {
                this.transform.rotation = Quaternion.LookRotation(MovementVector);
            }
        }
    }
    public void Attack(InputAction.CallbackContext ctx)
    {
        if (LockControls == false&&ItemInHandID!=0)
        {

            if (PlayerStamina >= AttackStaminaCost && LockControls == false&& building.enabled == false)
            {
                if (ctx.ReadValue<float>() > 0)
                {
                    PlayerAnimator.SetBool("Attacking", true);
                    LockControls = true;
                    Invoke("EndAttack", 1f);
                    Equipped.GetComponent<Weapon>().attack = true;
                    LooseStamina(AttackStaminaCost);
                }
                else
                {
                    PlayerAnimator.SetBool("Attacking", false);
                }
            }
            else
            {
                BuildAction();
            }
        }
    }
    public void Block(InputAction.CallbackContext ctx)
    {
        if (LockControls == false && ItemInHandID !=0)
        {
            if (PlayerStamina >= AttackStaminaCost / 2 && building.enabled == false)
            {
                if (ctx.ReadValue<float>() > 0 && LockControls == false)
                {
                    LooseStamina(AttackStaminaCost / 2);
                    PlayerAnimator.SetBool("Blocking", true);
                    Invoke("EndBlock", 1.2f);
                    LockControls = true;
                    Blocking = true;
                }
                else if (ctx.ReadValue<float>() == 0)
                {
                    PlayerAnimator.SetBool("Blocking", false);
                }
            }
        }
        if (building.enabled == true)
        {
            UI.ToggleBuildMenu();
        }

    }
    public void Interact(InputAction.CallbackContext ctx)
    {
        if (LockControls == false)
        {
            if (ctx.ReadValue<float>() > 0)
            {
                PlayerAnimator.SetBool("Interacting", true);
                LockControls = true;
                Invoke("EndInteraction", 1.2f);
            }
            else if (LockControls == false)
            {
                PlayerAnimator.SetBool("Interacting", false);
            }
        }
    }
    public void ToggleMenu(InputAction.CallbackContext ctx)
    {
        UI.InGameMenu();
    }
    public void ChangeItemInHand(int slot)
    {
        if (LockControls == false)
        {
            Destroy(Equipped);
            GameObject.Find("QuickSlot" + ItemInHandID).GetComponent<Image>().color = Color.white;
            ItemInHandID = slot;
            GameObject.Find("QuickSlot" + slot).GetComponent<Image>().color = Color.red;
            switch (PlayerEQ[slot].itemType)
            {
                case 0: Equipped = Instantiate(Hand, GameObject.Find("RightHand").transform); break;
                case 1: Equipped = Instantiate(Club, GameObject.Find("RightHand").transform); break;
                case 2: Equipped = Instantiate(Sword, GameObject.Find("RightHand").transform); break;
                case 3: Equipped = Instantiate(Axe, GameObject.Find("RightHand").transform); break;
                case 4: Equipped = Instantiate(Hammer, GameObject.Find("RightHand").transform); break;
                case 5: Equipped = Instantiate(Hoe, GameObject.Find("RightHand").transform); break;
                case 6: Equipped = Instantiate(Bucket, GameObject.Find("RightHand").transform); break;
                case 7: Equipped = Instantiate(CarrotSeed, GameObject.Find("RightHand").transform); break;
                case 8: Equipped = Instantiate(Carrot, GameObject.Find("RightHand").transform); break;
            }
            Equipped.transform.localPosition = new Vector3(0, 0, 0);
            Equipped.transform.rotation = Quaternion.Euler(GameObject.Find("RightHand").transform.rotation.eulerAngles.x, GameObject.Find("RightHand").transform.rotation.eulerAngles.y - 90, GameObject.Find("RightHand").transform.rotation.eulerAngles.z + 90);

            Equipped.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            if (PlayerEQ[ItemInHandID].itemType == 4)
            {
                building.enabled = true;
                ToggleGrid(true);
                building.Build = true;
                building.SelectStructureToConstruct(1);
            }
            else if(PlayerEQ[ItemInHandID].itemType == 5)
            {
                building.enabled = true;
                ToggleGrid(true);
                building.Build = false;
                building.SelectStructureToConstruct(2);
            }

            else
            {
                ToggleGrid(false);
                building.enabled = false;
            }
        }

    }
    void EndAttack()
    {
        PlayerAnimator.SetBool("Attacking", false);
        LockControls = false;
        Equipped.GetComponent<BoxCollider>().enabled = true;
        Equipped.GetComponent<Weapon>().attack = true;
        MovementVector = new Vector3(0, 0, 0);
    }
    void EndBlock()
    {
        PlayerAnimator.SetBool("Blocking", false);
        LockControls = false;

        Blocking = false;
    }
    void EndInteraction()
    {
        PlayerAnimator.SetBool("Interacting", false);
        LockControls = false;
    }
    public void EndDamage()
    {
        PlayerAnimator.SetBool("TakingDamage", false);
        LockControls = false;

    }
    public void BuildAction()
    {
        building.PlaceStructure(PlotX, PlotZ);
    }
    #endregion

    #region Inventory
    public void AddItem(Item item, int slot)
    {

    }
    public void RemoveItem(int slot, bool drop)
    {

    }
    public Item ReadItem(int id)
    {
        Item temp = PlayerEQ[id];
        return null;
    }
    public void MoveItem(int idPrev, int idNext)
    {
        PlayerEQ[idNext] = PlayerEQ[idPrev];
        PlayerEQ[idPrev] = new Item { };
    }
    #endregion

    #region Stats
    public void LooseStamina(float deltaStamina)
    {
        PlayerStamina -= deltaStamina;
    }
    public void LooseHP(float deltaHP, GameObject damageObject)
    {
        if (Blocking == false)
        {
            PlayerHP -= deltaHP;
            PlayerAnimator.SetBool("TakingDamage", true);
            Invoke("EndDamage", 1);
            LockControls = true;
            this.transform.position += new Vector3(this.transform.position.x - damageObject.transform.position.x, 0, this.transform.position.z - damageObject.transform.position.z) * 2;
        }
    }
    public void RegenerateStamina(float regenerateStaminaRate)
    {
        if (PlayerStamina > 100)
        {
            PlayerStamina = 100;
        }
        PlayerStamina += regenerateStaminaRate;
    }
    #endregion

    #region Building
    public void ToggleGrid(bool active)
    {
        building.enabled=active;

    }
    #endregion

    #region Interact
    public void InteractHighlight()
    {

        if (ItemInHandID == 0)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            mousePos = camera.ScreenToWorldPoint(mousePos);

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, Interactable))
            {
                if (hit.transform.GetComponent<MeshCollider>().material != HighlightedMaterial&&Vector3.Distance(this.transform.position, hit.transform.position)<InteractDistance)
                {
                    if (previousObject != null)
                    {
                        previousObject.GetComponent<MeshRenderer>().material = previousMaterial;
                    }
                    previousObject = hit.transform.gameObject;
                    previousMaterial = hit.transform.GetComponent<MeshRenderer>().material;
                    hit.transform.GetComponent<MeshRenderer>().material = HighlightedMaterial;
                    UI.StructureDataSpawn(hit.transform.gameObject);
                }
            }
            else
            {
                if (previousObject != null)
                {
                    previousObject.GetComponent<MeshRenderer>().material = previousMaterial;
                    previousObject = null;
                    UI.StructureDataDelete();
                }
            }
        }
        else if (previousObject != null)
        {
            previousObject.GetComponent<MeshRenderer>().material = previousMaterial;
            previousObject = null;
            UI.StructureDataDelete();
        }
    }
    #endregion

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sceneController.TimeFlow == true)
        {
            if (MovementVector.x != 0 || MovementVector.z != 0)
            {
                PlayerAnimator.SetBool("Mooving", true);
            }
            else
            {
                PlayerAnimator.SetBool("Mooving", false);
                if (LockControls == false)
                {
                    RegenerateStamina(0.25f);
                }
            }
            if (sprint == false || PlayerStamina < 5)
            {
                this.transform.position += MovementVector * PlayerSpeed / 60;
                PlayerCamera.transform.position = new Vector3(this.transform.position.x, 1.5f + 2 * CameraZoom, this.transform.position.z - 2 * CameraZoom);
                PlayerAnimator.speed = 1;
            }
            else
            {
                LooseStamina(RunStaminaCost);
                this.transform.position += MovementVector * PlayerSpeed / 30;
                PlayerCamera.transform.position = new Vector3(this.transform.position.x, 1.5f + 2 * CameraZoom, this.transform.position.z - 2 * CameraZoom);
                PlayerAnimator.speed = 2;
            }
            if (PlayerHP <= 0||this.transform.position.y<-50)
            {
                Destroy(this.gameObject);
                UI.GameOverScreen();
            }
            InteractHighlight();

        }


    }
}
