using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteractEnvironment : MonoBehaviour
{
	public float DistanceInteract = 3;
	public GameObject text;
	public GameObject dropHint;
	public GameObject itemPlace;
	public Item itemInHands;
	public GameObject camera;
	public GameObject world;

	private RaycastHit hitInteract;
	private bool isHit = false;

	void Update()
    {
		var isNoItem = false;
		var isItem = false;
		var hits = Physics.RaycastAll(camera.transform.position, camera.transform.forward, DistanceInteract);
		foreach (var hit in hits)
		{
				Debug.DrawLine(camera.transform.position, camera.transform.position + camera.transform.forward * DistanceInteract, Color.red);
				if (hit.transform.GetComponent<DoorScript.Door>() != null || hit.transform.GetComponent<Dialog>() != null || hit.transform.GetComponent<Item>() != null)
				{
					hitInteract = hit;
					//text.SetActive(true);
					isItem = true;
				}

		}

		if (isItem == true)
		{
			isHit = true;
			text.SetActive(true);
		}
		else 
		{
			isHit = false;
			text.SetActive(false);
		}
		
	}

	public void Interact(GameObject player)
	{
		Debug.Log("Interact");
		if (!isHit)
			return;

		if (hitInteract.transform.GetComponent<DoorScript.Door>() != null)
		{
			hitInteract.transform.GetComponent<DoorScript.Door>().OpenDoor();
		}
		else if (hitInteract.transform.GetComponent<Dialog>() != null)
		{
			if (hitInteract.transform.GetComponent<Dialog>().enabled)
				hitInteract.transform.GetComponent<Dialog>().Interact(player);
		}
		if (hitInteract.transform.GetComponent<Item>() != null && itemInHands == null)
		{
			var item = hitInteract.transform.GetComponent<Item>();
			if (!item.enabled)
				return;
			item.GetComponent<Rigidbody>().isKinematic = true;
			item.transform.parent = itemPlace.transform;
			item.transform.localPosition = itemPlace.transform.localPosition + item.position;
			item.transform.localRotation = item.quaternion;
			itemInHands = item;
			if (itemInHands != null)
			{
				dropHint.SetActive(true);
				itemInHands.ActionPickUp();

			}
		}
	}
	public void Drop()
	{
		if (itemInHands != null)
		{
			itemInHands.GetComponent<Rigidbody>().isKinematic = false;
			itemInHands.transform.parent = world.transform;
			itemInHands.Drop();
			itemInHands = null;
			dropHint.SetActive(false);
		}
	}
}
