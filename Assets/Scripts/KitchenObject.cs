using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;
    public KitchenObjectSO GetKitchenObjectSO() { 
        return kitchenObjectSO; 
    }
    public IKitchenObjectParent GetKitchenObjectParent() { 
        return kitchenObjectParent; 
    }
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent) {
        if(this.kitchenObjectParent != null) { // the kitchen object is responsible of the switch
            this.kitchenObjectParent.ClearKitchenObject();
            if (kitchenObjectParent.HasKitchenObject()) {
                Debug.LogError("IKitchenObjectParent already has a KitchenObject!");
            }
        }
        kitchenObjectParent.SetKitchenObject(this);
        this.kitchenObjectParent = kitchenObjectParent;
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject) {
        if (this is PlateKitchenObject) {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        plateKitchenObject = null;
        return false;
    }
    public void Destroy() {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(this.gameObject);
    }
    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent) {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        return kitchenObject;
    }
}
