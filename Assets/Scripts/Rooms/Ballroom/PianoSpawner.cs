using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rooms.SymbolsLabyrinth
{
    public class PianoSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject tileText;

        private readonly string[] phrases = new[]
        {
            "C",
            "C#",
            "D",
            "E",
            "F",
            "G",
            "A",
            "B"
        };
        public void SpawnLibrary(int tileIndex)
        {
            Debug.Log("Tuile # " + (tileIndex + 1) + " à partir de la gauche" );
            tileText.GetComponent<Text>().text = phrases[tileIndex];
        }

        public void OnTriggerEnter(Collider other)
        {
            tileText.SetActive(true);
        }

        public void OnTriggerExit(Collider other)
        {
            tileText.SetActive(false);
        }
    }
}