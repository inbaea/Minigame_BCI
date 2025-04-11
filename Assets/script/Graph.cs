using UnityEngine;

public class Graph : MonoBehaviour
{
    public GameObject Dot;

    void Start() //�����Ҷ�
    {
        for (float i = -45; i < 45; i += 0.05f)
        { //0.05�� �÷����� -45���� 45�� �ɶ����� �ݺ�
            DrawDot(Dot, linearFunction(1, 0, i));
        }
    }

    void DrawDot(GameObject obj, Vector2 posi)
    { //�� ��� �Լ�
        GameObject dot = Instantiate(obj) as GameObject; //�� ����
        dot.transform.position = posi; //���� posi��ġ�� �ű�.
    }
    Vector2 linearFunction(float Rotation, float plusY, float X)
    { //1�� �Լ�
        float Y;
        Y = Rotation * X + plusY; //y = ax + b (�����Լ�) a�� ����� Rotation���� ����, b�� y�����̶�� �ϱ⿡ plusY�� ����.
        return new Vector2(X, Y); //�츮�� ���� X�� �����ϴ� Y�� ����Ͽ� ������ (X,Y)����
    }
}
