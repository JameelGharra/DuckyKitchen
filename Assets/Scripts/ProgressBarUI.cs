using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {

    [SerializeField] private Image barImage;
    [SerializeField] private GameObject progressableGameObject;
    
    private IProgressable progressable;

    private void Start() {
        progressable = progressableGameObject.GetComponent<IProgressable>();
        if(progressable == null ) { // safety check
            Debug.LogError($"Game Object {progressableGameObject} does not have a component implementing IProgressable");
        }
        progressable.OnProgressChanged += Progressable_OnProgressChanged;
        barImage.fillAmount = 0f;
        Hide();
    }
    private void Show() {
        gameObject.SetActive(true);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }
    private void Progressable_OnProgressChanged(object sender, IProgressable.OnProgressChangedEventArgs e) {
        barImage.fillAmount = e.progressNormalized;
        if(e.progressNormalized == 0f || e.progressNormalized == 1f) {
            Hide();
        }
        else {
            Show();
        }
    }
}
