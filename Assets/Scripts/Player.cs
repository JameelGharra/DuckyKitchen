using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private Vector3 lastinteractDirection;
    private ClearCounter selectedCounter;
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float playerRadius = 0.57f;
    [SerializeField] private float maxInteractionRange = 2f;
    [SerializeField] private LayerMask counterLayerMask;

    private bool isWalking;

    private bool IsValidMoveDirection(Vector3 moveDirection, float moveDistance) {
        // Capsule cast requires the top and the bottom of the virtual capsule fired
        return !Physics.CapsuleCast(transform.position,
            transform.position + Vector3.up * playerHeight,
            playerRadius,
            moveDirection,
            moveDistance
            );
    }
    private Vector3 getValidMoveDirection(Vector3 moveDirection, float moveDistance) {
        if(IsValidMoveDirection(moveDirection, moveDistance)) {
            return moveDirection;
        }
        // Attempt to move towards either X or Z if we cannot move on 
        Vector3 moveDirX = new Vector3(moveDirection.x, 0, 0).normalized; // we normalize to keep eliminate speed cuts we did earlier
        if (IsValidMoveDirection(moveDirX, moveDistance))
            return moveDirX;
        // Attempt only Z movement
        Vector3 moveDirZ = new Vector3(0, 0, moveDirection.z).normalized;
        if (IsValidMoveDirection(moveDirZ, moveDistance))
            return moveDirZ;

        return Vector3.zero;
    }
    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 intendedDirection = new(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        Vector3 moveDirection = getValidMoveDirection(intendedDirection, moveDistance);
        isWalking = moveDirection != Vector3.zero;
        if (isWalking) {
            transform.position += moveSpeed * Time.deltaTime * moveDirection;
        }
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }
    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        RaycastHit hitObject;
        if (moveDirection != Vector3.zero) {
            lastinteractDirection = moveDirection;
        }
        if (Physics.Raycast(transform.position, lastinteractDirection, out hitObject, maxInteractionRange, counterLayerMask)) {
            if (hitObject.transform.TryGetComponent(out ClearCounter clearCounter)) {
                if(clearCounter != selectedCounter) {
                    selectedCounter = clearCounter;
                }
            }
            else {
                selectedCounter = null;
            }
        }
        else {
            selectedCounter = null;
        }
        Debug.Log(selectedCounter);
    }
    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if(selectedCounter != null) {
            selectedCounter.Interact();
        }
    }
    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction; 
    }


    private void Update() {
        HandleMovement();
        HandleInteractions();
    }
    public bool IsWalking() {
        return isWalking;
    }
}
