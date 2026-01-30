using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private Rigidbody m_rb;
    [SerializeField] private float m_speed = 10;
    private InputAction m_moveAction;
    private InputAction m_switchMask;
    private List<MaskState> m_equippedMasks; // [0] = Equipped mask, [1] = mask on back

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        GetComponent<MeshRenderer>().material.color = Color.red;
        m_equippedMasks = new List<MaskState>() { MaskState.Fire, MaskState.Water };
        GameManager.Instance.MaskChanged += onMaskChange;
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_switchMask = InputSystem.actions.FindAction("SwitchMask");
        m_switchMask.started += switchMask;
    }
    private void Update()
    {
        Vector2 moveValue = m_moveAction.ReadValue<Vector2>();
        moveValue *= m_speed;
        Vector3 velocity = new Vector3(moveValue.x, 0,moveValue.y);
        m_rb.linearVelocity = velocity;
    }

    private void switchMask(InputAction.CallbackContext context)
    {
        m_equippedMasks.Reverse();
        GameManager.Instance.ChangeMask(m_equippedMasks[0]);
    }

    private void onMaskChange(MaskState state)
    {
        m_equippedMasks[0] = state;
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Color newColor;
        switch(state)
        {
            case MaskState.Fire:
                newColor = Color.red; 
                break;
            case MaskState.Water:
                newColor = Color.blue;
                break;
            case MaskState.Plant:
                newColor = Color.green;
                break;
            case MaskState.Unmasked:
                newColor = Color.grey;
                break;
            default:
                newColor = Color.grey;
                break;
        }
        meshRenderer.material.color = newColor;
        Debug.Log(state);
    }

}