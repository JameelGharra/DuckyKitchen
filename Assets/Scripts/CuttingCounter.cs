using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter {

    [SerializeField] private KitchenObjectSO cutKitchenObjectSO;

    public override void Interact(Player player) {
        if (!HasKitchenObject() && player.HasKitchenObject()) {
            player.GetKitchenObject().SetKitchenObjectParent(this);
        }
        else if (HasKitchenObject() && !player.HasKitchenObject()) {
            GetKitchenObject().SetKitchenObjectParent(player);
        }
    }
    public override void InteractAlternate(Player player) {
        if(HasKitchenObject()) {
            GetKitchenObject().Destroy();
            KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);
        }
    }
}
