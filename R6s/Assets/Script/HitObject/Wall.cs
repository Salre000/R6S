using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Assertions.Must;
using static UnityEditor.PlayerSettings;
using System.Threading.Tasks;
using Unity.VisualScripting;
using System.Linq.Expressions;

public class Wall : HitObject
{
    MeshFilter meshFilter;

    //空白のインデックス番号リスト
    List<int> spaceIndex = new List<int>();
    List<Vector3> spaceVec = new List<Vector3>();


    GameObject dummyObject;
    MeshFilter DummyMeshFilter;

    private readonly float THICKNESS = 1f;

    private const float renge = 0.1f;

    private Vector3[] testPos =
        {

        new Vector3(renge,renge,0),
        new Vector3(renge,-renge,0),
        new Vector3(-renge,-renge,0),
        new Vector3(-renge,renge,0)
        };

    private Vector3[] testMAXPos =
        {

        new Vector3(0.5f,0.5f,0),
        new Vector3(0.5f,-0.5f,0),
        new Vector3(-0.5f,-0.5f,0),
        new Vector3(-0.5f,0.5f,0),
        };


    public Wall(GameObject wall)
    {
        SetHitObject(wall);

        dummyObject = GameObject.Instantiate(wall);

        dummyObject.transform.position += new Vector3(10, 0, 0);

        meshFilter = wall.GetComponent<MeshFilter>();
        DummyMeshFilter = dummyObject.GetComponent<MeshFilter>();
        int[] triangles =
        {
            0, 1, 2,
            2, 1, 3
        };
        meshFilter.mesh.triangles = triangles;
        DummyMeshFilter.mesh.triangles = triangles;

        DummyMeshFilter.mesh = new Mesh();

        HitObjectManager.instance.SetHitObject(this);
    }


    public override void HitAction(int attackID)
    {
        Attack attack = AttackObjectManager.instance.GetAttack(attackID);


        if (attack == null) return;

        List<Vector3> hitPos = new List<Vector3>();
        hitPos.Add(attack.GetHitPos());

        for (int i = 0; i < meshFilter.mesh.triangles.Length; i += 3)
        {

            if (!ComparisonOfRanges(hitPos, i, meshFilter.mesh)) continue;



        }

        MeshCut(attack.GetHitPos(), testPos);


    }


    public override void Update()
    {
    }

    private void MeshCut(Vector3 pos, Vector3[] vector3s)
    {
        DebugTEST.Instance.Lsot();
        DebugTEST.Instance.LostMesh();


        List<int> hitPolygon = new List<int>();
        Vector3 hitPos = (pos - GetHitObject().transform.position);
        hitPos.x /= GetHitObject().transform.localScale.x;
        hitPos.y /= GetHitObject().transform.localScale.y;


        pos.z = GetHitObject().transform.position.z + 0.5f;

        for (int j = 0; j < vector3s.Length; j++)
        {
            Vector3 vector = GetHitObject().transform.TransformPoint(hitPos + vector3s[j]);
            for (int i = 0; i < meshFilter.mesh.triangles.Length; i += 3)
            {
                if (
                Triangle3D.IsPointInTriangle
                    (
                    vector
                    , GetHitObject().transform.TransformPoint(meshFilter.mesh.vertices[meshFilter.mesh.triangles[i]])
                    , GetHitObject().transform.TransformPoint(meshFilter.mesh.vertices[meshFilter.mesh.triangles[i + 1]])
                    , GetHitObject().transform.TransformPoint(meshFilter.mesh.vertices[meshFilter.mesh.triangles[i + 2]])

                    )
                   )
                {
                    hitPolygon.Add(i);
                    hitPolygon.Add(i + 1);
                    hitPolygon.Add(i + 2);
                }
            }
        }

        List<Vector3> HitPosList = new List<Vector3>();
        for (int j = 0; j < vector3s.Length; j++)
        {
            HitPosList.Add(GetHitObject().transform.TransformPoint(hitPos + vector3s[j]));
        }
        for (int i = 0; i < meshFilter.mesh.triangles.Length; i += 3)
        {
            if (ComparisonOfRanges(HitPosList, i, meshFilter.mesh)
               )
            {

                hitPolygon.Add(i);
                hitPolygon.Add(i + 1);
                hitPolygon.Add(i + 2);
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
        HitVerticalIndex = HitVerticalIndex.Distinct().ToList();

        //当たったポリゴンを削除
        meshFilter.mesh.triangles = VerticalIndex.ToArray();

        int count = meshFilter.mesh.vertexCount;

        List<Vector3> Dvertices = new List<Vector3>();

        for (int i = 0; i < vector3s.Length; i++)
        {

            Vector3 v = vector3s[i] + hitPos;

            v = v.MaxMinValue(0.5f, -0.5f);


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
            Vector3 vector = GetHitObject().transform.TransformPoint(inPos[i])
                - GetHitObject().transform.TransformPoint(hitPos);

            float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

            //DebugTEST.Instance.ShowDebugObject(vector + GetHitObject().transform.TransformPoint(hitPos), angle.ToString());

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
            Vector3 vector = GetHitObject().transform.TransformPoint(outPos[i])
                - GetHitObject().transform.TransformPoint(hitPos);

            float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

            //DebugTEST.Instance.ShowDebugObject(GetHitObject().transform.TransformPoint(outPos[i]), angle.ToString());

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
        int inCount = 0;
        // メッシュのインデックス番号配列
        List<int> indexs = new List<int>();

        int meshCount = 0;

        // 基準になる角度
        float ReferenceAngle = 0;

        // メッシュのインデックス番号を追加
        while (true)
        {

            meshCount++;


            indexs.Add(vertices.IndexOf(outPos[outCount % outPos.Count]));
            indexs.Add(inCount % inPos.Count + startIndex);

            if (Mathf.Abs(inAngles[(inCount + 1) % inPos.Count] - ReferenceAngle) <
                Mathf.Abs(outAngles[(outCount + 1) % outPos.Count] - ReferenceAngle))
            {
                inCount++;
                indexs.Add(inCount % inPos.Count + startIndex);
            }
            else
            {
                outCount++;
                indexs.Add(vertices.IndexOf(outPos[outCount % outPos.Count]));

            }

            if ((inPos.Count + outPos.Count) - 1 <= meshCount) break;

            ReferenceAngle = inAngles[inCount % inPos.Count] > outAngles[outCount % outPos.Count] ?
                inAngles[inCount % inPos.Count] : outAngles[outCount % outPos.Count];

        }

        //一番最後の始まりと終わりを繋げる
        indexs.Add(vertices.IndexOf(outPos[outPos.Count - 1]));
        indexs.Add(startIndex + (inPos.Count - 1));
        indexs.Add(vertices.IndexOf(outPos[0]));

        indexs.Add(startIndex + (0));
        indexs.Add(vertices.IndexOf(outPos[0]));
        indexs.Add(startIndex + (inPos.Count - 1));

        //　現在のメッシュのインデックス番号を取得
        List<int> triangles = new List<int>(meshFilter.mesh.triangles);


        // リストにInPosを追加
        vertices.AddRange(inPos);

        spaceVec.AddRange(inPos);

        // インデックス番号を追加
        triangles.AddRange(indexs);

        // 値をメッシュに戻す
        meshFilter.mesh.vertices = vertices.ToArray();
        meshFilter.mesh.triangles = triangles.ToArray();

        spaceIndex.Add(DummyMeshFilter.mesh.vertexCount - (DummyMeshFilter.mesh.vertexCount > 0 ? 4 : 0));
        spaceIndex.Add(DummyMeshFilter.mesh.vertexCount + 2 - (DummyMeshFilter.mesh.vertexCount > 0 ? 4 : 0));
        spaceIndex.Add(DummyMeshFilter.mesh.vertexCount + 1 - (DummyMeshFilter.mesh.vertexCount > 0 ? 4 : 0));
        spaceIndex.Add(DummyMeshFilter.mesh.vertexCount - (DummyMeshFilter.mesh.vertexCount > 0 ? 4 : 0));
        spaceIndex.Add(DummyMeshFilter.mesh.vertexCount + 3 - (DummyMeshFilter.mesh.vertexCount > 0 ? 4 : 0));
        spaceIndex.Add(DummyMeshFilter.mesh.vertexCount + 2 - (DummyMeshFilter.mesh.vertexCount > 0 ? 4 : 0));
        //spaceIndex.Add(DummyMeshFilter.mesh.vertexCount );
        //spaceIndex.Add(DummyMeshFilter.mesh.vertexCount + 2);
        //spaceIndex.Add(DummyMeshFilter.mesh.vertexCount + 1);
        //spaceIndex.Add(DummyMeshFilter.mesh.vertexCount);
        //spaceIndex.Add(DummyMeshFilter.mesh.vertexCount + 3);
        //spaceIndex.Add(DummyMeshFilter.mesh.vertexCount + 2);

        DummyMeshFilter.mesh.vertices = spaceVec.ToArray();
        DummyMeshFilter.mesh.triangles = spaceIndex.ToArray();

        DummyMesh();

    }

    /// <summary>
    /// ダミーのメッシュの周りを取得
    /// </summary>
    private void DummyMesh()
    {

        List<int> vertices = new List<int>();

        List<List<int>> triangles = new List<List<int>>();


        // メッシュのtrianglesをリストリストに変更
        for (int j = 0; j < DummyMeshFilter.mesh.triangles.Length; j += 3)
        {
            triangles.Add(new());
            triangles[j / 3].Add(DummyMeshFilter.mesh.triangles[j]);
            triangles[j / 3].Add(DummyMeshFilter.mesh.triangles[j + 1]);
            triangles[j / 3].Add(DummyMeshFilter.mesh.triangles[j + 2]);
        }

        //　同じ座標を使用しているポリゴンを一つのリストに纏める
        for (int i = 0; i < triangles.Count; i++)
        {
            for (int j = 0; j < triangles.Count; j++)
            {
                if (i == j) continue;

                bool flag = false;

                if (triangles[i].Contains(triangles[j][0])) flag = true;
                if (triangles[i].Contains(triangles[j][1])) flag = true;
                if (triangles[i].Contains(triangles[j][2])) flag = true;


                if (!flag) continue;

                triangles[i].AddRange(triangles[j]);

                triangles.RemoveAt(j);
            }

        }





        for (int i = 0; i < DummyMeshFilter.mesh.vertices.Length; i++)
        {

            for (int j = 0; j < DummyMeshFilter.mesh.triangles.Length; j += 3)
            {
                if (DummyMeshFilter.mesh.triangles[j] == i) continue;
                if (DummyMeshFilter.mesh.triangles[j + 1] == i) continue;
                if (DummyMeshFilter.mesh.triangles[j + 2] == i) continue;


                if (!CheckAreaToPoint(
                    DummyMeshFilter.gameObject.transform.TransformPoint(DummyMeshFilter.mesh.vertices[i]),
                    j,
                    DummyMeshFilter
                    )) continue;

                vertices.Add(i);


                for (int index = 0; index < triangles.Count; index++)
                {
                    if (triangles[index].Contains(i))
                        for (int p = 0; p < triangles.Count; p++)
                        {
                            if (index == p) continue;

                            if (!triangles[p].Contains(DummyMeshFilter.mesh.triangles[j])) continue;

                            triangles[index].AddRange(triangles[p]);




                        }

                }


                break;
            }
        }




        for (int i = 0; i < triangles.Count; i++)
        {
            for (int j = 0; j < triangles.Count; j++)
            {
                if (i == j) continue;

                bool flag = false;
                for (int p = 0; p < triangles[j].Count; p++)
                {


                    if (!triangles[i].Contains(triangles[j][p])) flag = true;

                }

                if (flag) continue;


                i--;
                triangles.RemoveAt(j);
                break;

            }
        }

        for (int i = 0; i < triangles.Count; i++)
        {
            triangles[i] = triangles[i].Distinct().ToList();

            for (int j = 0; j < vertices.Count; j++)
            {
                triangles[i].Remove(vertices[j]);
            }

            Debug.Log(triangles[i].Count + "数と");
        }


        //for (int i = 0; i < triangles[0].Count; i++)
        //{
        //    DebugTEST.Instance.ShowDebugObject(
        //        DummyMeshFilter.gameObject.transform.TransformPoint(DummyMeshFilter.mesh.vertices[triangles[0][i]]));
        //}

        //MeshSystem();

        DummyMeshShow(triangles);
    }

    //
    private void DummyMeshShow(List<List<int>> indexLists)
    {
        List<List<Vector3>> verticesList = new List<List<Vector3>>();

        // intリストリストをVector3リストリストに変更
        for (int i = 0; i < indexLists.Count; i++)
        {
            verticesList.Add(new List<Vector3>());

            for (int j = 0; j < indexLists[i].Count; j++)
            {
                verticesList[i].Add(DummyMeshFilter.mesh.vertices[indexLists[i][j]]);

            }
        }

        List<Vector3> outVec=testMAXPos.ToList();

        DummyMeshFilter.mesh.triangles = new List<int>().ToArray();

        for (int i = 0; i < verticesList.Count; i++)
        {
            MeshSystem(verticesList[i], outVec, DummyMeshFilter.mesh, DummyMeshFilter.gameObject);

            if (i + 1 >= verticesList.Count) return;

            int cash = i + 1;

            //外側の座標を初期化
            outVec.Clear();

            List<int> outVecIndex= new List<int>();

            List<Vector3> cashVertices=new List<Vector3>(verticesList[cash]);

            // 中央の座標を取得
            Vector3 centerPos = Extra.GetCenter(cashVertices);

            // 中央から見て時計回りに並べて変え
            cashVertices.Sort((a, b) =>
            {

                Vector3 vectorA = DummyMeshFilter.gameObject.transform.TransformPoint(a)
                    - DummyMeshFilter.gameObject.transform.TransformPoint(centerPos);

                int angleA = (int)(Mathf.Atan2(vectorA.y, vectorA.x) * Mathf.Rad2Deg);
                if (angleA < 0) angleA += 360;

                Vector3 vectorB = DummyMeshFilter.gameObject.transform.TransformPoint(b)
                    - DummyMeshFilter.gameObject.transform.TransformPoint(centerPos);

                int angleB = (int)(Mathf.Atan2(vectorB.y, vectorB.x) * Mathf.Rad2Deg);
                if (angleB < 0) angleB += 360;

                return angleA.CompareTo(angleB);
            });

            for(int j=0;j< DummyMeshFilter.mesh.triangles.Length; j+=3) 
            {

                bool flag = false;

                List<Vector3> transformPoint=new List<Vector3>();

                for (int p = 0; p < cashVertices.Count; p++) 
                {
                    transformPoint.Add(DummyMeshFilter.gameObject.transform.TransformPoint(cashVertices[p]));
                    if (CheckAreaToPoint(transformPoint[transformPoint.Count-1], j, DummyMeshFilter)) flag = true;

                }

                if (!ComparisonOfRanges(transformPoint, j, DummyMeshFilter.mesh)&&!flag) continue;


                outVecIndex.Add(DummyMeshFilter.mesh.triangles[j]);
                outVecIndex.Add(DummyMeshFilter.mesh.triangles[j+1]);
                outVecIndex.Add(DummyMeshFilter.mesh.triangles[j+2]);
            }

            // 重複した値を一つに纏める
            outVecIndex = outVecIndex.Distinct().ToList();

            List<int> triangles = DummyMeshFilter.mesh.triangles.ToList();

            for(int j = 0; j < DummyMeshFilter.mesh.triangles.Length; j += 3) 
            {
                if (!outVecIndex.Contains(DummyMeshFilter.mesh.triangles[j]))continue;
                if (!outVecIndex.Contains(DummyMeshFilter.mesh.triangles[j+1]))continue;
                if (!outVecIndex.Contains(DummyMeshFilter.mesh.triangles[j+2]))continue;

                triangles[j]=-1;
                triangles[j+1]=-1;
                triangles[j+2]=-1;
            }

            for(int j=0;j< triangles.Count; j++) 
            {
                if (triangles[j] != -1) continue;

                triangles.RemoveAt(j);

                j--;
            }

            DummyMeshFilter.mesh.triangles=triangles.ToArray();

            return;

            for (int j=0;j< outVecIndex.Count; j++) 
            {
                outVec.Add(DummyMeshFilter.mesh.vertices[outVecIndex[j]]);
            }

            // 中央の座標を取得
            centerPos = Extra.GetCenter(outVec);

            outVec.GetAction(vec =>
            {
                DebugTEST.Instance.ShowDebugObject(DummyMeshFilter.gameObject.transform.TransformPoint(vec));

                return vec;

            });

            // 中央から見て時計回りに並べて変え
            outVec.Sort((a, b) =>
            {

                Vector3 vectorA = DummyMeshFilter.gameObject.transform.TransformPoint(a)
                    - DummyMeshFilter.gameObject.transform.TransformPoint(centerPos);

                int angleA = (int)(Mathf.Atan2(vectorA.y, vectorA.x) * Mathf.Rad2Deg);
                if (angleA < 0) angleA += 360;

                Vector3 vectorB = DummyMeshFilter.gameObject.transform.TransformPoint(b)
                    - DummyMeshFilter.gameObject.transform.TransformPoint(centerPos);

                int angleB = (int)(Mathf.Atan2(vectorB.y, vectorB.x) * Mathf.Rad2Deg);
                if (angleB < 0) angleB += 360;

                return angleA.CompareTo(angleB);
            });

        }

        return;

    }

    /// <summary>
    /// outVecの範囲を外側としてinVecの座標の外側にメッシュを貼る
    /// </summary>
    /// <param name="inVec"></param>
    /// <param name="outVec"><中央から見て時計回りに並んでいる必要性あり/param>
    /// <param name="mesh"></param>
    /// <param name="game"></param>
    private void MeshSystem(List<Vector3> inVec, List<Vector3> outVec, Mesh mesh, GameObject game)
    {

        List<Extra.Line> lines = new List<Extra.Line>();

        List<Vector3> vertices = mesh.vertices.ToList();

        List<List<int>> angleIndex = new List<List<int>>(outVec.Count);

        // 外側の座標の数だけリストを生成
        for (int i = 0; i < outVec.Count; i++)
            angleIndex.Add(new List<int>());

        // 穴の内側への線を取得
        for (int j = 0; j < inVec.Count; j++)
        {
            for (int p = 0; p < inVec.Count; p++)
            {
                if (j == p) continue;

                Extra.Line line = new Extra.Line(
                     DummyMeshFilter.gameObject.transform.TransformPoint(inVec[j]).Vector2ToX(),
                     DummyMeshFilter.gameObject.transform.TransformPoint(inVec[p]).Vector2ToX());

                lines.Add(line);

            }
        }

        // 穴から正しい角への線を取得
        for (int j = 0; j < inVec.Count; j++)
        {
            for (int p = 0; p < outVec.Count; p++)
            {
                Extra.Line line = new Extra.Line
                    (
                    game.transform.TransformPoint(inVec[j]).Vector2ToX(),
                    game.transform.TransformPoint(outVec[p]).Vector2ToX()
                    ); ;

                if (!Extra.LineIntersections(lines, line).CheckZeroVector3()) continue;

                angleIndex[p].Add(vertices.IndexOf(inVec[j]));
                lines.Add(line);
            }
        }

        List<int> indexs = new List<int>(mesh.triangles.ToArray());

        for (int j = 0; j < outVec.Count; j++)
        {
            // angleIndexの起点になっているoutVecから見て時計回りに並べて変え
            angleIndex[j].Sort((a, b) =>
            {

                Vector3 vectorA = game.transform.TransformPoint
                (mesh.vertices[a])
                    - game.transform.TransformPoint
                    (outVec[j]);

                int angleA = (int)(Mathf.Atan2(vectorA.y, vectorA.x) * Mathf.Rad2Deg);

                if (angleA < 0) angleA += 360;

                Vector3 vectorB = game.transform.TransformPoint
                (mesh.vertices[b])
                    - game.transform.TransformPoint
                    (outVec[j]);

                int angleB = (int)(Mathf.Atan2(vectorB.y, vectorB.x) * Mathf.Rad2Deg);

                if (angleB < 0) angleB += 360;


                return angleA.CompareTo(angleB);
            });

            for (int p = 0; p < angleIndex[j].Count - 1; p++)
            {

                indexs.Add(mesh.vertexCount + j);
                indexs.Add(angleIndex[j][p + 1]);
                indexs.Add(angleIndex[j][p]);
            }
        }
        for (int j = 0; j < angleIndex.Count; j++)
        {
            for (int p = 0; p < angleIndex[(j + 1) % angleIndex.Count].Count; p++)
            {
                if (!angleIndex[j].Contains(angleIndex[(j + 1) % angleIndex.Count][p])) continue;

                indexs.Add(j + mesh.vertexCount);
                indexs.Add(((j + 1) % angleIndex.Count) + mesh.vertexCount);
                indexs.Add(angleIndex[(j + 1) % angleIndex.Count][p]);
            }
        }

        List<Vector3> vec = mesh.vertices.ToList();

        vec.AddRange(outVec.ToList());

        mesh.vertices = vec.ToArray();
        mesh.triangles = indexs.ToArray();
    }

    /// <summary>
    /// 範囲ないに範囲が被っているかどうかを判断
    /// 被っている場合はtrueを返す
    /// </summary>
    /// <returns></returns>
    private bool ComparisonOfRanges(List<Vector3> hitPos, int startIndex, Mesh mesh)
    {
        for (int i = 0; i < hitPos.Count; i++)
        {

            Vector2 startVec = hitPos[i].Vector2ToX();
            Vector2 endVec = hitPos[(i + 1) % hitPos.Count].Vector2ToX();

            for (int j = 0; j < 3; j++)
            {

                if (Extra.IsCross(startVec, endVec,
                        GetHitObject().transform.TransformPoint
                        (mesh.vertices[mesh.triangles[j + startIndex]])
                        .Vector2ToX(),
                        GetHitObject().transform.TransformPoint
                        (mesh.vertices[mesh.triangles[((j + 1) % 3) + startIndex]])
                        .Vector2ToX()
                    )) return true;

            }

        }





        return false;

    }

    /// <summary>
    /// ポリゴンの内側に座標が存在するかどうかを判断
    /// </summary>
    /// <param name="point"></param>
    /// <param name="StartIndex"></param>
    /// <param name="mesh"></param>
    /// <returns></returns>
    private bool CheckAreaToPoint(Vector3 point, int StartIndex, MeshFilter mesh)
    {
        if (
        Triangle3D.IsPointInTriangle
            (
            point
            , mesh.gameObject.transform.TransformPoint
            (mesh.mesh.vertices[mesh.mesh.triangles[StartIndex]])
            , mesh.gameObject.transform.TransformPoint
            (mesh.mesh.vertices[mesh.mesh.triangles[StartIndex + 1]])
            , mesh.gameObject.transform.TransformPoint
            (mesh.mesh.vertices[mesh.mesh.triangles[StartIndex + 2]])

            )
           )
        {
            return true;
        }


        return false;
    }
}
