using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilding : MonoBehaviour
{
    [SerializeField] private float offset = 0.16f;
    private Transform preview_item = null;
    private BuildingItem building_item = null;
    private BuildingItem prefab_item = null;


    private PlayerInput playerInput;

    private readonly Dictionary<string, BuildingItem> objectPooling = new();

    Vector2 pos = Vector2.zero;
    Vector3 rot = Vector3.zero;


    public BuildingItem testItem;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        ChangeBuildingItem(testItem);
    }
    private void Update()
    {
        ChangePreviewItemPosition();
        if (preview_item != null)
        {
            preview_item.position = pos;

            if (playerInput.onFoot.Build.triggered)
            {
                if (building_item.CanBuild())
                {
                    BuildObject();
                }
                else
                {

                }
            }
        }
    }

    public void ChangePreviewItemPosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        float newXPos = Mathf.Round(worldPosition.x / offset) * offset;
        float newYPos = Mathf.Round(worldPosition.y / offset) * offset;
        pos = new(newXPos, newYPos);
    }

    public void ChangeBuildingItem(BuildingItem item)
    {
        if (preview_item != null)
        {
            preview_item = null;
        }
        prefab_item = item;
        if (item != null)
        {
            string objectName = item.name;
            if (objectPooling.ContainsKey(objectName))
            {
                building_item = objectPooling[objectName];
            }
            else
            {
                building_item = Instantiate(item, pos, Quaternion.Euler(rot));
                objectPooling[objectName] = building_item;
            }
            building_item.gameObject.SetActive(true);
            preview_item = building_item.transform;
        }
        else
        {
            building_item.gameObject.SetActive(false);
            building_item = null;
        }
    }
    public void BuildObject()
    {
        if (prefab_item != null)
        {
            BuildingItem tempItem = Instantiate(prefab_item, pos, Quaternion.Euler(rot));
            tempItem.BuildingInit();
        }
    }
}
