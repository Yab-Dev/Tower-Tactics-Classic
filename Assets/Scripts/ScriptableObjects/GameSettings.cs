using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Settings", menuName = "Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("Video")]
    public bool fullscreen;

    [Header("Audio")]
    public float masterVolume;
}
