using UnityEngine;
//유니티 인스펙터를 수정하기 위한 네임스페이스 (유니티 개조) => 자신만의 엔진 만들자 
using UnityEditor;

// 선언시 게임오브젝트 다중선택 가능(같은 컴포넌트 보이게...)
[CanEditMultipleObjects]
//AutoMoved의 인스펙터를 바꿀거다 따라서 클래스 위에 어트리뷰트 선언
[CustomEditor(typeof(AutoMove))]

//상속을 MonoBehaviour=>Editor 바꾸자...또한 MonoBehaviour 관련 함수를 지움.
public class EditorCreate : Editor {

    //선택된 오브젝트를 받아오기 위한 배열
    AutoMove[] selectObjects;

    //인스펙터를 보여주는 OnInspectorGUI를 오버라이드
    public override void OnInspectorGUI()
    {
        //AutoMove 컴포넌트 참조
        AutoMove autoM = (AutoMove)target;

        //AutoMove 컴포넌트의 moveSpeed를 hahaha 란 이름으로 노출(FloatField는 float 관련 함수)
        //EditorGUILayout으로 원하는 필드를 추가할 수 있다.
        //다음과 같이 오버로딩 되어있다.
        //EditorGUILayout.IntField("변수이름", int 값);
        //EditorGUILayout.FloatField("변수이름", float 값);
        //다음에 autoM.moveSpeed = 는 인스펙터에 노출하는 것 뿐 아니라 인스펙터에
        //입력한 값을 실제 변수에 적용해야 하므로 다시 autoM.moveSpeed에 대입...
        autoM.moveSpeed = EditorGUILayout.FloatField("hahaha", autoM.moveSpeed);
        autoM.testNum = EditorGUILayout.IntField("testNum", autoM.testNum);
        //LabelField는 hehehe 값(autoM.instance)을 인스펙터에 노출 하면서, 임의로 수정 못하게 할때 쓰임  
        //LabelField는 문자열을 받는다 따라서 autoM.instance는 float형이 리턴 되므로 ToString 사용
        EditorGUILayout.LabelField("hehehe", autoM.Instance.ToString());

        //한번 호출시 원래 스크립트가 표현하는 인스펙터
        //표현 방식을 그대로 보여줌 (스크립트 참조가 포함됨)
        //DrawDefaultInspector();

        //인스펙터에 버튼을 추가하여 버튼 클릭시 해당 메서드 실행
        if(GUILayout.Button("Origin Point"))
        {
            autoM.OriginSet();
        }

        //인스펙터에 경고,주의, 인포등을 띄울 수 있다.
        EditorGUILayout.HelpBox("안녕하세요!~ 좋은 하루!!~~~^^", MessageType.Info);

        //세로 라인에 추가
        EditorGUILayout.BeginVertical();
        //가로 라인에 추가
        //EditorGUILayout.BeginHorizontal();

        //다음에 오는 버튼들은 위 셋팅으로 정렬 된다.

        // 클릭된 오브젝트의 이름을 보여준다.(만들어진 순서로 스텍에 쌓임)
        if (GUILayout.Button("Show name +"))
        {
            for (int i = 0; i < selectObjects.Length; i++)
            {
                Debug.Log(selectObjects[i].name);
            }
        }

        // 오브젝트의 이름을 반대로 보여준다.
        if (GUILayout.Button("Show name -"))
        {
            for (int i = selectObjects.Length-1; i >= 0; i--)
            {
                Debug.Log(selectObjects[i].name);
            }
        }

        //End~() 정렬 후 항상 써줘야 된다.
        EditorGUILayout.EndVertical();
        //EditorGUILayout.EndHorizontal();

        //인스펙터의 요소의 값이 변경되면 호출
        if (GUI.changed)
        {
            /*EditorUtility.SetDirty(target); 이 함수 호출은
             * 유니티는 dirty 플래그를 사용하여 자산이 변경되면
             * 디스크에 저장해야하는시기를 찾는다.
             * 게임오브젝트의 인스펙터에 요소를 변경 후 플레이를 눌르면
             * 값이 저장되지 않고 날가가고 원래 값으로 (플레이 전 값) 
             * 돌아간다...이때 그 값을 디스크에 저장해서 Asst의 인스펙터의
             * 값을 바꾸는 함수.
             * 
             * (참고) http://www.devkorea.co.kr/reference/Documentation/ScriptReference/EditorUtility.SetDirty.html
             */

            EditorUtility.SetDirty(target);
            Debug.Log("changed");
        }

    }

    //활성화 될 때마다 호출되는 함수입니다.(Awake/Start와 달리 활성화 될 때마다...)
    //여기선 클릭할때도 호출된다...
    void OnEnable()
    {
        //선택된 오브젝트를 가져와 배열에 저장
        selectObjects = ToGetObj(targets);
        Debug.Log(selectObjects.Length);
    }


    //선택된 오브젝트의 AutoMove들을 참조하는 배열을 리턴
    AutoMove[] ToGetObj(Object[] objs)
    {
        AutoMove[] _selectObj = new AutoMove[objs.Length];

        for (int i = 0; i < objs.Length; i++)
        {
            _selectObj[i] = objs[i] as AutoMove;
        }
        //배열 이름을 전달
        return _selectObj;
    }

}
