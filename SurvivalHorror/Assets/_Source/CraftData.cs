using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftData",menuName = "SO/NewCraftData")]
public class CraftData : ScriptableObject
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] List<Resource> resourceForCraft;

    public GameObject ItemPrefab => itemPrefab;
    public List<Resource> ResourceForCraft => resourceForCraft;
}

[Serializable]
public class Resource
{
    public ResourceType resourceType;
    public int amount;
}
