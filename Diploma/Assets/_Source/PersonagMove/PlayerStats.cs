using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using GooglyEyesGames.FusionBites;
using System;


public class PlayerStats : NetworkBehaviour
{
    [Networked(OnChanged = nameof(UpdatePlayerName))] public NetworkString<_32> PlayerName { get; set; }

    [SerializeField] TextMeshPro playerNameLabel;

    private void Start()
    {
        if (this.HasStateAuthority)
        {
            PlayerName = FusionConnector.instance._playerName;
        }
    }

    protected static void UpdatePlayerName(Changed<PlayerStats> changed)
    {
        changed.Behaviour.playerNameLabel.text = changed.Behaviour.PlayerName.ToString();
    }

}
