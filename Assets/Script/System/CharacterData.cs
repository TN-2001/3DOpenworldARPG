using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/CharacterData")]
public class CharacterData : ScriptableObject
{
    [SerializeField] private new string name = null;
    public string Name => name;
}
