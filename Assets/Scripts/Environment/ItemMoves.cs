using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMoves : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isMoved;
    private Renderer renderer;
    private Rigidbody rigidbody;
    void Start()
    {
        isMoved = false;
        renderer = GetComponent<Renderer>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerLooking() && isMoved == false) 
        {
            MoveRandom();
        }
    }
    private bool PlayerLooking()
    {
        var isPlayerLooking = renderer.isVisible;
        if (isPlayerLooking)
            isMoved = false;
        return isPlayerLooking;
    }
    private void MoveRandom() 
    {
        Debug.Log("MoveRandom:" + this.name);
        Vector3 forceVector = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(0f, 2f));
        rigidbody.AddForce(forceVector, ForceMode.Impulse);
        isMoved = true;
    }
}
