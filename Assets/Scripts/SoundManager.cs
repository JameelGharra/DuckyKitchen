using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour {

    [SerializeField] private AudioClipReferenceSO audioClipReferencesSO;

    public static SoundManager instance {  get; private set; }

    private void Awake() {
        instance = this;
    }

    private void Start() {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnAnyCounterPlaced += BaseCounter_OnAnyCounterPlaced;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e) {
        TrashCounter trashCounter = (TrashCounter)sender;
        PlaySound(audioClipReferencesSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyCounterPlaced(object sender, System.EventArgs e) {
        BaseCounter baseCounter = (BaseCounter)sender;
        PlaySound(audioClipReferencesSO.objectDrop, baseCounter.transform.position);
    }
    private void Player_OnPickedSomething(object sender, System.EventArgs e) {
        PlaySound(audioClipReferencesSO.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e) {
        CuttingCounter cuttingCounter = (CuttingCounter)sender;
        PlaySound(audioClipReferencesSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e) {
        PlaySound(audioClipReferencesSO.deliveryFail, DeliveryCounter.Instance.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e) {
        PlaySound(audioClipReferencesSO.deliverySuccess, DeliveryCounter.Instance.transform.position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }
    public void PlayFootstepsSound(Vector3 position, float volume) {
        PlaySound(audioClipReferencesSO.footsteps, position, volume);
    }
}
