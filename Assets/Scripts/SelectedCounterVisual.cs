using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter counter;
    [SerializeField] private GameObject[] visualGameObjectArray;
    // If set to Awake instead, there is a chance that we have a null ref. exception
    private void Start() {
        Player.Instance.onSelectedCounterChange += Player_OnSelectedCounterChanged;
    }
    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e) {
        if(e.selectedCounter == counter) {
            Show();
        }
        else {
            Hide();
        }
    }
    private void Show() {
        foreach (GameObject gameObject in visualGameObjectArray) {
            gameObject.SetActive(true);
        }
    }
    private void Hide() {
        foreach (GameObject gameObject in visualGameObjectArray) {
            gameObject.SetActive(false);
        }
    }
}
