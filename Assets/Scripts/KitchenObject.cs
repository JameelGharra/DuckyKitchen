using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private ClearCounter clearCounter;
    public KitchenObjectSO GetKitchenObjectSO() { 
        return kitchenObjectSO; 
    }
    public ClearCounter GetClearCounter() { 
        return clearCounter; 
    }
    public void SetClearCounter(ClearCounter clearCounter) {
        if(this.clearCounter != null) { // the kitchen object is responsible of the switch
            this.clearCounter.ClearKitchenObject();
            if (clearCounter.HasKitchenObject()) {
                Debug.LogError("Counter already has a KitchenObject!");
            }
        }
        clearCounter.SetKitchenObject(this);
        this.clearCounter = clearCounter;
        transform.parent = clearCounter.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
}
