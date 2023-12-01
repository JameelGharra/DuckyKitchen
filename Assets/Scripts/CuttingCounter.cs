using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter {

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private KitchenObjectSO GetSlicedKitchenObjectSO(KitchenObjectSO inputKitchenObjectSO) {
        foreach(CuttingRecipeSO cuttingRecipseSO in cuttingRecipeSOArray) {
            if (cuttingRecipseSO.input == inputKitchenObjectSO) {
                return cuttingRecipseSO.output;
            }
        }
        return null;
    }

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
            KitchenObjectSO slicedKitchenObject = GetSlicedKitchenObjectSO(GetKitchenObject().GetKitchenObjectSO());
            GetKitchenObject().Destroy();
            KitchenObject.SpawnKitchenObject(slicedKitchenObject, this);
        }
    }
}
