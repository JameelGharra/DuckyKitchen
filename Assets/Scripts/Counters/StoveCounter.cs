using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IProgressable {

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;

    public event EventHandler<IProgressable.OnProgressChangedEventArgs> OnProgressChanged;
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO; // caching instead of constantly searching
    private State currentState;
    public enum State { // had to make it public to not get the inconsistent accessbility err. for EventArgs
        Idle,
        Frying,
        Fried,
        Burned,
    }

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs {
        public State state;
    }

    private void Start() {
        currentState = State.Idle;
    }
    private void Update() {
        if (HasKitchenObject()) {
            switch (currentState) {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IProgressable.OnProgressChangedEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
                        GetKitchenObject().Destroy();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        fryingRecipeSO = GetFryingRecipeSOFromInput(GetKitchenObject().GetKitchenObjectSO());
                        currentState = State.Fried;
                        fryingTimer = 0f;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = currentState
                        }) ;
                    }
                    break;
                case State.Fried:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IProgressable.OnProgressChangedEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
                        GetKitchenObject().Destroy();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        currentState = State.Burned;
                        fryingTimer = 0f;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = currentState
                        });
                        OnProgressChanged?.Invoke(this, new IProgressable.OnProgressChangedEventArgs {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Burned:

                    break;
            }
        }
    }

    private FryingRecipeSO GetFryingRecipeSOFromInput(KitchenObjectSO kitchenObjectSO) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
            if (fryingRecipeSO.input == kitchenObjectSO) {
                return fryingRecipeSO;
            }
        }
        return null;
    }
    private KitchenObjectSO GetOutputKitchenObjectSO(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOFromInput(inputKitchenObjectSO);
        return fryingRecipeSO?.output;
    }

    public override void Interact(Player player) {
        if (!HasKitchenObject() && player.HasKitchenObject() &&
            GetOutputKitchenObjectSO(player.GetKitchenObject().GetKitchenObjectSO()) != null) {
            player.GetKitchenObject().SetKitchenObjectParent(this);
            fryingRecipeSO = GetFryingRecipeSOFromInput(GetKitchenObject().GetKitchenObjectSO());
            currentState = State.Frying;
            fryingTimer = 0f;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                state = currentState
            });
            OnProgressChanged?.Invoke(this, new IProgressable.OnProgressChangedEventArgs {
                progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
            });
        }
        else if (HasKitchenObject() && !player.HasKitchenObject()) {
            GetKitchenObject().SetKitchenObjectParent(player);
            currentState = State.Idle;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                state = currentState
            });
            OnProgressChanged?.Invoke(this, new IProgressable.OnProgressChangedEventArgs {
                progressNormalized = 0f
            });
        }
    }
}
