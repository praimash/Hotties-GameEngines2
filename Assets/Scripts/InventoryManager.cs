using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance; // Her yerden eriþmek için (Singleton)

    [Header("UI Ayarlarý")]
    public GameObject inventoryPanel; // Açýp kapatacaðýmýz panel
    public Transform slotContainer;   // Ýkonlarýn dizileceði yer (InventoryPanel'in kendisi)
    public GameObject slotPrefab;     // Oraya koyacaðýmýz ikon taslaðý

    [Header("Envanter Verisi")]
    public List<Item> collectedItems = new List<Item>(); // Toplananlar listesi

    private bool isOpen = false;

    void Awake()
    {
        // Singleton ayarý (Bunu ezberle, çok iþine yarar)
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Baþlangýçta panel kapalý olsun
        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        // I tuþuna basýnca Aç/Kapa
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);

        if (isOpen)
        {
            // Panel açýlýnca mouse'u serbest býrak
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UpdateUI(); // Ekraný yenile
        }
        else
        {
            // Kapanýnca mouse'u kilitle
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void AddItem(Item item)
    {
        collectedItems.Add(item); // Listeye ekle
        Debug.Log(item.itemName + " Envantere eklendi!");
    }

    void UpdateUI()
    {
        // Önce eski ikonlarý temizle
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }

        // Listeki her eþya için yeni ikon yarat
        foreach (Item item in collectedItems)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotContainer);
            // Slotun resmini eþyanýn ikonu yap
            newSlot.GetComponent<Image>().sprite = item.icon;
        }
    }

    // Bu fonksiyon envanterde belirli bir isimde eþya var mý diye bakar
    public bool HasItem(string itemNameToCheck)
    {
        foreach (Item item in collectedItems)
        {
            if (item.itemName == itemNameToCheck)
            {
                return true; // Buldum!
            }
        }
        return false; // Yokmuþ :(
    }
}