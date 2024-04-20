using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInGame : MonoBehaviour
{
    [SerializeField] private bool isVisible;
    [SerializeField] Canvas m_canvas;
    private MeshRenderer m_Renderer;

    private void Start()
    {
        m_Renderer = GetComponent<MeshRenderer>();
    }
    private void Update()
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        m_Renderer.enabled = isVisible;
         if(m_canvas) m_canvas.enabled = isVisible;
    }
}

