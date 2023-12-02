using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesIconUI : MonoBehaviour {
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate; // meant only for duplication

    private void Awake() {
        iconTemplate.gameObject.SetActive(false); // hiding the sample
    }
    private void Start() {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }
    private void UpdateVisual() {
        foreach(Transform child in transform) {
            if(child != iconTemplate)
                Destroy(child.gameObject);
        }
        foreach(KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.GetComponent<PlateIconsSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
            iconTransform.gameObject.SetActive(true); // since was not sample active
        }
    }
    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e) {
        UpdateVisual();
    }
}
