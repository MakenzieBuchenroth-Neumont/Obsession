using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable {
    public Student studentData;

    private GameObject dialogueBox;

    public CapsuleCollider mainCollider;
    public GameObject rig;
    public Animator anim;

    Collider[] ragdollColliders;
    Rigidbody[] ragdollRigidbodies;

    private void Start() {
        getRagdollBits();
        if (!studentData.IsDead) {
            ragdollOff();
        }
        dialogueBox = FindDialogueBoxInDontDestroyOnLoad();

        if (dialogueBox != null) {
            dialogueBox.SetActive(false);
        }
    }

    private GameObject FindDialogueBoxInDontDestroyOnLoad() {
        return GameObject.FindGameObjectWithTag("DialogueBox");
    }

    public void interact(PlayerInteract player) {
        Vector3 directionToNPC = (player.transform.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, directionToNPC);

        if (dotProduct > 0.5f) {
            UIManager.Instance.showInteractionPrompt("Talk");
            talkToNPC();
            anim.SetBool("Walking", false);
        }
        else if (dotProduct < -0.5f && player.hasWeapon) {
            UIManager.Instance.showInteractionPrompt("Stab");
            stab();
			anim.SetBool("Walking", false);
		}
        else if (dotProduct < -0.5f && !player.hasWeapon) {
            talkToNPC();
			anim.SetBool("Walking", false);
		}
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Door")) {
            DoorPrompt prompt = other.GetComponent<DoorPrompt>();
            if (prompt != null && !prompt.isDoorOpened()) {
                prompt.toggleDoor();
            }
        }
    }

    private void talkToNPC() {
        UIManager.Instance.hideInteractionPrompt();
        GameObject dialogueBox = DialogueManager.Instance.dialogueBox;
        if (dialogueBox != null) {
            dialogueBox.SetActive(true);
            if (dialogueBox.TryGetComponent(out Dialogue dialogue)) {
                dialogue.studentData = studentData;
                if (studentData.DialogueLines.Length > 0) {
					dialogue.parent.SetActive(true);
					dialogue.startDialogue(studentData.DialogueLines);
                }
                else {
                    Debug.LogWarning("No dialogue lines assigned.");
                }
            }
        }
        else {
            Debug.LogError("DialogueBox is null in talkToNPC().");
        }
    }

    private void stab() {
        UIManager.Instance.hideInteractionPrompt();
        startStabQTE();
    }

    private void startStabQTE() {
        KeyCode stabKey = KeyCode.U;
        QTEManager qTEManager = FindObjectOfType<QTEManager>();
        if (qTEManager != null) {
            qTEManager.StartQTE(stabKey, 1f, this);
            Debug.Log("QTE started for stabbing.");
        }
    }

    public void handleQTESuccess() {
        Debug.Log("NPC has been stabbed.");
        studentData.IsDead = true;
        ragdollOn();
    }

    public void handleQTEFailure() {
        Debug.Log("You got caught.");
    }

    public void ragdollOn() {
        foreach (Collider collider in ragdollColliders) {
            collider.enabled = true;
        }
        foreach (Rigidbody rb in ragdollRigidbodies) {
            rb.isKinematic = false;
        }
        anim.enabled = false;
        mainCollider.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void ragdollOff() {
        foreach (Collider collider in ragdollColliders) {
            collider.enabled = false;
        }
        foreach (Rigidbody rb in ragdollRigidbodies) {
            rb.isKinematic = true;
        }
        anim.enabled = true;
        mainCollider.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void getRagdollBits() {
        ragdollColliders = rig.GetComponentsInChildren<Collider>();
        ragdollRigidbodies = rig.GetComponentsInChildren<Rigidbody>();
    }
}
