using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingItem : MonoBehaviour
{

    public static string Left = "left";
    public static string Right = "right";
    public static string Top = "top";
    public static string Bottom = "bottom";
    private Dictionary<BuildingItemPosition, Sprite> textStore = new();

    [SerializeField] private List<ColliderPositionItem> colliderPositions = new();
    [SerializeField] private LayerMask checkMask;
    [SerializeField] private float rayCastDistance = 0.09f;
    [SerializeField] private int main_layer;

    SpriteRenderer mainRenderer;

    bool canBuild = true;

    private void Start()
    {
        for (int i = 0; i < colliderPositions.Count; i++)
        {
            textStore[colliderPositions[i].position] = colliderPositions[i].texture;
        }

        if (TryGetComponent<SpriteRenderer>(out mainRenderer))
        {

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;
        canBuild = ((1 << layer) & checkMask) == 0;
    }

    public bool CanBuild()
    {
        return canBuild;
    }
    public void BuildingInit()
    {
        gameObject.layer = main_layer;
        for (int i = 0; i < colliderPositions.Count; i++)
        {
            textStore[colliderPositions[i].position] = colliderPositions[i].texture;
        }

        if (TryGetComponent<SpriteRenderer>(out mainRenderer))
        {

        }
        Vector3 position = transform.position;
        Dictionary<string, bool> rayCastStore = new();

        CheckRayCastPosition(position, transform.right * -1f, Left, rayCastStore);
        CheckRayCastPosition(position, transform.right, Right, rayCastStore);
        CheckRayCastPosition(position, transform.up * -1f, Top, rayCastStore);
        CheckRayCastPosition(position, transform.up, Bottom, rayCastStore);

        ChangeSprite(rayCastStore);
    }

    public void CheckRayCastPosition(Vector2 posi, Vector2 dir, string position, Dictionary<string, bool> store)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(posi, dir, rayCastDistance, checkMask);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject != this)
            {
                if (hits[i].collider.gameObject.TryGetComponent<BuildingItem>(out var item))
                {
                    item.ReloadSprite();
                }
            }
        }
        store[position] = hits.Length > 1;
    }

    public void CheckRayCastPositionSprite(Vector2 posi, Vector2 dir, string position, Dictionary<string, bool> store)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(posi, dir, rayCastDistance, checkMask);
        store[position] = hits.Length > 1;
    }

    public void ReloadSprite()
    {
        Vector3 position = transform.position;
        Dictionary<string, bool> rayCastStore = new();

        CheckRayCastPositionSprite(position, transform.right * -1f, Left, rayCastStore);
        CheckRayCastPositionSprite(position, transform.right, Right, rayCastStore);
        CheckRayCastPositionSprite(position, transform.up * -1f, Top, rayCastStore);
        CheckRayCastPositionSprite(position, transform.up, Bottom, rayCastStore);

        ChangeSprite(rayCastStore);
    }

    public void ChangeSprite(Dictionary<string, bool> store)
    {
        BuildingItemPosition position;

        if (store[Top] && !store[Bottom])
        {
            position = BuildingItemPosition.Corner_Top_Center;
            if (store[Left] && !store[Right])
            {
                position = BuildingItemPosition.Corner_Top_Right;
            }
            if (!store[Left] && store[Right])
            {
                position = BuildingItemPosition.Corner_Top_Left;
            }
        }
        else if (!store[Top] && store[Bottom])
        {
            position = BuildingItemPosition.Corner_Down_Center;
            if (store[Left] && !store[Right])
            {
                position = BuildingItemPosition.Corner_Down_Right;
            }

            if (!store[Left] && store[Right])
            {
                position = BuildingItemPosition.Corner_Down_Left;
            }
        }
        else if (store[Top] && store[Bottom])
        {
            position = BuildingItemPosition.Center;
            if (!store[Left] && store[Right])
            {
                position = BuildingItemPosition.Left_Center;
            }

            if (store[Left] && !store[Right])
            {
                position = BuildingItemPosition.Right_Center;
            }

        }
        else
        {
            position = BuildingItemPosition.Center;
            if (!store[Left] && store[Right])
            {
                position = BuildingItemPosition.Left_Center;
            }

            if (store[Left] && !store[Right])
            {
                position = BuildingItemPosition.Right_Center;
            }
        }

        mainRenderer.sprite = GetSprite(position);
    }


    public Sprite GetSprite(BuildingItemPosition position)
    {
        if (textStore.ContainsKey(position))
        {
            return textStore[position];
        }

        return mainRenderer != null ? mainRenderer.sprite : null;
    }
}
[System.Serializable]
public class ColliderPositionItem
{
    public Sprite texture;
    public BuildingItemPosition position;
}
