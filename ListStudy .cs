using System.Collections.Generic;
using UnityEngine;

public class ListStudy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Program1 test1 = new Program1();
        test1.test1();
        test1.test2();
    }
}

class Program1
{
    public void test1()
    {
        List<int> number = new List<int>();     //리스트 생성

        number.Add(10);      //리스트 추가
        number.Add(20);      //리스트 추가
        number.Add(30);      //리스트 추가

        Debug.Log(number[0]);        //[0] 번째 리스트 출력
        Debug.Log(number[1]);        //[1] 번째 리스트 출력
        Debug.Log(number[2]);        //[2] 번째 리스트 출력

        // 둘다 주석 풀면 error 이유 알기~!
        //number.Remove(10);      //리스트에서 10을 삭제 합니다
        number.RemoveAt(2);      //리스트에서 인덱스 2에 30을 삭제 합니다

        Debug.Log(number[0]);       //[0] 번째 리스트 출력
        Debug.Log(number[1]);       //[1] 번째 리스트 출력
        //Debug.Log(number[3]);       //[2] 번째 리스트 출력 error

        int I = 20;     //체크할 숫자 선언

        if (number.Contains(I))     //리스트에서 숫자 체크
        {
            Debug.Log(I + " 이 리스트에 존재함");

        }
    }

    public void test2()
    {
        // List
        List<string> list = new List<string>();

        // 추가
        list.Add("a");
        list.Add("b");
        list.Add("c");

        // 탐색
        foreach (string it in list)
        {
            Debug.Log(it);
        }

        // 찾기
        string key = "키"; // 여기선 b 넣어보자
        string result = list.Find(
            delegate (string data) { return (key == data); }
            );

        if (result != null)
        {
            Debug.Log(result);
        }
    }

    public void test3()
    {
        List<int> sortList = new List<int>();
        sortList.Add(10);
        sortList.Add(3);
        sortList.Add(7);
        sortList.Add(1);
        sortList.Add(4);

        // 정렬 !!
        sortList.Sort(delegate (int a, int b) {

            Debug.Log(a.CompareTo(b));

            return a.CompareTo(b);

        });

        /* [인스턴스].CompareTo(value) 
         * 
         *       반환 값                            의미 
         *       
         *       (-1 출력) 0보다 작음               이 인스턴스는 value보다 작습니다. 
         *       ( 0 출력) 0                        이 인스턴스는 value와 같습니다. 
         *       ( 1 출력) 0보다 큼                 이 인스턴스는 value보다 큽니다. 
         */

        // 탐색
        foreach (int it in sortList)
        {
            Debug.Log(it);
        }

        /*
        //이렇게 하면 더 간단함.

        List<int> sortList = new List<int>();
        sortList.Add(10);
        sortList.Add(3);
        sortList.Add(7);
        sortList.Add(1);
        sortList.Add(4);

        // 알아서 정렬 !!
        sortList.Sort();
        */
    }
}

// 해당 조건에 맞는 리스트 반환
class Program2
{
    public void test1()
    {
        //리스트 생성
        List<string> dinosaurs = new List<string>();

        dinosaurs.Add("Compsognathus");
        dinosaurs.Add("Amargasaurus");
        dinosaurs.Add("Oviraptor");
        dinosaurs.Add("Velociraptor");
        dinosaurs.Add("Deinonychus");
        dinosaurs.Add("Dilophosaurus");
        dinosaurs.Add("Gallimimus");
        dinosaurs.Add("Triceratops");

        //EndsWithSaurus를 사용하여 검색
        Debug.Log(dinosaurs.Find(EndsWithSaurus));


        /* 대리자 메소드
         * List.Find() => Predicate 형식 
         * 
         * 메서드를 사용한 검색 -> Predicate
         * list 집합을 대상으로 각각 요소마다 해당 메서드를 호출
         * 메서드로 내부조건을 검사해서 조건이 True인 해당 요소객체 반환
         * 검색할 함수는 list집합의 요소를 매개변수로 가지고 bool 값을 반환하는 메서드로...
         * 
         * 대리자 메소드인  List.Find는 리스트에서 결과값을 찾음 빠져나가고  List.FindAll은 찾고나서도 끝까지 검색
         * 
         */

        //RemoveAll 공부

        //리스트 갯수
        Debug.Log(dinosaurs.Count);

        //조건 리스트 삭제(익명==무명)  
        dinosaurs.RemoveAll(EndsWithSaurus);
        //리스트 갯수
        Debug.Log(dinosaurs.Count);

        //조건 리스트 삭제(무명=익명)
        dinosaurs.RemoveAll(
            delegate (string data)
            {
                return (data == "Velociraptor" || data == "Gallimimus");

            }
         );
        //리스트 갯수
        Debug.Log(dinosaurs.Count);

        //모든 리스트 삭제
        dinosaurs.Clear();
        //리스트 갯수
        Debug.Log(dinosaurs.Count);
    }

    //찾으면 true를 반환 
    private static bool EndsWithSaurus(string s)
    {
        if ((s.Length > 5) &&
            (s.Substring(s.Length - 6).ToLower() == "saurus"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /* C# string에서  Substring(StartIndex), Replace(문자변환), ToUpper(대문자), ToLower(소문자), Trim(공백제거)
     * 
     * Substring()은 인자로 전달되는 StartIndex 번째 자리부터 끝까지 제외한 나머지 문자를 자른다. 
     * string val = "t..t"; => val.Replace("t","^^"); => t..t 가 ^^..^^ 로 변환
     * string val = "abc" => val.ToUpper(); =>"ABC" 로 변환
     * string val = "     abc" => val.Trim(); => "abc" 로 변환
     * 
     */
}

//무명대리자 이용 !! (무명 메서드)
class Program3
{
    public void test1()
    {
        //리스트 생성
        List<string> dinosaurs = new List<string>();

        dinosaurs.Add("Compsognathus");
        dinosaurs.Add("Amargasaurus");
        dinosaurs.Add("Oviraptor");
        dinosaurs.Add("Velociraptor");
        dinosaurs.Add("Deinonychus");
        dinosaurs.Add("Dilophosaurus");
        dinosaurs.Add("Gallimimus");
        dinosaurs.Add("Triceratops");

        string searchopt = "saurus";


        Debug.Log(dinosaurs.Find(

            //무명대리자 이용
            delegate (string s)
            {
                if ((s.Length > 5) &&
                 (s.Substring(s.Length - 6).ToLower() == searchopt))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        ));


    }

    public void test2()
    {
        //리스트 생성
        List<string> dinosaurs = new List<string>();
        
        dinosaurs.Add("Compsognathus");
        dinosaurs.Add("Amargasaurus");
        dinosaurs.Add("Oviraptor");
        dinosaurs.Add("Velociraptor");
        dinosaurs.Add("Deinonychus");
        dinosaurs.Add("Dilophosaurus");
        dinosaurs.Add("Gallimimus");
        dinosaurs.Add("Triceratops");

        string searchopt = "saurus";

        //List.FindAll은 검색한 객체의 리스트를 반환
        List<string> dinoList = dinosaurs.FindAll(

            //무명대리자 이용
            delegate (string s)
            {
                if ((s.Length > 5) &&
                 (s.Substring(s.Length - 6).ToLower() == searchopt))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        );

        foreach (string _dino in dinoList)
        {
            Debug.Log(_dino);
        }


    }

    /*
     * (활용)
     * IDtoFind는 조건자 대리자를 사용하여 ID로 책을 찾는다.
     * 즉, 익명 대리자에서 조건을 판별하여 리스트에서 처음으로 검색한 요소를 반환
     * Book result = Books.Find(
       delegate (Book bk)
     * {
     *     return bk.ID == IDtoFind; // IDtoFind=> 익명 대리자
     * }
     * );
     * if (result != null)
     * {
     *     DisplayResult(result, "Find by ID: " + IDtoFind);
     *     Debug.Log(result.ID + "Find");
     * }
     * else
     * {
     *     Debug.Log(result.ID + "not Found");
     * }
     * 
     * 
     */
}


/*  c# List
 * 리스트는 배열과 동일한 역할을 한다.
 * List<자료형> 변수명 형태로 선언하며, 만들어지는 것이 객체이기 때문에 new연산자를 꼭 써줘야 한다!
 * 리스트 사용을 위해 꼭 using System.Collections.Generic; 네임스페이스를 추가
 * 
 * List 선언법:
   List<자료형 또는 클래스> _list = new List<자료형 또는 클래스>();
   (cf) List 또한 객체이기 때문에 꼭 new를 해주자.
 
   주요 메소드:
   _list.Add() : 원하는 자료를 컨테이너에 삽입 할 수 있다.
   _list.Remove(자료) : 원하는 자료를 컨테이너에서 삭제할 수 있다. 
   _list.RemoveAt(인덱스) : 원하는 자료를 인덱스 번호로 삭제할 수 있다.

 * 
 * 배열과 다른점: 
 * 1. 리스트는 크기를 정해줄 필요가 없다... 그때 그때 Add함수를 사용하여 리스트 값을 추가할 수 있으며
 * 이때 추가한 순서대로 리스트에 [x]번째가 된다.
 * 2. Remove를 이용해 언제든지 list의 원하는값을 삭제 가능하다. 이때, 삭제된 [x]번째보다 뒤에 있는 리스트값은
 * 한칸씩 전부 당겨진다. (메모리 공간 활용능력이 좋다)
 * 3. 배열은 원하는 값이 있는지 확인하기 위해서 배열의 개수만큼 전부 반복문으로 체크해봐야 한다. 그러나 리스트는
 * Contains()를 이용하여 원하는 값이 있는지 바로 판단 가능하다.
 * 
 * 
 * 
 * 
 * 
 */
