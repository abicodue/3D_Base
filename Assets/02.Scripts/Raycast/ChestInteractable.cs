using UnityEngine;

public class ChestInteractable : MonoBehaviour
{

    [SerializeField]
    private Renderer m_renderer;
    [SerializeField]
    private Material openedMaterial;

    private bool opened = false;

    public bool TryOpen()
    {
        if (opened)
        {
            return false;
        }

        opened = true;

        if (m_renderer != null && openedMaterial != null)
        {
            m_renderer.material = openedMaterial;
        }

        return true;
        
    }
}
