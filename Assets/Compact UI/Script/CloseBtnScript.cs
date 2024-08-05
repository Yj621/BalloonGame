using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloseButton
{
    public class CloseBtnScript : MonoBehaviour
    {
        public GameObject objectToClose;
        public GameObject panelToClose;

        public void CloseObj()
        {
            objectToClose.SetActive(false);
            panelToClose.SetActive(false);
        }
    }
}
