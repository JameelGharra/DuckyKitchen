using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClearCounter : BaseCounter {
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player) {

        if(HasKitchenObject()) {
            // the counter is full
            if(!player.HasKitchenObject() ) {
                // the player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            else if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                // the player is carrying a plate
                if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    GetKitchenObject().Destroy();
            }
            else if(GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
                // player is carrying something, but not a plate
                if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                    player.GetKitchenObject().Destroy();
            }

        }
        else if(player.HasKitchenObject()) {
            // player carrying an item and the counter is clear
            player.GetKitchenObject().SetKitchenObjectParent(this);
        }
    }
}
