using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    [SerializeField] private List<CraftData> craftables;
    [SerializeField] private Transform spawnpoint;

    public List<CraftData> Craftables => craftables;
    public Transform Spawnpoint => spawnpoint;
}
