using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {
    private Player player;
    private float footstepTimer;
    private float footstepTimerMax = 0.1f; // 10 times per second
    private const float FOOTSTEPS_VOLUME = 1f;
    private void Awake() {
        player = GetComponent<Player>();
    }
    private void Update() {
        footstepTimer -= Time.deltaTime;
        if(footstepTimer < 0f) {
            footstepTimer = footstepTimerMax;
            if (player.IsWalking()) {
                SoundManager.instance.PlayFootstepsSound(player.transform.position, FOOTSTEPS_VOLUME);
            }
        }
    }
}
