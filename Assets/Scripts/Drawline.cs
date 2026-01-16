using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drawline : MonoBehaviour
{
    [Header("Board Settings")]
    public RectTransform boardArea;         // Assign UI Image RectTransform 
    public Transform lineParent;            // Empty GameObject to hold all strokes
    public Material lineMaterial;           // Brush material (Sprites/Default or Unlit/Color)
    public float paintSize = 0.05f;         // Stroke thickness

    private LineRenderer currentLine;
    private List<Vector3> points = new List<Vector3>();
    private Vector3 lastMousePosition;

    void Update()
    {
        // Start drawing
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverBoard())
            {
                CreateNewLine();
                AddPoint(GetWorldMousePosition());
                lastMousePosition = GetWorldMousePosition();
            }
        }

        // Continue drawing
        if (Input.GetMouseButton(0) && currentLine != null)
        {
            if (IsPointerOverBoard())
            {
                Vector3 mousePos = GetWorldMousePosition();
                if (Vector3.Distance(mousePos, lastMousePosition) > 0.01f)
                {
                    AddPoint(mousePos);
                    lastMousePosition = mousePos;
                }
            }
        }

        // Stop drawing
        if (Input.GetMouseButtonUp(0))
        {
            currentLine = null;
            points.Clear();
        }
    }

    /// <summary>
    /// Create a new stroke (LineRenderer)
    /// </summary>
    private void CreateNewLine()
    {
        GameObject newLine = new GameObject("Stroke");
        newLine.transform.SetParent(lineParent);

        currentLine = newLine.AddComponent<LineRenderer>();
        currentLine.material = lineMaterial;
        currentLine.startWidth = paintSize;
        currentLine.endWidth = paintSize;
        currentLine.positionCount = 0;

        // Ensure it renders above the board
        currentLine.sortingLayerName = "UI";
        currentLine.sortingOrder = 5;
    }

    /// <summary>
    /// Add a point to the current stroke
    /// </summary>
    private void AddPoint(Vector3 pos)
    {
        points.Add(pos);
        currentLine.positionCount = points.Count;
        currentLine.SetPositions(points.ToArray());
    }

    /// <summary>
    /// Convert mouse position to world space (2D)
    /// </summary>
    private Vector3 GetWorldMousePosition()
    {
        Vector3 screenPos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0; // keep on 2D plane
        return worldPos;
    }

    /// <summary>
    /// Check if pointer is inside the board area
    /// </summary>
    private bool IsPointerOverBoard()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            boardArea,
            Input.mousePosition,
            Camera.main,
            out localPoint
        );
        return boardArea.rect.Contains(localPoint);
    }

    /// <summary>
    /// Clear all strokes
    /// </summary>
    public void ClearBoard()
    {
        for (int i = lineParent.childCount - 1; i >= 0; i--)
        {
            Destroy(lineParent.GetChild(i).gameObject);
        }
    }

}