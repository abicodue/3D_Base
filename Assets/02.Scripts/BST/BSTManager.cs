using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BSTManager : MonoBehaviour
{
    public Node root;

    [Header("Node Visual Settings")]
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private Transform nodeParent;

    [Header("Position Settings")]
    [SerializeField] private float rootY = 3f;
    [SerializeField] private float verticalGap = 1.5f;
    [SerializeField] private float horizontalGap = 4f;

    [Header("Line Settings")]
    [SerializeField] private Material lineMaterial;
    [SerializeField] private Color lineColor = Color.white;
    [SerializeField] private float lineWidth = 0.05f;

    [Header("Search UI Settings")]
    [SerializeField] private TMP_InputField searchInput;
    [SerializeField] private TMP_Text comparisonText;

    [Header("Search Visual Settings")]
    [SerializeField] private float searchDelay = 0.5f;
    [SerializeField] private Color normalColor = Color.gray;
    [SerializeField] private Color visitedColor = Color.yellow;
    [SerializeField] private Color foundColor = Color.green;

    private Material runtimeLineMaterial;
    private Coroutine searchCoroutine;

    private List<Node> allNodes = new List<Node>();

    // Linear Search 비교용 List
    private List<int> linearDataList = new List<int>();

    private void Start()
    {
        int[] values = { 50, 30, 70, 20, 40, 60, 80 };

        // Linear Search용 List에도 같은 데이터 저장
        linearDataList = new List<int>(values);

        // BST에도 같은 데이터 삽입
        foreach (int value in values)
        {
            Insert(value);
        }

        Debug.Log("BST creation completed");
        PrintTree(root);

        UpdateComparisonText("Enter a number and press Search.");
    }

    public void Insert(int newValue)
    {
        if (root == null)
        {
            root = new Node(newValue);

            Vector3 rootPosition = new Vector3(0f, rootY, 0f);
            CreateNodeVisual(root, rootPosition);

            return;
        }

        Node current = root;
        int depth = 0;

        while (true)
        {
            float xOffset = horizontalGap / Mathf.Pow(2, depth);

            if (newValue < current.value)
            {
                if (current.left == null)
                {
                    Node newNode = new Node(newValue);

                    // 중요: new Node(newValue)를 또 만들지 말고, newNode를 연결해야 함
                    current.left = newNode;

                    Vector3 newPosition = current.position + new Vector3(-xOffset, -verticalGap, 0f);

                    CreateNodeVisual(newNode, newPosition);
                    CreateLine(current, newNode);

                    return;
                }

                current = current.left;
                depth++;
            }
            else if (newValue > current.value)
            {
                if (current.right == null)
                {
                    Node newNode = new Node(newValue);

                    // 중요: new Node(newValue)를 또 만들지 말고, newNode를 연결해야 함
                    current.right = newNode;

                    Vector3 newPosition = current.position + new Vector3(xOffset, -verticalGap, 0f);

                    CreateNodeVisual(newNode, newPosition);
                    CreateLine(current, newNode);

                    return;
                }

                current = current.right;
                depth++;
            }
            else
            {
                Debug.Log($"Don't insert the same value: {newValue}");
                return;
            }
        }
    }

    // 버튼에서 호출할 함수
    public void SearchFromInput()
    {
        if (searchInput == null)
        {
            Debug.LogWarning("Search Input is not connected.");
            return;
        }

        if (int.TryParse(searchInput.text, out int targetValue))
        {
            Search(targetValue);
        }
        else
        {
            UpdateComparisonText("Please enter a valid number.");
        }
    }

    // 과제 요구사항의 Search(int targetValue)
    public void Search(int targetValue)
    {
        if (root == null)
        {
            UpdateComparisonText("Tree is empty.");
            return;
        }

        if (searchCoroutine != null)
        {
            StopCoroutine(searchCoroutine);
        }

        ResetNodeColors();

        // 1. Linear Search 비교 횟수 계산
        bool linearFound = LinearSearch(targetValue, out int linearComparisons);

        // 2. BST Search는 시각적으로 천천히 진행
        searchCoroutine = StartCoroutine(BSTSearchCompareRoutine(targetValue, linearFound, linearComparisons));
    }

    private bool LinearSearch(int targetValue, out int comparisons)
    {
        comparisons = 0;

        for (int i = 0; i < linearDataList.Count; i++)
        {
            comparisons++;

            if (linearDataList[i] == targetValue)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator BSTSearchCompareRoutine(int targetValue, bool linearFound, int linearComparisons)
    {
        Node current = root;
        int bstComparisons = 0;

        while (current != null)
        {
            bstComparisons++;

            SetNodeColor(current, visitedColor);

            UpdateComparisonText(
                $"Target: {targetValue}\n" +
                $"Linear Search: {(linearFound ? "Found" : "Not Found")}, Comparisons: {linearComparisons}\n" +
                $"BST Search: Searching...\n" +
                $"Current Node: {current.value}\n" +
                $"BST Comparisons: {bstComparisons}"
            );

            yield return new WaitForSeconds(searchDelay);

            if (targetValue == current.value)
            {
                SetNodeColor(current, foundColor);

                ShowFinalComparison(
                    targetValue,
                    linearFound,
                    linearComparisons,
                    true,
                    bstComparisons
                );

                searchCoroutine = null;
                yield break;
            }
            else if (targetValue < current.value)
            {
                current = current.left;
            }
            else
            {
                current = current.right;
            }
        }

        ShowFinalComparison(
            targetValue,
            linearFound,
            linearComparisons,
            false,
            bstComparisons
        );

        searchCoroutine = null;
    }

    private void ShowFinalComparison(
        int targetValue,
        bool linearFound,
        int linearComparisons,
        bool bstFound,
        int bstComparisons
    )
    {
        string winnerText;

        if (linearComparisons > bstComparisons)
        {
            winnerText = $"BST used {linearComparisons - bstComparisons} fewer comparisons.";
        }
        else if (linearComparisons < bstComparisons)
        {
            winnerText = $"Linear Search used {bstComparisons - linearComparisons} fewer comparisons.";
        }
        else
        {
            winnerText = "Both used the same number of comparisons.";
        }

        UpdateComparisonText(
            $"Target: {targetValue}\n" +
            $"Linear Search: {(linearFound ? "Found" : "Not Found")}\n" +
            $"Linear Comparisons: {linearComparisons}\n\n" +
            $"BST Search: {(bstFound ? "Found" : "Not Found")}\n" +
            $"BST Comparisons: {bstComparisons}\n\n" +
            $"{winnerText}"
        );
    }

    private void PrintTree(Node node)
    {
        if (node == null)
        {
            return;
        }

        PrintTree(node.left);
        Debug.Log(node.value);
        PrintTree(node.right);
    }

    private void CreateNodeVisual(Node node, Vector3 position)
    {
        node.position = position;

        GameObject nodeObject = Instantiate(nodePrefab, position, Quaternion.identity);
        nodeObject.name = $"Node_{node.value}";

        if (nodeParent != null)
        {
            nodeObject.transform.SetParent(nodeParent);
        }

        TMP_Text text = nodeObject.GetComponentInChildren<TMP_Text>();

        if (text != null)
        {
            text.text = node.value.ToString();
        }
        else
        {
            Debug.LogWarning("Couldn't find TextMeshPro in the Node Prefab.");
        }

        node.visualObject = nodeObject;

        allNodes.Add(node);

        SetNodeColor(node, normalColor);
    }

    private void CreateLine(Node parent, Node child)
    {
        GameObject lineObject = new GameObject($"Line_{parent.value}_to_{child.value}");

        if (nodeParent != null)
        {
            lineObject.transform.SetParent(nodeParent);
        }

        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;

        lineRenderer.material = GetLineMaterial();

        lineRenderer.sortingOrder = 0;

        Vector3 start = parent.position;
        Vector3 end = child.position;

        start.z = 0.1f;
        end.z = 0.1f;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    private Material GetLineMaterial()
    {
        if (lineMaterial != null)
        {
            return lineMaterial;
        }

        if (runtimeLineMaterial == null)
        {
            runtimeLineMaterial = new Material(Shader.Find("Sprites/Default"));
        }

        return runtimeLineMaterial;
    }

    private void ResetNodeColors()
    {
        foreach (Node node in allNodes)
        {
            SetNodeColor(node, normalColor);
        }
    }

    private void SetNodeColor(Node node, Color color)
    {
        if (node == null || node.visualObject == null)
        {
            return;
        }

        SpriteRenderer spriteRenderer = node.visualObject.GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
            return;
        }

        Renderer[] renderers = node.visualObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            if (renderer.GetComponent<TMP_Text>() == null)
            {
                renderer.material.color = color;
                return;
            }
        }
    }

    private void UpdateComparisonText(string message)
    {
        if (comparisonText != null)
        {
            comparisonText.text = message;
        }

        Debug.Log(message);
    }
}