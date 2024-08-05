using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToggleSwitchScript
{
    public class SwitchScript : MonoBehaviour
    {
        public GameObject ToggleOn;
        public GameObject ToggleOff;

        public bool toggleOn;
        public void Start()
        {
            ToggleOn.SetActive(false);
            toggleOn = false;
        }
        public void ToggleSwitch()
        {
            if (toggleOn == false)
            {
                ToggleOff.SetActive(false);
                ToggleOn.SetActive(true);
                toggleOn = true;
            }
            else
            {
                ToggleOff.SetActive(true);
                ToggleOn.SetActive(false);
                toggleOn = false;
            }
        }
    }
}
