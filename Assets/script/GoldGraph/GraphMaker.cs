using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphMaker : MonoBehaviour
{
    public int SAMPLE_RATE = 100;

    Dictionary<int, int[]> AllTeamTotalGold = new Dictionary<int, int[]> { };

    [Header("GoldGraph")]
    public Transform DotGroup;
    public Transform LineGroup;
    public Transform TextGroup;
    public Transform InnerFilledGroup;
    public GameObject Dot;
    public GameObject Line;
    public GameObject Text;
    public GameObject MaskPanel;
    public Color DotGreen;
    public Color DotRed;
    public Material BlueMat;
    public Material PurpleMat;
    public GameObject Alpha;
    public GameObject Beta;

    public RectTransform GraphArea;
    public int EEG1;
    public int EEG2;

    private float width;
    private float height;

    void Start()
    {
        StartCoroutine(GetAlphaBeta());

        width = GraphArea.rect.width;
        height = GraphArea.rect.height;

        StartCoroutine(DrawGoldGraph());
    }

    IEnumerator GetAlphaBeta()
    {
        for (int time = 0; time < 100; time++)
        {
            yield return new WaitForSeconds(0.1f);
            EEG1 = (int)Alpha.GetComponent<InletOutlet2>().EEGpow;
            EEG2 = (int)Beta.GetComponent<InletOutlet2>().EEGpow;
            AllTeamTotalGold.Add(time, new[] { EEG1, EEG2 });
        }
        yield return null;
    }

    IEnumerator DrawGoldGraph()
    {
        yield return new WaitForSeconds(10.5f);
        float startPositionX = -width / 2;
        float maxYPosition = height / 2;
        var comparisonValue = new Dictionary<int, float>();
        var innterFilled = new List<Vector3>();

        foreach (var pair in AllTeamTotalGold)
            comparisonValue.Add(pair.Key, pair.Value[0] - pair.Value[1]);

        float MaxValue = comparisonValue.Max(x => Mathf.Abs(x.Value));
        Vector2 prevDotPos = Vector2.zero;

        for (int i = 0; i < SAMPLE_RATE; i++)
        {
            // Dot
            GameObject dot = Instantiate(Dot, DotGroup, true);
            dot.transform.localScale = Vector3.one;

            RectTransform dotRT = dot.GetComponent<RectTransform>();
            Image dotImage = dot.GetComponent<Image>();

            int tick = SAMPLE_RATE - 1 == i ? AllTeamTotalGold.Count - 1 : AllTeamTotalGold.Count / (SAMPLE_RATE - 1) * i;

            float yPos = comparisonValue[tick] / MaxValue;
            
            dotImage.color = yPos >= 0f ? DotGreen : DotRed;

            dotImage.color = new Color(dotImage.color.r, dotImage.color.g, dotImage.color.b, 1f);

            if (tick % 10 == 0)
            {
                int max;
                if (AllTeamTotalGold[tick][0] > AllTeamTotalGold[tick][1])
                {
                    max = AllTeamTotalGold[tick][0];
                }
                else
                {
                    max = AllTeamTotalGold[tick][1];
                }

                GameObject tmp = Instantiate(Text, TextGroup, true);
                tmp.GetComponent<TMP_Text>().text = max.ToString();
                tmp.GetComponent<TMP_Text>().color = new Color(dotImage.color.r, dotImage.color.g, dotImage.color.b, 1f);
                tmp.GetComponent<RectTransform>().anchoredPosition = yPos >= 0f ? new Vector2(startPositionX + (width / (SAMPLE_RATE - 1) * i), maxYPosition * yPos + 50) : new Vector2(startPositionX + (width / (SAMPLE_RATE - 1) * i), maxYPosition * yPos - 50);
            }
                        
            dotRT.anchoredPosition = new Vector2(startPositionX + (width / (SAMPLE_RATE - 1) * i), maxYPosition * yPos);

            innterFilled.Add(dotRT.anchoredPosition);

            // Line
            if (i == 0)
            {
                prevDotPos = dotRT.anchoredPosition;
                continue;
            }

            GameObject line = Instantiate(Line, LineGroup, true);
            line.transform.localScale = Vector3.one;

            RectTransform lineRT = line.GetComponent<RectTransform>();
            Image lineImage = line.GetComponent<Image>();

            lineImage.color = yPos >= 0f ? DotGreen : DotRed;
            lineImage.color = new Color(lineImage.color.r, lineImage.color.g, lineImage.color.b, 1f);

            float lineWidth = Vector2.Distance(prevDotPos, dotRT.anchoredPosition);
            float xPos = (prevDotPos.x + dotRT.anchoredPosition.x) / 2;
            yPos = (prevDotPos.y + dotRT.anchoredPosition.y) / 2;

            Vector2 dir = (dotRT.anchoredPosition - prevDotPos).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            lineRT.anchoredPosition = new Vector2(xPos, yPos);
            lineRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lineWidth);
            lineRT.localRotation = Quaternion.Euler(0f, 0f, angle);

            GameObject maskPanel = Instantiate(MaskPanel, Vector3.zero, Quaternion.identity);
            maskPanel.transform.SetParent(LineGroup);
            maskPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            maskPanel.transform.SetParent(line.transform);

            prevDotPos = dotRT.anchoredPosition;
        }

        CreateFilledGraphShape(innterFilled.ToArray());
    }

    private void CreateFilledGraphShape(Vector3[] linePoints)
    {
        List<Vector3> filledGraphPointList = new List<Vector3>();
        Vector3 tmp;
        float x;

        for (int i = 0; i < linePoints.Length; ++i)
        {
            filledGraphPointList.Add(new Vector3(linePoints[i].x, 0, 0));
            filledGraphPointList.Add(new Vector3(linePoints[i].x, linePoints[i].y, 0));

            if (i + 1 < linePoints.Length)
            {
                if (linePoints[i].y < 0 && linePoints[i + 1].y >= 0)
                {
                    x = Mathf.Lerp(linePoints[i].x, linePoints[i + 1].x, Mathf.Abs(linePoints[i].y) / (Mathf.Abs(linePoints[i].y) + Mathf.Abs(linePoints[i + 1].y)));
                    tmp = new Vector3(x, 0f, 0f);

                    filledGraphPointList.Add(tmp);

                    MakeRenderer(filledGraphPointList.ToArray(), filledGraphPointList.Count, PurpleMat);

                    filledGraphPointList.Clear();
                    filledGraphPointList.Add(tmp);
                }
                else if (linePoints[i].y > 0 && linePoints[i + 1].y <= 0)
                {
                    x = Mathf.Lerp(linePoints[i].x, linePoints[i + 1].x, Mathf.Abs(linePoints[i].y) / (Mathf.Abs(linePoints[i].y) + Mathf.Abs(linePoints[i + 1].y)));
                    tmp = new Vector3(x, 0f, 0f);

                    filledGraphPointList.Add(tmp);

                    MakeRenderer(filledGraphPointList.ToArray(), filledGraphPointList.Count, BlueMat);

                    filledGraphPointList.Clear();
                    filledGraphPointList.Add(tmp);
                }
            }
        }

        if (filledGraphPointList[filledGraphPointList.Count - 1].y >= 0)
        {
            MakeRenderer(filledGraphPointList.ToArray(), filledGraphPointList.Count, BlueMat);
        }
        else
        {
            MakeRenderer(filledGraphPointList.ToArray(), filledGraphPointList.Count, PurpleMat);
        }
    }

    private void MakeRenderer(Vector3[] graphPoints, int count, Material innerFill)
    {
        int trinangleCount = count - 2;
        int[] triangles = new int[trinangleCount * 3];

        int idx = 0;
        int ex = trinangleCount / 2;
        for (int i = 0; i < ex; i++)
        {
            triangles[idx++] = 2 * i;
            triangles[idx++] = 2 * i + 1;
            triangles[idx++] = 2 * i + 2;

            triangles[idx++] = 2 * i + 1;
            triangles[idx++] = 2 * i + 2;
            triangles[idx++] = 2 * i + 3;
        }

        if (count % 2 == 1)
        {
            triangles[idx++] = 2 * ex;
            triangles[idx++] = 2 * ex + 1;
            triangles[idx++] = 2 * ex + 2;
        }

        Mesh filledGraphMesh = new Mesh();
        filledGraphMesh.vertices = graphPoints;
        filledGraphMesh.triangles = triangles;

        GameObject filledGraph = new GameObject("Filled graph");
        CanvasRenderer renderer = filledGraph.AddComponent<CanvasRenderer>();
        renderer.SetMesh(filledGraphMesh);
        renderer.SetMaterial(innerFill, null);
        filledGraph.transform.SetParent(InnerFilledGroup);
        filledGraph.AddComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}