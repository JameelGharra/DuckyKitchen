using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter {

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;
    public class OnProgressChangedEventArgs : EventArgs {
        public float progressNormalized;
    }

    private CuttingRecipeSO GetCuttingRecipseSOFromInput(KitchenObjectSO kitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipseSO in cuttingRecipeSOArray) {
            if (cuttingRecipseSO.input == kitchenObjectSO) {
                return cuttingRecipseSO;
            }
        }
        return null;
    }
    private KitchenObjectSO GetSlicedKitchenObjectSO(KitchenObjectSO inputKitchenObjectSO) {
        CuttingRecipeSO cuttingRecipe = GetCuttingRecipseSOFromInput(inputKitchenObjectSO);
        return cuttingRecipe?.output;
    }
    public override void Interact(Player player) {
        if (!HasKitchenObject() && player.HasKitchenObject() && 
            GetSlicedKitchenObjectSO(player.GetKitchenObject().GetKitchenObjectSO()) != null) {
            player.GetKitchenObject().SetKitchenObjectParent(this);
            cuttingProgress = 0;
            CuttingRecipeSO cuttingRecipe = GetCuttingRecipseSOFromInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs {
                progressNormalized = (float)cuttingProgress / cuttingRecipe.cuttingProgressMax
            });
        }
        else if (HasKitchenObject() && !player.HasKitchenObject()) {
            GetKitchenObject().SetKitchenObjectParent(player);
        }
    }
    public override void InteractAlternate(Player player) {
        if(HasKitchenObject() && GetSlicedKitchenObjectSO(GetKitchenObject().GetKitchenObjectSO()) != null) {
            ++cuttingProgress;

            CuttingRecipeSO cuttingRecipe = GetCuttingRecipseSOFromInput(GetKitchenObject().GetKitchenObjectSO());
            OnCut?.Invoke(this, EventArgs.Empty);
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs {
                progressNormalized = (float)cuttingProgress / cuttingRecipe.cuttingProgressMax
            });
            if (cuttingProgress >= cuttingRecipe.cuttingProgressMax) {
                KitchenObjectSO slicedKitchenObject = GetSlicedKitchenObjectSO(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().Destroy();
                KitchenObject.SpawnKitchenObject(slicedKitchenObject, this);
            }
        }
    }
}
