using Assets.Scripts.CommonComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.States.BattleExecutorNumbers.Common
{
    public class BENCodeElement : MonoBehaviour
    {
        public BENCodeElement? ListPrevNode { get; set; }
        public BENCodeElement? ListNextNode { get; set; }

        [SerializeField]
        private BEN_CE_MovingButton _moveUpButton;
        public BEN_CE_MovingButton MoveUpButton => _moveUpButton;

        [SerializeField]
        private BEN_CE_MovingButton _moveDownButton;
        public BEN_CE_MovingButton MoveDownButton => _moveDownButton;

        [SerializeField]
        private BEN_CE_MovingButton _removeButton;
        public BEN_CE_MovingButton RemoveButton => _removeButton;

        [SerializeField]
        private BEN_CE_MovingButton _addButton;
        public BEN_CE_MovingButton AddButton => _addButton;

        [SerializeField]
        private TMP_Text _text;

        public BEN_CodeElementType CodeElementType { get; private set; }

        public void SetButtonsActivity()
        {
            _moveUpButton.SetButtonActive(ListPrevNode != null);
            _moveDownButton.SetButtonActive(ListNextNode != null);

            _removeButton.SetButtonActive(ListPrevNode != null);
            _addButton.SetButtonActive(true);
        }

        public void InitType(BEN_CodeElementType elementType)
        {
            CodeElementType = elementType;
            _text.text = elementType.GetName();
        }

        public void InitButtons(GameObject onCreateParent, ObjectPoolComponent objectsPool, Action updateCodeTreeRoot, BEN_CodeType_Selector typeSelector)
        {
            SetButtonsActivity();
            _moveUpButton.OnClick.AddListener(() =>
            {
                if (ListPrevNode == null) return;

                var curPrev = ListPrevNode;
                var curNext = ListNextNode;

                var prevPrev = curPrev.ListPrevNode;

                ListPrevNode = prevPrev;
                ListNextNode = curPrev;

                curPrev.ListPrevNode = this;
                curPrev.ListNextNode = curNext;

                transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
                SetButtonsActivity();

                curPrev?.SetButtonsActivity();
                curNext?.SetButtonsActivity();
                prevPrev?.SetButtonsActivity();

                updateCodeTreeRoot.Invoke();
            });

            _moveDownButton.OnClick.AddListener(() =>
            {
                if (ListPrevNode == null) return;

                var curPrev = ListPrevNode;
                var curNext = ListNextNode;

                var nextNext = curNext.ListNextNode;

                ListPrevNode = curNext;
                ListNextNode = nextNext;

                curNext.ListPrevNode = curPrev;
                curNext.ListNextNode = this;

                transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
                SetButtonsActivity();

                curPrev?.SetButtonsActivity();
                curNext?.SetButtonsActivity();
                nextNext?.SetButtonsActivity();

                updateCodeTreeRoot.Invoke();
            });

            _addButton.OnClick.AddListener(() =>
            {
                var obj = objectsPool.GetObject();
                var element = obj.GetComponent<BENCodeElement>();

                element.ListPrevNode = this;
                element.ListNextNode = ListNextNode;

                ListNextNode = element;

                element.transform.SetParent(onCreateParent.transform);
                element.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
                element.InitType(typeSelector.SelectedButton.ElementType);
                element.InitButtons(onCreateParent, objectsPool, updateCodeTreeRoot, typeSelector);
                element.gameObject.SetActive(true);

                SetButtonsActivity();
                element.ListNextNode?.SetButtonsActivity();

                updateCodeTreeRoot.Invoke();
            });

            _removeButton.OnClick.AddListener(() =>
            {
                updateCodeTreeRoot.Invoke();

                if (ListPrevNode != null)
                {
                    ListPrevNode.ListNextNode = ListNextNode;
                    ListPrevNode.SetButtonsActivity();
                }
                if (ListNextNode != null)
                {
                    ListNextNode.ListPrevNode = ListPrevNode;
                    ListNextNode.SetButtonsActivity();
                }

                OnElementDestory();
                transform.SetParent(objectsPool.transform);
                objectsPool.FreeObject(gameObject);
            });
        }

        public void OnElementDestory()
        {
            _moveUpButton.OnClick.RemoveAllListeners();
            _moveDownButton.OnClick.RemoveAllListeners();
            _addButton.OnClick.RemoveAllListeners();
            _removeButton.OnClick.RemoveAllListeners();

            ListPrevNode = ListNextNode = null;
        }
    }
}
