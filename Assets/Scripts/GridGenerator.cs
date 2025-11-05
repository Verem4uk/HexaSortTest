using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField]
    private int radius = 2;
    [SerializeField]
    private float hexSize = 1f;
    [SerializeField]
    private CellView hexPrefab;

    private Grid _grid;
    private readonly Dictionary<Cell, CellView> _cellObjects = new();

    void Start()
    {
        if (hexPrefab == null)
        {
            Debug.LogError("Hex prefab is not assigned!");
            return;
        }

        _grid = new Grid(radius);
        GenerateVisualGrid();
        CenterGrid();
    }

    private void GenerateVisualGrid()
    {
        foreach (var cell in _grid.GetAllCells())
        {
            Vector3 position = AxialToWorld(cell.Q, cell.R);
            var hexagonePlate = Instantiate(hexPrefab, position, Quaternion.identity, transform);
            hexagonePlate.Initialize(cell);
            _cellObjects[cell] = hexagonePlate;
        }
    }

    private Vector3 AxialToWorld(int q, int r)
    {
        float x = hexSize * 1.5f * q;
        float z = hexSize * Mathf.Sqrt(3f) * (r + q / 2f);
        return new Vector3(x, 0f, z);
    }

    private void CenterGrid()
    {        
        Vector3 avg = Vector3.zero;
        int count = 0;

        foreach (var cell in _cellObjects.Values)
        {
            avg += cell.transform.position;
            count++;
        }

        avg /= count;

        foreach (var cell in _cellObjects.Values)
        {
            cell.transform.position -= avg;
        }            
    }
}
