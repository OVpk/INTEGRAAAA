using System;
using UnityEngine;

public class StartScreen : MonoBehaviour
{

    [SerializeField] private Kahoot kahoot;

    private bool isLock = false;

    private void Update()
    {
        if (isLock) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isLock = true;
            kahoot.Init();
            this.gameObject.SetActive(false);
        }
    }
}
