using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
/// <summary>
/// 拡張クラス
/// </summary>
public static class Extra
{

    /// <summary>
    /// 拡張関数
    /// </summary>
    /// <typeparam name="T"><リストの一要素/typeparam>
    /// <param name="list"><リストの本体このリスト自体に変更を入れるわけではない/param>
    /// <param name="lostID"><移動元の要素番号/param>
    /// <param name="nextID"><移動先の要素番号/param>
    public static List<T> ChengeOrder<T>(List<T> list, int lostID, int nextID)
    {
        List<T> dummyList = new List<T>();
        //ネクストの一個前まで追加
        for (int i = 0; i < nextID; i++) { dummyList.Add(list[i]); }

        int startPos = 0;

        if (nextID < lostID)
        {
            dummyList.Add(list[lostID]);
            dummyList.Add(list[lostID - 1]);
            startPos = lostID + 1;
        }
        else
        {

            dummyList.RemoveAt(lostID);
            dummyList.Add(list[lostID + 1]);
            dummyList.Add(list[lostID]);
            startPos = nextID + 1;
        }

        for (int i = startPos; i < list.Count; i++) { dummyList.Add(list[i]); }


        return dummyList;


    }


    /// <summary>
    /// リストの中に条件を入れてその条件にあった物の個数を返す関数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static int GetCount<T>(this List<T> values, System.Func<T, bool> func)
    {
        int count = 0;

        for (int i = 0; i < values.Count; i++)
        {
            if (!func(values[i])) continue;
            count++;
        }
        return count;
    }

    /// <summary>
    /// リストを使い他の型に変更して返す関数
    /// </summary>
    /// <typeparam name="T"><変更前の型/typeparam>
    /// <typeparam name="T2"><変更後の型/typeparam>
    /// <param name="values"><使用するリスト/param>
    /// <param name="func"><変更後の型を返す関数/param>
    /// <returns></returns>
    public static List<T2> GetSeparateList<T,T2>(this List<T> values, System.Func<T,T2> func) 
    {
        List<T2> list = new();

        for(int i = 0; i < values.Count; i++) 
        {
            list.Add(func(values[i]));

        }

        return list;

    }

    /// <summary>
    /// リストの重複した値を削除したリストを返す関数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<int> GetDuplicateDelete(this List<int> list) 
    {
        List<int> result = new();
        List<int> dommy = list;
        for(int i = 0; i < dommy.Count; i++) 
        {

            if (dommy.GetCount(value => value == dommy[i]) > 1)
            {

                dommy.RemoveAt(i);
                i--;
                continue;
            }


            result.Add(dommy[i]);


        }
        return result;

    }

    /// <summary>
    /// 対象のリストから同じ値のインデックス番号のリストを返す関数
    /// 戻り値はvalueのインデックス番号で-1が入っているのはガッチがない
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="traget"></param>
    /// <returns></returns>
    public static List<int> SearchListIndex<T>(this List<T> value, List<T> traget) 
    {
        List<int> result=new();

        for(int i = 0; i < traget.Count; i++) 
        {
            for(int j = 0; j < value.Count; j++) 
            {
                if (!EqualityComparer<T>.Default.Equals(traget[i], value[j])) continue;
                result.Add(j);
                break;
            }
            // resultの配列番号とtargetの番号を合わせるために埋める
            if ((i + 1) != result.Count) result.Add(-1);


        }
        return result;
    }

    /// <summary>
    /// 引数２のリストのインデックス番号だけをリストにまとめて返す関数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="indexs"></param>
    /// <returns></returns>
    public static List<T>GetList<T>(this List<T> value,List<int> indexs)
    {
        List<T> result = new();

        for(int i=0;i<indexs.Count;i++)
            result.Add(value[indexs[i]]);
        return result;
    }

    /// <summary>
    /// リストの全てに同じ関数を使用する
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values"></param>
    /// <param name="action"></param>
    public static void GetAction<T>(this List<T> values, System.Func<T, T> action)
    {
        for (int i = 0; i < values.Count; i++)
        {
            values[i] = action(values[i]);

        }
    }

    /// <summary>
    /// AI
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public static bool IsCross(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        float ta = Cross(b - a, c - a);
        float tb = Cross(b - a, d - a);

        float tc = Cross(d - c, a - c);
        float td = Cross(d - c, b - c);

        return ta * tb < 0 && tc * td < 0;
    }

    /// <summary>
    /// AI
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Cross(Vector2 a, Vector2 b)
    {
        return a.x * b.y - a.y * b.x;
    }

    public static Vector2 Vector2ToZ(this Vector3 vector3) 
    {
        return new Vector2(vector3.z, vector3.y);
    }
    public static Vector2 Vector2ToX(this Vector3 vector3) 
    {
        return new Vector2(vector3.x, vector3.y);
    }

    /// <summary>
    /// 最大値と最小値を固定する関数
    /// </summary>
    /// <param name="vector3"></param>
    /// <param name="Max"></param>
    /// <param name="Min"></param>
    /// <returns></returns>
    public static Vector3 MaxMinValue(this Vector3 vector3,float Max,float Min) 
    {
        vector3.x = Mathf.Clamp(vector3.x,Min,Max);
        vector3.y = Mathf.Clamp(vector3.y,Min,Max);
        vector3.z = Mathf.Clamp(vector3.z,Min,Max);

        return vector3;

    }

}