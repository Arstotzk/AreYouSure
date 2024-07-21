using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractEnvironment : MonoBehaviour
{
	public float DistanceInteract = 3;
	public GameObject text;
	public GameObject itemPlace;
	public Item itemInHands;
	public GameObject camera;
	public GameObject world;

	private RaycastHit hit;
	private bool isNoHit = true;

	void Update()
    {
		if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, DistanceInteract))
		{
			if (hit.transform.GetComponent<DoorScript.Door>() != null || hit.transform.GetComponent<Dialog>() != null || hit.transform.GetComponent<Item>() != null)
			{
				text.SetActive(true);
				isNoHit = false;
			}
			else
			{
				text.SetActive(false);
				isNoHit = true;
			}
		}
		else
		{
			text.SetActive(false);
			isNoHit = true;
		}
	}

	public void Interact(GameObject player)
	{
		Debug.Log("Interact");
		if (isNoHit)
			return;

		if (hit.transform.GetComponent<DoorScript.Door>() != null)
		{
			hit.transform.GetComponent<DoorScript.Door>().OpenDoor();
		}
		else if (hit.transform.GetComponent<Dialog>() != null)
		{
			hit.transform.GetComponent<Dialog>().Interact(player);
		}
		else if (hit.transform.GetComponent<Item>() != null && itemInHands == null)
		{
			var item = hit.transform.GetComponent<Item>();
			item.transform.position = itemPlace.transform.position;
			item.transform.rotation = new Quaternion(0f,0f,0f,0f);
			item.GetComponent<Rigidbody>().isKinematic = true;
			item.transform.parent = itemPlace.transform;
			itemInHands = item;
		}
	}
	public void Drop()
	{
		if (itemInHands != null)
		{
			itemInHands.GetComponent<Rigidbody>().isKinematic = false;
			itemInHands.transform.parent = world.transform;
			itemInHands = null;
		}
	}
}
