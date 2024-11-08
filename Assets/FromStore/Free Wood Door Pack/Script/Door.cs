﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DoorScript
{
	[RequireComponent(typeof(AudioSource))]
	
	
	public class Door : MonoBehaviour 
	{
	
		public bool open;
		public bool isCanOpen;
		private bool isTryOpen;
		private int framesTry = 5;
		private int frame;
		public float smooth = 1.0f;
		public float angryClose = 1.0f;
		float DoorOpenAngle = -90.0f;
	    float DoorCloseAngle = 0.0f;
		float DoorOpenAngleTry = -5f;
		public AudioSource asource;
		public AudioClip openDoor,closeDoor, tryOpen;
		void Start () 
		{
			asource = GetComponent<AudioSource> ();
		}
		
		void Update () {
			if (open)
			{
	            var target = Quaternion.Euler (0, DoorOpenAngle, 0);
	            transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 5 * smooth);
		
			}
			else
			{
	            var target1= Quaternion.Euler (0, DoorCloseAngle, 0);
	            transform.localRotation = Quaternion.Slerp(transform.localRotation, target1, Time.deltaTime * 5 * smooth * angryClose);
		
			}
			if (!isCanOpen)
			{
				if (isTryOpen)
				{
					frame++;
					if (frame > framesTry)
						isTryOpen = false;
					var target = Quaternion.Euler(0, DoorOpenAngleTry, 0);
					transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 5 * smooth);

				}
				else
				{
					var target1 = Quaternion.Euler(0, DoorCloseAngle, 0);
					transform.localRotation = Quaternion.Slerp(transform.localRotation, target1, Time.deltaTime * 5 * smooth);
				}
			}
		}
	
		public void OpenDoor()
		{
			if (!isCanOpen)
			{
				frame = 0;
				isTryOpen = true;
				asource.clip = tryOpen;
				asource.Play();
				return;
			}

			open =!open;
			asource.clip = open?openDoor:closeDoor;
			asource.Play ();
		}
	}
}