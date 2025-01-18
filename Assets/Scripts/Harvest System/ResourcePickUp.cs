using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePickUp : MonoBehaviour
{
    [field : SerializeField] public Resource InventoryItem { get; private set; }
    [field: SerializeField] public int Quantity { get; set; } = 1;
    // [SerializeField] AudioSource audioSource;
    [SerializeField] float duration = 3f;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = InventoryItem.Icon;
    }

    public void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(AnimateItemPickUp());
    }

    private IEnumerator AnimateItemPickUp()
    {
        // audioSource.Play();
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / duration);
            yield return null;
        }

        Destroy(gameObject);
    }
}
