using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drawline : MonoBehaviour
{
    [Header("Board Settings")]
    public RectTransform boardArea;     // Assign UI Image RectTransform
    public Transform lineParent;        // Empty GameObject to hold all strokes
    public Material lineMaterial;       // Brush material (Sprites/Default or Unlit/Color)
    public float paintSize = 0.05f;     // Stroke thickness
    public float replayDelay = 0.5f;    // Delay between strokes during replay

    private LineRenderer currentLine;
    private List<Vector3> points = new List<Vector3>();
    private List<List<Vector3>> allStrokes = new List<List<Vector3>>(); // store all strokes
    private Vector3 lastMousePosition;
    private int strokeCount = 0;

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
                strokeCount++;
                Debug.Log("Started stroke #" + strokeCount + " at " + lastMousePosition);
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
            if (points.Count > 0)
            {
                // Save this stroke permanently
                allStrokes.Add(new List<Vector3>(points));
            }
            currentLine = null;
            points.Clear();
        }
    }

    /// <summary>
    /// Create a new stroke (LineRenderer)
    /// </summary>
    private void CreateNewLine()
    {
        GameObject newLine = new GameObject("Stroke_" + strokeCount);
        newLine.transform.SetParent(lineParent);

        currentLine = newLine.AddComponent<LineRenderer>();
        currentLine.material = lineMaterial;
        currentLine.startWidth = paintSize;
        currentLine.endWidth = paintSize;
        currentLine.positionCount = 0;

        // Ensure it renders above the board
        currentLine.sortingLayerName = "UI";
        currentLine.sortingOrder = 1;
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
    /// Clear only visuals, keep stroke data
    /// </summary>
    public void ClearBoardVisualsOnly()
    {
        for (int i = lineParent.childCount - 1; i >= 0; i--)
        {
            Destroy(lineParent.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Clear everything (visuals + stored strokes)
    /// </summary>
    public void ClearBoard()
    {
        ClearBoardVisualsOnly();
        allStrokes.Clear();
        strokeCount = 0;
    }

    /// <summary>
    /// Coroutine to replay all strokes sequentially
    /// </summary>
    public IEnumerator ReplayAllStrokes(float delay)
    {
        ClearBoardVisualsOnly();

        for (int i = 0; i < allStrokes.Count; i++)
        {
            // Create stroke GameObject
            GameObject newLine = new GameObject("Stroke_" + (i + 1));
            newLine.transform.SetParent(lineParent);

            LineRenderer lr = newLine.AddComponent<LineRenderer>();
            lr.material = lineMaterial;
            lr.startWidth = paintSize;
            lr.endWidth = paintSize;
            lr.sortingLayerName = "UI";
            lr.sortingOrder = 1;

            lr.positionCount = allStrokes[i].Count;
            lr.SetPositions(allStrokes[i].ToArray());

            Debug.Log("Replayed stroke #" + (i + 1));

            yield return new WaitForSeconds(delay); // wait before next stroke
        }
    }

    /// <summary>
    /// Start replay coroutine from a button
    /// </summary>
    public void StartReplay()
    {
        StartCoroutine(ReplayAllStrokes(replayDelay));
    }
}