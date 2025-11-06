using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.Arm;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField]
    private int radius = 2;
    [SerializeField]
    private float hexSize = 1f;
    [SerializeField]
    private CellView hexPrefab;

    private Grid Grid;
    private Dictionary<Cell, CellView> CellObjects = new();
    
    public void Initialize()
    {        
        Grid = new Grid(radius);        
        GenerateVisualGrid();
        CenterGrid();
    }   

    public Vector3 GetPositionByCell(Cell cell)
    {
        var view = CellObjects[cell];
        return view.gameObject.transform.position;
    }

    public void CleanUp()
    {
        Grid.CleanUp();
    }

    public bool GridIsFull() => Grid.IsFull();
    private void GenerateVisualGrid()
    {
        foreach (var cell in Grid.GetAllCells())
        {
            Vector3 position = AxialToWorld(cell.Q, cell.R);
            var hexagonePlate = Instantiate(hexPrefab, position, Quaternion.identity, transform);
            hexagonePlate.Initialize(cell);
            CellObjects[cell] = hexagonePlate;
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

        foreach (var cell in CellObjects.Values)
        {
            avg += cell.transform.position;
            count++;
        }

        avg /= count;

        foreach (var cell in CellObjects.Values)
        {
            cell.transform.position -= avg;
        }            
    }
}
