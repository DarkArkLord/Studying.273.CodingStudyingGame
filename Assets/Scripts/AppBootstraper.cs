using Assets.Scripts.Controllers;
using Assets.Scripts.StatesMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppBootstraper : MonoBehaviour
{
    private MainStatesController mainStatesController;

    private void Awake()
    {
        mainStatesController = new MainStatesController();
    }

    // Update is called once per frame
    void Update()
    {
        mainStatesController.Update();
    }
}
