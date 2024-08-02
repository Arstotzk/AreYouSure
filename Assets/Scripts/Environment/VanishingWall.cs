using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingWall : MonoBehaviour
{
    public GameObject door;
    private Renderer renderer;
    private bool isExist;
    void Start()
    {
        renderer = GetComponent<Renderer>();
        isExist = true;
    }
    void Update()
    {
        if (!PlayerLooking() && isExist == false)
        {
            gameObject.SetActive(false);
            door.SetActive(true);
        }
    }

    private bool PlayerLooking()
    {
        var isPlayerLooking = renderer.isVisible;
        if (isPlayerLooking)
            isExist = false;
        return isPlayerLooking;
    }
}
