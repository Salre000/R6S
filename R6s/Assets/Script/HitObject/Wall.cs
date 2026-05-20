using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Wall : HitObject
{
    MeshFilter meshFilter;

    private readonly float THICKNESS = 1f;

    private const float renge = 0.1f;

    private Vector3[] testPos =
        {
        new Vector3(renge,renge,0),
        new Vector3(renge,-renge,0),
        new Vector3(-renge,renge,0),
        new Vector3(-renge,-renge,0)
        };


    public Wall(GameObject wall)
    {
        SetHitObject(wall);

        meshFilter = wall.GetComponent<MeshFilter>();

        int[] triangles =
        {
            0, 1, 2,
            2, 1, 3
        };
        meshFilter.mesh.triangles = triangles;

        HitObjectManager.instance.SetHitObject(this);
        Debug.Log(Vector3.Cross(new Vector3(1, 1, 0), new Vector3(-1, 0, 0)));

    }


    public override void HitAction(int attackID)
    {
        Attack attack = AttackObjectManager.instance.GetAttack(attackID);


        if (attack == null) return;

        MeshCat(attack.GetHitPos(), testPos);


    }


    public override void Update()
    {
    }

    private void MeshCat(Vector3 pos, Vector3[] vector3s)
    {


        List<int> hitPolygon = new List<int>();

        pos.z = GetHitObject().transform.position.z + 0.5f;

        for (int j = 0; j < vector3s.Length; j++)
        {
            Vector3 vector = pos + vector3s[j];
            for (int i = 0; i < meshFilter.mesh.triangles.Length; i += 3)
            {
                if (
                Triangle3D.IsPointInTriangle
                    (
                    pos
                    , GetHitObject().transform.TransformPoint(meshFilter.mesh.vertices[meshFilter.mesh.triangles[i]])
                    , GetHitObject().transform.TransformPoint(meshFilter.mesh.vertices[meshFilter.mesh.triangles[i + 1]])
                    , GetHitObject().transform.TransformPoint(meshFilter.mesh.vertices[meshFilter.mesh.triangles[i + 2]])

                    )
                   )
                {
                    hitPolygon.Add(i);
                    hitPolygon.Add(i+1);
                    hitPolygon.Add(i+2);
                }
            }
        }


        List<int> VerticalIndex = meshFilter.mesh.triangles.ToList();
        List<int> HitVerticalIndex = new List<int>();

        hitPolygon = hitPolygon.Distinct().ToList();

        // 当たったポリゴンを記録して削除している
        for (int i = 0; i < hitPolygon.Count; i++)
        {
            HitVerticalIndex.Add(VerticalIndex[hitPolygon[i]]);
            //DebugTEST.Instance.ShowDebugObject(GetHitObject().transform.TransformPoint(meshFilter.mesh.vertices[VerticalIndex[hitPolygon[i]]]));
            VerticalIndex[hitPolygon[i]] = -1;

        }
        for (int i = 0; i < VerticalIndex.Count; i++)
            if (VerticalIndex[i] == -1)
            {
                VerticalIndex.RemoveAt(i);
                i--;
            }


        //当たったポリゴンを削除
        meshFilter.mesh.triangles = VerticalIndex.ToArray();


        int count = meshFilter.mesh.vertexCount;

        List<Vector3> Dvertices = new List<Vector3>();
        Vector3 hitPos = (pos - GetHitObject().transform.position);
        hitPos.x /= GetHitObject().transform.localScale.x;
        hitPos.y /= GetHitObject().transform.localScale.y;

        for (int i = 0; i < vector3s.Length; i++)
        {

            Vector3 v = vector3s[i] + hitPos;


            Dvertices.Add(v);
        }

        Reattach(Dvertices, HitVerticalIndex, hitPos, count);



    }

    /// <summary>
    /// 穴を開けた事を基準にメッシュの貼り直しを行う関数
    /// outPosがIntなのは既にメッシュに入っていてそれのインデックス番号でいいから
    /// </summary>
    private void Reattach(List<Vector3> inPos, List<int> outPosIndex, Vector3 hitPos, int startIndex)
    {
        List<Vector3> outPos = new List<Vector3>();
        List<Vector3> vertices = new List<Vector3>(meshFilter.mesh.vertices);

        for (int i = 0; i < outPosIndex.Count; i++)
            outPos.Add(meshFilter.mesh.vertices[outPosIndex[i]]);

        // inPosの角度を取得
        List<float> inAngles = new List<float>();
        for (int i = 0; i < inPos.Count; i++)
        {
            Vector3 vector = inPos[i] - hitPos;

            float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

            DebugTEST.Instance.ShowDebugObject(GetHitObject().transform.TransformPoint(vector + hitPos), angle.ToString());

            if (angle < 0) angle += 360f;

            inAngles.Add(angle);

        }

        // inAngleを基準に順番を入れ替える
        for (int i = 0; i < inPos.Count; i++)
        {
            for (int j = 0; j < inPos.Count; j++)
            {
                if (i == j) continue;

                if (inAngles[i] > inAngles[j]) continue;

                float dummyAngle = inAngles[j];
                inAngles[j] = inAngles[i];
                inAngles[i] = dummyAngle;

                Vector3 dummyPos = inPos[j];
                inPos[j] = inPos[i];
                inPos[i] = dummyPos;
            }
        }

        // outPosの角度を取得
        List<float> outAngles = new List<float>();
        for (int i = 0; i < outPosIndex.Count; i++)
        {
            Vector3 vector = outPos[i];

            float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

            DebugTEST.Instance.ShowDebugObject(GetHitObject().transform.TransformPoint(vector),angle.ToString());

            if (angle < 0) angle += 360f;

            outAngles.Add(angle);

        }

        // outAngleを基準に順番を入れ替える
        for (int i = 0; i < outPosIndex.Count; i++)
        {
            for (int j = 0; j < outPosIndex.Count; j++)
            {
                if (i == j) continue;

                if (outAngles[i] > outAngles[j]) continue;

                float dummyAngle = outAngles[j];
                outAngles[j] = outAngles[i];
                outAngles[i] = dummyAngle;

                Vector3 dummyPos = outPos[j];
                outPos[j] = outPos[i];
                outPos[i] = dummyPos;
            }
        }

        // 中央が2点かどうかを判断
        bool inFlag = true;

        // 一番小さな角度がin||out　のどちらか判断する
        if (inAngles[0] > outAngles[0]) inFlag = false;

        // outPosのカウント
        int outCount = 0;

        // メッシュのインデックス番号配列
        List<int> indexs = new List<int>();

        // メッシュのインデックス番号を追加
        for (int inCount = 0; inCount < inPos.Count;)
        {
            if (inFlag) 
            { indexs.Add(vertices.IndexOf(outPos[outCount%outPos.Count]));
              indexs.Add(inCount%inPos.Count + startIndex); }
            else 
            { indexs.Add(inCount%inPos.Count + startIndex);
              indexs.Add(vertices.IndexOf(outPos[outCount%outPos.Count])); }

            if (inAngles[(inCount + 1 )% inPos.Count] < outAngles[(outCount + 1) %outPos.Count ])
            { inCount++; indexs.Add(inCount%inPos.Count + startIndex); }
            else if(outCount == outPos.Count - 1)
            { inCount++; indexs.Add(inCount % inPos.Count + startIndex); }
            else
            { outCount++; indexs.Add(vertices.IndexOf(outPos[outCount%outPos.Count])); }

        }

        // 一番最後の始まりと終わりを繋げる
        if (inFlag)
        {
            indexs.Add(vertices.IndexOf(outPos[outPos.Count - 1]));
            indexs.Add(startIndex);
            indexs.Add(vertices.IndexOf(outPos[0]));
        }
        else
        {
            indexs.Add(startIndex);
            indexs.Add(vertices.IndexOf(outPos[outPos.Count - 1]));
            indexs.Add(startIndex + inPos.Count - 1);
        }




        // リストにInPosを追加
        vertices.AddRange(inPos);
        
        //　現在のメッシュのインデックス番号を取得
        List<int> triangles=new List<int>(meshFilter.mesh.triangles);

        // インデックス番号を追加
        triangles.AddRange(indexs);

        // 値をメッシュに戻す
        meshFilter.mesh.vertices= vertices.ToArray();
        meshFilter.mesh.triangles= triangles.ToArray();


    }

}
