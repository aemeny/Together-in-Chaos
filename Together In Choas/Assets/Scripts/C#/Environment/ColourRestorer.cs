using UnityEngine;

public class ColourRestorer : MonoBehaviour
{
    [SerializeField] float colourRadius = 0.0f;
    private Transform tf;
    private int dataIndex;

    void Start()
    {
        tf = GetComponent<Transform>();
        dataIndex = ColourRestorationManager.Instance.PackAndUpdateNodeData(ref tf, colourRadius);
    }

    void Update()
    {
        ColourRestorationManager.Instance.PackAndUpdateNodeData(ref tf, colourRadius, dataIndex);
    }
}
