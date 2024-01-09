using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.States.BattleExecutorNumbers.Common
{
    public class BEN_CodeType_Selector : MonoBehaviour
    {
        [SerializeField]
        private BEN_CodeType_Button[] buttons;

        public BEN_CodeType_Button SelectedButton { get; private set; }

        public void OnInit()
        {
            foreach (var button in buttons)
            {
                button.OnInit(button =>
                {
                    var image = SelectedButton?.GetComponent<Image>();
                    if (image != null)
                    {
                        image.color = Color.black;
                    }
                    SelectedButton = button;
                    SelectedButton.GetComponent<Image>().color = Color.blue;
                });
            }
        }

        public void OnClose()
        {
            foreach (var button in buttons)
            {
                button.OnClose();
            }
        }
    }
}
