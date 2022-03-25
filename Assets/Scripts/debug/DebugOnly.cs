using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOnly : MonoBehaviour
{
    [SerializeField] string Notes;
    [SerializeField] bool enableResetPlayerPos;
    [SerializeField] Transform resetPlayerPosition;

    private void Update() {
        if (Input.GetKeyUp(KeyCode.R)) {
            gameObject.GetComponent<CharacterController>().enabled = false;
            gameObject.transform.position = resetPlayerPosition.position;
            gameObject.transform.rotation = resetPlayerPosition.rotation;
            gameObject.GetComponent<CharacterController>().enabled = true;
        }
    }
}
