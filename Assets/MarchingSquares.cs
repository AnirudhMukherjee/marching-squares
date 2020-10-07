using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingSquares : MonoBehaviour
{
    public int width = 600;
    public int height = 400;
    public int resolution = 10;
    public float perlx, perly;
    public float inc = 0.1f;
    int rows, columns;
    float [,] scalarGrid;
    public GameObject cube;
    public LineRenderer lineRenderer;
    public Material lineMat;

    // Start is called before the first frame update
    void Start()
    {
        rows = height / resolution;
        columns = width / resolution;
        scalarGrid = new float[rows,columns];
        
    }

    void line(Vector3 a, Vector3 b)
    {
        //For creating line renderer object
        lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
        lineRenderer.material = lineMat;
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;

        //For drawing line in the world space, provide the x,y,z values
        lineRenderer.SetPosition(0, a); //x,y and z position of the starting point of the line
        lineRenderer.SetPosition(1, b); //x,y and z position of the starting point of the line
    }
    int findCase(int a,int b,int c,int d)
    {
        Debug.Log(a + ""+ b + ""+ c+ ""+d);
        return (a * 8 + b * 4 + c * 2 + d);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < rows; i++)
        {
            perlx += inc;
            for (int j = 0; j < columns; j++)
            {
                scalarGrid[i, j] = Mathf.PerlinNoise(perlx, perly);
                perly += inc;
                cube = Instantiate(cube, new Vector3(i * resolution, 0, j * resolution), Quaternion.identity);
                Renderer rend = cube.transform.GetComponent<Renderer>();
                //rend.material.SetColor("Color", new Color(scalarGrid[i, j], scalarGrid[i, j], scalarGrid[i, j]));
                rend.material.color = new Color(scalarGrid[i, j], scalarGrid[i, j], scalarGrid[i, j]);
            }
        }

        for (int i = 0; i < rows - 1; i++)
        {
            for (int j = 0; j < columns - 1; j++)
            {
                float x = i * (float)resolution;
                float y = j * (float)resolution;

                Vector3 a = new Vector3(x + resolution * 0.5f, 0, y);
                Vector3 b = new Vector3(x + resolution, 0, y + resolution * 0.5f);
                Vector3 c = new Vector3(x + resolution * 0.5f, 0, y + resolution);
                Vector3 d = new Vector3(x, 0, y + resolution * 0.5f);
                int caseNo = findCase(Mathf.RoundToInt(scalarGrid[i, j]), Mathf.RoundToInt(scalarGrid[i + 1, j]), Mathf.RoundToInt(scalarGrid[i + 1, j + 1]), Mathf.RoundToInt(scalarGrid[i, j + 1]));
                switch (caseNo)
                {
                    case 1:
                        line(c, d);
                        break;
                    case 2:
                        line(b, c);
                        break;
                    case 3:
                        line(b, d);
                        break;
                    case 4:
                        line(a, b);
                        break;
                    case 5:
                        line(a, d);
                        line(b, c);
                        break;
                    case 6:
                        line(a, c);
                        break;
                    case 7:
                        line(a, d);
                        break;
                    case 8:
                        line(a, d);
                        break;
                    case 9:
                        line(a, c);
                        break;
                    case 10:
                        line(a, b);
                        line(c, d);
                        break;
                    case 11:
                        line(a, b);
                        break;
                    case 12:
                        line(b, d);
                        break;
                    case 13:
                        line(b, c);
                        break;
                    case 14:
                        line(c, d);
                        break;

                }
            }
        }
        StartCoroutine(deleteLines());
    }

    private IEnumerator deleteLines()
    {
        
        yield return new WaitForSeconds(2.5f);
        var objects = GameObject.FindObjectsOfType<LineRenderer>();
        foreach (LineRenderer o in objects)
        {
            Destroy(o.gameObject);
        }
    }
}
