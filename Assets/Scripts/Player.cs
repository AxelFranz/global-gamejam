using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    [SerializeField] private Rigidbody m_rb;
    private InputAction m_moveAction;
    [SerializeField] private const float m_speed = 10;

    private void Start()
    {
        m_moveAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        Vector2 moveValue = m_moveAction.ReadValue<Vector2>();
        moveValue *= m_speed;
        Vector3 velocity = new Vector3(moveValue.x, 0,moveValue.y);
        m_rb.linearVelocity = velocity;
        Debug.Log(moveValue);
    }

}