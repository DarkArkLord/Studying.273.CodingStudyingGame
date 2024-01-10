using Assets.Scripts.CommonComponents;
using System;
using UnityEngine;

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
        private BEN_CE_Int_One _int_1;
        [SerializeField]
        private BEN_CE_Int_Two _int_2;
        [SerializeField]
        private BEN_CE_Int_Three _int_3;

        public BEN_CodeElementType CodeElementType { get; private set; }

        private BEN_ExecutionContext executionContext;

        public void SetButtonsActivity()
        {
            _moveUpButton.SetButtonActive(ListPrevNode != null);
            _moveDownButton.SetButtonActive(ListNextNode != null);

            _removeButton.SetButtonActive(ListPrevNode != null);
            _addButton.SetButtonActive(true);
        }

        public void InitType(BEN_CodeElementType elementType, BEN_ExecutionContext context)
        {
            executionContext = context;
            CodeElementType = elementType;

            switch (elementType)
            {
                case BEN_CodeElementType.IO_ReadInput:
                case BEN_CodeElementType.IO_WriteOutput:
                    _int_1.OnInit(CodeElementType, context);
                    _int_2.OnClose();
                    _int_3.OnClose();
                    break;
                case BEN_CodeElementType.IO_SetValue:
                    _int_1.OnClose();
                    _int_2.OnInit(CodeElementType, context);
                    _int_3.OnClose();
                    break;
                case BEN_CodeElementType.Numeric_Add:
                case BEN_CodeElementType.Numeric_Sub:
                case BEN_CodeElementType.Numeric_Mult:
                case BEN_CodeElementType.Numeric_Div:
                case BEN_CodeElementType.Numeric_Mod:
                    _int_1.OnClose();
                    _int_2.OnClose();
                    _int_3.OnInit(CodeElementType, context);
                    break;
                default:
                    throw new ArgumentException(nameof(elementType));
            }
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
                element.InitType(typeSelector.SelectedButton.ElementType, executionContext);
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

            SetElementsActive(true);
        }

        public void OnElementDestory()
        {
            _moveUpButton.OnClick.RemoveAllListeners();
            _moveDownButton.OnClick.RemoveAllListeners();
            _addButton.OnClick.RemoveAllListeners();
            _removeButton.OnClick.RemoveAllListeners();

            ListPrevNode = ListNextNode = null;
        }

        public void SetElementsActive(bool active)
        {
            _moveUpButton.SetButtonActive(active);
            _moveDownButton.SetButtonActive(active);
            _removeButton.SetButtonActive(active);
            _addButton.SetButtonActive(active);

            _int_1.SetElementsActive(active);
            _int_2.SetElementsActive(active);
            _int_3.SetElementsActive(active);
        }
    }
}
