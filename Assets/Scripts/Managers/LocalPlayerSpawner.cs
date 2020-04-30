using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField, Tooltip("Place transforms of empty game objects in-scene here to use as the spawn points for each player.")]
    private Transform p1spawn, p2spawn;

    private PlayerController p1pc, p2pc;

    // Start is called before the first frame update
    void Start()
    {
        StartSpawnPlayers();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void StartSpawnPlayers()
    {
        GameObject p1go = Instantiate(playerPrefab, p1spawn.position, p1spawn.rotation);
        GameObject p2go = Instantiate(playerPrefab, p2spawn.position, p2spawn.rotation);

        p1pc = p1go.GetComponent<PlayerController>();
        p2pc = p2go.GetComponent<PlayerController>();

        p1pc.AssignRotation(p1spawn.eulerAngles);
        p2pc.AssignRotation(p2spawn.eulerAngles);

        p1pc.PlayerNumber = 1;
        p2pc.PlayerNumber = 2;

        SetPlayerCameras();
    }

    private void SetPlayerCameras()
    {
        p1pc.PlayerCamera.rect = new Rect(0f, 0.5f, 1f, 0.5f);
        p2pc.PlayerCamera.rect = new Rect(0f, 0f, 1f, 0.5f);
    }
}
