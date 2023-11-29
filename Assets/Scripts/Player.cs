using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour { 
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private GameInput gameInput;
    private bool isWalking;

    private void Update() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float playerRadius = 0.57f, playerHeight = 2f, moveDistance = moveSpeed * Time.deltaTime;
        // Capsule cast requires the top and the bottom of the virtual capsule fired
        bool canMove = !Physics.CapsuleCast(transform.position, 
            transform.position+ Vector3.up*playerHeight, 
            playerRadius, 
            moveDir, 
            moveDistance
        );
        if(!canMove) {
            // Attempt to move towards either X or Z if we cannot move on both
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0);
            canMove = !Physics.CapsuleCast(transform.position,
            transform.position + Vector3.up * playerHeight,
            playerRadius,
            moveDirX,
            moveDistance
            );
            if (canMove) {
                // Can move only on X
                moveDir = moveDirX;
            }
            else {
                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z);
                canMove = !Physics.CapsuleCast(transform.position,
                transform.position + Vector3.up * playerHeight,
                playerRadius,
                moveDirZ,
                moveDistance
                );
                if(canMove) {
                    moveDir = moveDirZ;
                }
            }
        }
        if (canMove) {
            transform.position += moveDir * Time.deltaTime * moveSpeed;
        }
        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime*rotateSpeed);
    }
    public bool IsWalking() {
        return isWalking;
    }
}
