using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/DataBase")]
public class DataBase : ScriptableObject
{
    [SerializeField] private List<ItemData> itemDataList = null;
    public List<ItemData> ItemDataList => itemDataList;

    [SerializeField] private List<CharacterData> characterDataList = null;
    public List<CharacterData> CharacterDataList => characterDataList;
}
