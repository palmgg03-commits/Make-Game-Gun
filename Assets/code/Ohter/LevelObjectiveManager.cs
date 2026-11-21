using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelObjectiveManager : MonoBehaviour
{
    public static LevelObjectiveManager Instance;

    [Header("Objective Settings")]
    [SerializeField] private int requiredItems = 5;
    private int collectedItems = 0;

    [Header("Stage Clear / Exit")]
    [SerializeField] private GameObject exitZoneOrClearUI;
    [SerializeField] private bool autoClearWhenComplete = false;

    [Header("UI Reference")]
    [SerializeField] private TMP_Text objectiveText;
    [SerializeField] private List<Toggle> itemCheckboxes = new List<Toggle>(); // ลาก Toggle ทั้งหมดมาใส่ที่นี่

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (exitZoneOrClearUI != null)
        {
            exitZoneOrClearUI.SetActive(false);
        }

        // รีเซ็ตกล่องติ๊กถูกทั้งหมดให้ว่างตอนเริ่มเกม
        foreach (Toggle checkbox in itemCheckboxes)
        {
            if (checkbox != null)
            {
                checkbox.isOn = false;
            }
        }

        UpdateObjectiveUI();
    }

    public void CollectItem(int amount = 1)
    {
        collectedItems += amount;
        collectedItems = Mathf.Clamp(collectedItems, 0, requiredItems);

        UpdateObjectiveUI();

        if (collectedItems >= requiredItems)
        {
            OnObjectiveCompleted();
        }
    }

    private void UpdateObjectiveUI()
    {
        if (objectiveText != null)
        {
            objectiveText.text = $"Items: {collectedItems} / {requiredItems}";
        }

        // สั่งติ๊กถูกในกล่องตามจำนวนของที่เก็บได้แล้ว
        for (int i = 0; i < itemCheckboxes.Count; i++)
        {
            if (itemCheckboxes[i] != null)
            {
                itemCheckboxes[i].isOn = (i < collectedItems);
            }
        }
    }

    private void OnObjectiveCompleted()
    {
        Debug.Log("เก็บของครบตามเป้าหมายแล้ว!");

        if (exitZoneOrClearUI != null)
        {
            exitZoneOrClearUI.SetActive(true);
        }

        if (autoClearWhenComplete)
        {
            CompleteStage();
        }
    }

    public void CompleteStage()
    {
        Debug.Log("Stage Clear! ผ่านด่านแล้ว");
    }
}