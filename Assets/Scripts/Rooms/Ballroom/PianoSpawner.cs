using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Rooms.SymbolsLabyrinth
{
	public class PianoSpawner : MonoBehaviour
	{
		[SerializeField] private GameObject imageNode;
		[SerializeField] private Sprite[] feuillesMusique;


		public void SpawnLibrary(int tileIndex)
		{
			Debug.Log("Tuile # " + (tileIndex + 1) + " à partir de la gauche" );
			imageNode.GetComponent<Image>().sprite = feuillesMusique[tileIndex];
		}

		public void OnTriggerEnter(Collider other)
		{
			imageNode.SetActive(true);
		}

		public void OnTriggerExit(Collider other)
		{
			imageNode.SetActive(false);
		}
	}
}
