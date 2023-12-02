using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter {

    [SerializeField] private float spawnPlateTimerMax = 4f;
    [SerializeField] private int platesSpawnedAmountMax = 4;
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private float spawnPlateTimer;
    private int platesSpawnedAmount;

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    private void Update() {
        spawnPlateTimer += Time.deltaTime;
        if(spawnPlateTimer > spawnPlateTimerMax) {
            if (platesSpawnedAmount < platesSpawnedAmountMax) {
                ++platesSpawnedAmount;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
            spawnPlateTimer = 0f;
        }
    }
    public override void Interact(Player player) {
        if(!player.HasKitchenObject() && platesSpawnedAmount > 0) {
            --platesSpawnedAmount;
            KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
            OnPlateRemoved?.Invoke(this, EventArgs.Empty);
        }
    }
}
