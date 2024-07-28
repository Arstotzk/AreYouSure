using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ItemChangeCameraEffects : Item
{
    public Volume volume;
    public float volumeChangeStrenght = 0.001f;
    private bool isItemInHands;

    void Start()
    {
        isItemInHands = false;
    }

    void Update()
    {
        if (isMimic)
        {
            var newVolume = volume.weight + volumeChangeStrenght * (isItemInHands == true ? 1 : -1);
            if (newVolume > 1)
                volume.weight = 1;
            else if (newVolume < 0)
                volume.weight = 0;
            else
                volume.weight = newVolume;
        }
    }
    public override void ActionPickUp()
    {
        if (isMimic)
            isItemInHands = true;
    }
    public override void Drop()
    {
        if (isMimic)
            isItemInHands = false;
    }
}
