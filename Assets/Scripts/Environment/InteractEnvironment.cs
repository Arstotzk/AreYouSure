using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractEnvironment : MonoBehaviour
{
	public float DistanceInteract = 3;
	public GameObject text;
	private RaycastHit hit;


	void Update()
    {
		if (Physics.Raycast(transform.position, transform.forward, out hit, DistanceInteract))
		{
			if (hit.transform.GetComponent<DoorScript.Door>() != null || hit.transform.GetComponent<Dialog>() != null)
			{
				text.SetActive(true);
			}
			else
			{
				text.SetActive(false);
			}
		}
		else
		{
			text.SetActive(false);
		}
	}

	public void Interact(GameObject player)
	{
		Debug.Log("Interact");
		if (hit.transform.GetComponent<DoorScript.Door>() != null)
		{
			hit.transform.GetComponent<DoorScript.Door>().OpenDoor();
		}
		else if (hit.transform.GetComponent<Dialog>() != null)
		{
			hit.transform.GetComponent<Dialog>().StartDialog(player);
		}
	}
}
