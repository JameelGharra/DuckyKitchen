using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour { 
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float playerRadius = 0.57f;

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
        Vector3 moveDirX = new Vector3(moveDirection.x, 0, 0);
        if (IsValidMoveDirection(moveDirX, moveDistance))
            return moveDirX;
        // Attempt only Z movement
        Vector3 moveDirZ = new Vector3(0, 0, moveDirection.z);
        if (IsValidMoveDirection(moveDirZ, moveDistance))
            return moveDirZ;

        return Vector3.zero;
    }

    private void Update() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 intendedDirection = new(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        Vector3 moveDirection = getValidMoveDirection(intendedDirection, moveDistance);
        if (moveDirection != Vector3.zero) {
            transform.position += moveSpeed * Time.deltaTime * moveDirection;
        }
        isWalking = moveDirection != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime*rotateSpeed);
    }
    public bool IsWalking() {
        return isWalking;
    }
}
