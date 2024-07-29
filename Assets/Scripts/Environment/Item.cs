using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    public string itemName;
    public bool isMimic;
    public Quaternion quaternion = new Quaternion(70f, 50f, 50f, 0f);
    public Vector3 position = new Vector3(0f, 0f, 0f);
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void ActionPickUp()
    {

    }
    public virtual void Drop()
    {

    }
}
