using UnityEngine;

public class ColourRestorer : MonoBehaviour
{
    [SerializeField] float colourRadius = 0.0f;
    private Transform tf;
    private int dataIndex;

    void Start()
    {
        tf = GetComponent<Transform>();
        dataIndex = ColourRestorationManager.Instance.packAndUpdateNodeData(ref tf, colourRadius);
    }

    void Update()
    {
        ColourRestorationManager.Instance.packAndUpdateNodeData(ref tf, colourRadius, dataIndex);
    }
}
