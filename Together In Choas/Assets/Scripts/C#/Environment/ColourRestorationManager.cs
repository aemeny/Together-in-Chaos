using System.Linq;
using UnityEngine;

public class ColourRestorationManager : MonoBehaviour
{
    // Singleton handling
    private static ColourRestorationManager _instance;
    public static ColourRestorationManager Instance { get { return _instance; } }

    // Max amount of possible colour nodes
    // (PREALLOCATE MEMORY IN SHADER IN COURSE WITH THIS NUMBER)
    const int MAX_ARRAY_SIZE = 20;

    private Vector4[] colourNodeData = new Vector4[] {};
    private bool updated;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        updated = false;
        Shader.SetGlobalInt("PREALLOC_AMOUNT", MAX_ARRAY_SIZE);
    }

    // Packs transform and sphere radius in to a Vec4 to be sent to the shader. Returns new index if needed
    public int PackAndUpdateNodeData(ref Transform _transform, float _radius, int _index = -1)
    {
        Vector4 packedData = new Vector4(_transform.position.x, _transform.position.y, _transform.position.z, _radius);

        if (_index == -1)
            return InitValColourNodeData(ref packedData);
       
        SetValColourNodeData(ref packedData, _index);
        return -1; 
    }

    private void SetValColourNodeData(ref Vector4 _nodeData, int _index)
    {
        if (_index >= colourNodeData.Length)
        {
            print("COLOUR NODE INDEX NOT IN RANGE");
            return;
        }
        colourNodeData[_index] = _nodeData;
        updated = true;
    }

    private int InitValColourNodeData(ref Vector4 _nodeData)
    {
        colourNodeData = colourNodeData.Append<Vector4>(_nodeData).ToArray();
        updated = true;
        return colourNodeData.Length - 1;
    }

    private void LateUpdate()
    {
        if (updated)
        {
            Shader.SetGlobalInt("_NumColourNodes", colourNodeData.Length);
            Shader.SetGlobalVectorArray("_ColourNodeData", colourNodeData);
            updated = false;
        }
    }
}
