using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectPool : MonoBehaviour
{
    // pool에서 관리 및 가지고 있을 객체 -> GameObject 

    //pool에 사용되는 객체 레퍼런스
    public GameObject prefab;
    public int poolSize = 30;
    //미리 만들 객체의 수 == 개발자의 재량
    //크게 잡을수록 성능적으로 손해 (메모리 할당량 증가) 
    // 개발자 - 인간적인 실수가 발생 - 알고리즘 X , 코드 X  

    // 객체를 담을 Pool 그릇 -container 
    // Queue 초기화 , 생성 
    private Queue<GameObject> objectPool = new Queue<GameObject>();

    private void Awake()
    {
        //Pool 의 구조적인 원리 
        //- 게임이 시작되기전에 미리 준비되어 있어야한다
        //- 시작하자마자 총알을 미리 생성을 요청을 (난타) 
        // 총알이 안나거나, 
        // New 미리 여러개 -> Start 에서 하면, 렉 Frame drop이 관측

        // Pool 게임 시작전 초기화 생성 
        InitializePool();
    }

    private void InitializePool()
    {
        //  poolSize 만큼 prefab 게임오브젝트를 미리 생성해서 
        // 큐에 담아놓자 
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            // 비활성화해서 담아두자 
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
    }

    // 총알 쏘기 탄창
    // 꺼내는 주체는 -> 총
    // 총한테 쏴라 -> player 
    // *꺼내기 -> 대여 --> 외부에서 호출되는 함수 

    //꺼내진 GameObject 를 반환하는 형식으로 함수를 제작
    public GameObject GetObject()
    {
        //pool 에 deque 할수 있는 객체가 있으면 ~ 
        if (objectPool.Count > 0)
        {
            //꺼내기 Dequeue
            GameObject obj = objectPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else // pool에 
        {
            // 풀이 부족할경우 -> 추가생성 or null 반환
            GameObject obj = Instantiate(prefab);
            obj.SetActive(true);
            return obj;

            // Debug.Log("pool 객체가 모잘라서 추가 pool 필요");
            // return null; 
        }
    }

    // 사용된 객체 (삭제 X) -> 비활성화 후 -> Pool 다시 넣기
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }
}
