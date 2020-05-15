using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnTimer
{
    public int pNum { get; private set; }
    public bool WillRespawn;
    public float RespawnTimer;

    public PlayerRespawnTimer(int pNum)
    {
        this.pNum = pNum;
        this.WillRespawn = false;
        this.RespawnTimer = 0f;
    }
}

public class LocalPlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject p1Prefab, p2Prefab;

    [SerializeField, Tooltip("Place transforms of empty game objects in-scene here to use as the spawn points for each player.")]
    private Transform p1spawn, p2spawn;

    private GameObject p1go, p2go;
    private PlayerController p1pc, p2pc;

    [SerializeField]
    private float respawnCooldown;

    private List<PlayerRespawnTimer> respawnTimers;
    private List<PlayerRespawnTimer> activeRespawnTimers;

    private void Awake()
    {
        StartInitRespawnTimers();

        StartSpawnPlayers();
    }

    private void StartInitRespawnTimers()
    {
        respawnTimers = new List<PlayerRespawnTimer>();

        activeRespawnTimers = new List<PlayerRespawnTimer>();

        for (int i = 0; i < 2; i++)
        {
            respawnTimers.Add(new PlayerRespawnTimer(i + 1));
        }
    }

    private void StartSpawnPlayers()
    {
        p1go = Instantiate(p1Prefab, p1spawn.position, p1spawn.rotation);
        p2go = Instantiate(p2Prefab, p2spawn.position, p2spawn.rotation);

        p1pc = p1go.GetComponent<PlayerController>();
        p2pc = p2go.GetComponent<PlayerController>();

        p1pc.PlayerSpawnerInstance = this;
        p2pc.PlayerSpawnerInstance = this;

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

    // Update is called once per frame
    void Update()
    {
        UpdateCheckRespawnPlayers();
    }

    private void UpdateCheckRespawnPlayers()
    {
        foreach (PlayerRespawnTimer t in activeRespawnTimers)
        {
            t.RespawnTimer += Time.deltaTime;

            if (t.RespawnTimer >= respawnCooldown)
            {
                t.WillRespawn = false;
                t.RespawnTimer = 0f;

                switch (t.pNum)
                {
                    case 1:
                        p1go.SetActive(true);
                        break;
                    case 2:
                        p2go.SetActive(true);
                        break;
                }
                
                p1go.transform.position = p1spawn.position;
                p1go.transform.rotation = p1spawn.rotation;
                p1pc.AssignRotation(p1spawn.eulerAngles);
                p1pc.rb.velocity = Vector3.zero;
                p1pc.ChangeMovementState(PlayerController.MovementState.jumping);
                p1pc.HitboxOnGround = false;

                p2go.transform.position = p2spawn.position;
                p2go.transform.rotation = p2spawn.rotation;
                p2pc.AssignRotation(p2spawn.eulerAngles);
                p2pc.rb.velocity = Vector3.zero;
                p2pc.ChangeMovementState(PlayerController.MovementState.jumping);
                p2pc.HitboxOnGround = false;
            }
        }

        foreach (PlayerRespawnTimer t in respawnTimers)
        {
            if (activeRespawnTimers.Contains(t) && !t.WillRespawn)
            {
                activeRespawnTimers.Remove(t);
            }
        }
    }

    public void SpawnPlayerAfterCooldown(PlayerController pc)
    {
        respawnTimers[pc.PlayerNumber - 1].WillRespawn = true;

        activeRespawnTimers.Add(respawnTimers[pc.PlayerNumber - 1]);
    }
}
