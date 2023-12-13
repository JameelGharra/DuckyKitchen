using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {
    [SerializeField] private RecipeListSO recipeListSO;
    private const int MAX_WAITING_RECIPE = 4;
    private const float spawnRecipeTimerMax = 4f;
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;

    public static DeliveryManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }
    private void Update() {
        spawnRecipeTimer -= Time.deltaTime;
        if(spawnRecipeTimer <= 0f) {
            spawnRecipeTimer = spawnRecipeTimerMax;
        }
        if(waitingRecipeSOList.Count < MAX_WAITING_RECIPE) {
            RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
            Debug.Log(waitingRecipeSO.recipeName);
            waitingRecipeSOList.Add(waitingRecipeSO);
        }
    }
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        foreach(KitchenObjectSO kitchenObject in plateKitchenObject.GetKitchenObjectSOList()) {
            Debug.Log(kitchenObject.objectName);
        }
        for(int i = 0; i < waitingRecipeSOList.Count; ++i) {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if(waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) {
                bool plateContentsMatchesRecipe = true;
                foreach(KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList) {
                    bool ingredientFound = false;
                    foreach(KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
                        if(plateKitchenObjectSO == recipeKitchenObjectSO) {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound) {
                        plateContentsMatchesRecipe = false;
                        break;
                    }
                }
                if(plateContentsMatchesRecipe) {
                    // The player delivered the correct recipe
                    Debug.Log("This is a valid recipe delivery");
                    waitingRecipeSOList.RemoveAt(i);
                    return;
                }
            }
        }
        Debug.Log("Incorrect recipe delivery");
    }
}
