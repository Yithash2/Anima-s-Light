using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableManager : MonoBehaviour
    {
        public static InteractableManager Instance;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
        }
        

        private void Start()
        {
            InputsManager.Instance.Subscribe(InputsManager.Instance.Dive,Interact);
            
            _sortedPool = new SortedPool(1);
            _currentInteractable = null;
            _latePosition = transform.position;
            
        }

        private SortedPool _sortedPool = new SortedPool();
        private Interactable _currentInteractable;

        public void Subscribe(Interactable interactable)
        {
            _sortedPool.AddValue(interactable);
            interactable.subscribed = true;
            
            if (!interactHint.activeInHierarchy) interactHint.SetActive(true);
        }

        public void Unsubscribe(Interactable interactable)
        {
            _sortedPool.RemoveValue(interactable);
            interactable.subscribed = false;
            
            if (_sortedPool.IsEmpty()) interactHint.SetActive(false);

        }

        public Interactable DeQueue()
        {
            var result = _sortedPool.Dequeue(this);
            result.subscribed = false;
            if (_sortedPool.IsEmpty()) interactHint.SetActive(false);
            return result;
        }
        
        private void Interact(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                Debug.Log($"Interact : {_sortedPool.IsEmpty()} and {_currentInteractable != null}");
                StartInteraction(null);
            }
        }

        public void StartInteraction(Interactable interactable)
        {
            if(_sortedPool.IsEmpty()) return;
            if (_currentInteractable != null) return;
            
            _currentInteractable = interactable != null ? interactable : DeQueue();
            _currentInteractable.OnInteractionStart();
            interactHint.SetActive(false);
        }

        public void StopInteraction()
        {
            if (_currentInteractable is null) throw new InvalidOperationException("There is no interaction to stop");
            _currentInteractable.OnInteractionEnd();
            _currentInteractable = null;
            if (!_sortedPool.IsEmpty()) interactHint.SetActive(true);
            
        }

        [SerializeField] private GameObject interactHint;

        private void Update()
        {
            if(transform.position != _latePosition)
            MoveCursor();
        }
        
        private Interactable _currentSelected;
        private Vector3 _selectedPosition;
        private bool _cursorMoving = false;
        // ReSharper disable Unity.PerformanceAnalysis
        private void MoveCursor()
        {
            if (_sortedPool.IsEmpty())
            {
                interactHint.transform.position = transform.position + Vector3.up * 2f;
                interactHint.transform.parent = gameObject.transform;
                return;
            }
            
            interactHint.transform.parent = null;
            
            var curr = _sortedPool.Get(this);
            
            _currentSelected ??= curr;

            if (!_currentSelected.CanBeSelected)
            {
                _selectedPosition = transform.position + Vector3.up * 2f;
                if(!_cursorMoving) StartCoroutine(LerpCursorPosition());
            }
            
            if(curr == _currentSelected) return;
            
            _currentSelected = curr;
            var displacement = Vector3.up * ((_currentSelected.GetComponent<SpriteRenderer>().size.y / 2) + 2);
            _selectedPosition = curr.CanBeSelected ? curr.transform.position + displacement : (transform.position + Vector3.up * 2f);
            if (!_cursorMoving) StartCoroutine(LerpCursorPosition());
        }

        private IEnumerator LerpCursorPosition()
        {
            
            _cursorMoving = true;
            while (interactHint.transform.position != _selectedPosition)
            {
                interactHint.transform.position = Vector3.Lerp(interactHint.transform.position, _selectedPosition, 0.3f);
                yield return null;
            }
            _cursorMoving = false;
        }

        private Vector3 _latePosition;

        private void LateUpdate()
        {
            _latePosition = transform.position;
        }
    }
