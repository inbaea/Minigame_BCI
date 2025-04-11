using UnityEngine;

public class Graph : MonoBehaviour
{
    public GameObject Dot;

    void Start() //시작할때
    {
        for (float i = -45; i < 45; i += 0.05f)
        { //0.05씩 늘려가며 -45에서 45가 될때까지 반복
            DrawDot(Dot, linearFunction(1, 0, i));
        }
    }

    void DrawDot(GameObject obj, Vector2 posi)
    { //점 찍는 함수
        GameObject dot = Instantiate(obj) as GameObject; //점 생성
        dot.transform.position = posi; //점을 posi위치로 옮김.
    }
    Vector2 linearFunction(float Rotation, float plusY, float X)
    { //1차 함수
        float Y;
        Y = Rotation * X + plusY; //y = ax + b (일차함수) a가 기울기라서 Rotation으로 지정, b는 y절편이라고도 하기에 plusY로 지음.
        return new Vector2(X, Y); //우리가 넣은 X에 대응하는 Y를 계산하여 순서쌍 (X,Y)리턴
    }
}
