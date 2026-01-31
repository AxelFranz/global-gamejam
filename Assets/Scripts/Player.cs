using System;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject m_model;
    [SerializeField] private float m_speed = 10;
    [SerializeField] private GameObject m_maskEquipped;
    [SerializeField] private GameObject m_maskBack;


    public static Player Instance { get; private set; }
    private Rigidbody m_rb;
    private InputAction m_moveAction;
    private InputAction m_switchMask;
    private List<MaskState> m_equippedMasks; // [0] = Equipped mask, [1] = mask on back

    private void Awake()
    {
        // Keep the GameManager when loading new scenes
        DontDestroyOnLoad(gameObject);

        // Singleton checks
        if (Instance == null) { // If there is no instance of GameManager yet, then this one becomes the only instance
            Instance = this;
        } else {                // If a GameManager instance already exists, destroy the new one
            Debug.LogWarning("Player Instance already exists, destroying the duplicate");
            Destroy(gameObject);
            return;
        }
        Events.PlayerDetected += onPlayerDetected;
    }

    private void onPlayerDetected()
    {
        Debug.Log("Aïe");
    }

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_equippedMasks = new List<MaskState>(){ MaskState.Fire, MaskState.Water };
        GameManager.Instance.MaskChanged += onMaskChange;
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_switchMask = InputSystem.actions.FindAction("SwitchMask");
        m_switchMask.started += switchMask;
        renderMasks();
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

    public void addMask(MaskState mask)
    {
        // 1 because we change the mask on the back
        m_equippedMasks[1] = mask;
        renderMasks();
    }

    private void renderMasks()
    {
        m_maskEquipped.GetComponent<MeshRenderer>().material.color = Utils.MaskStateToColor(m_equippedMasks[0]);
        m_maskBack.GetComponent<MeshRenderer>().material.color = Utils.MaskStateToColor(m_equippedMasks[1]);
    }


    private void onMaskChange(MaskState state)
    {
        m_equippedMasks[0] = state;
        renderMasks();

        Debug.Log(state);
    }

}