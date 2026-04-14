using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// 현재
public class NonCachingLoadExample : MonoBehaviour
{
    public string bundleURL;
    public string assetName;

    IEnumerator Start()
    {
        // URL 로부터 파일 다운로드. (Cache 폴더로 저장하지 않음)
        using (UnityWebRequest request = UnityWebRequest.Get(bundleURL))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("fail :( " + request.error);
                yield break;
            }

            // 다운로드된 바이트 데이터를 이용해 AssetBundle 메모리 로드
            byte[] bundleData = request.downloadHandler.data;

            AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromMemoryAsync(bundleData);
            yield return bundleRequest;

            AssetBundle bundle = bundleRequest.assetBundle;

            if (bundle == null)
            {
                Debug.LogError("AssetBundle load failed.");
                yield break;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                // 1. 번들 안에 있는 첫 번째 에셋 아무거나 로드
                //// mainAsset 은 구식 방식이라 사용하지 않고,
                //// 번들 안의 에셋 이름 목록을 가져와 첫 번째 에셋을 로드한다.
                //string[] assetNames = bundle.GetAllAssetNames();

                //if (assetNames.Length > 0)
                //{
                //    AssetBundleRequest assetRequest = bundle.LoadAssetAsync<Object>(assetNames[0]);
                //    yield return assetRequest;

                //    Object loadedObj = assetRequest.asset;

                //    if (loadedObj != null)
                //    {
                //        Instantiate(loadedObj);
                //    }
                //    else
                //    {
                //        Debug.LogWarning("First asset load failed.");
                //    }
                //}
                //else
                //{
                //    Debug.LogWarning("No assets in bundle.");
                //}

                // 2. Cube 1 고정 로드
                // assetName 이 비어 있으면 "Cube 1" 을 로드
                //AssetBundleRequest assetRequest = bundle.LoadAssetAsync<Object>("Cube 1");
                //yield return assetRequest;

                //Object loadedObj = assetRequest.asset;

                //if (loadedObj != null)
                //{
                //    Instantiate(loadedObj);
                //}
                //else
                //{
                //    Debug.LogWarning("Cube 1 not found.");
                //}

                Debug.LogWarning("assetName is empty.");
            }
            else
            {
                // 3. 지정한 이름의 에셋 로드
                AssetBundleRequest assetRequest = bundle.LoadAssetAsync<Object>(assetName);
                yield return assetRequest;

                Object loadedObj = assetRequest.asset;

                if (loadedObj != null)
                {
                    Instantiate(loadedObj);
                }
                else
                {
                    Debug.LogWarning("Asset not found: " + assetName);
                }
            }

            // 메모리 반환을 위해 AssetBundle 압축 데이터 언로드
            bundle.Unload(false);
        }
    }

    void Update()
    {
        // Time.realtimeSinceStartup : 게임 시작 후 계산된 실제 시간
        // Caching.ClearCache();
        // Caching.IsVersionCached(...);
        // Caching.maximumAvailableDiskSpace;
        // Caching.spaceFree;
    }
}

//using UnityEngine;
//using System.Collections;

//// 과거
//class NonCachingLoadExample : MonoBehaviour
//{
//    public string bundleURL;
//    public string AssetName;

//    IEnumerator Start()
//    {
//        // URL 로부터 파일 다운로드. (Cache 폴더로 세이브 되지 않는다)
//        using (WWW www = new WWW(bundleURL))
//        {
//            yield return www;
//            if (www.error != null)
//            // throw new Exception("WWW download had an error:" + www.error);
//            {
//                Debug.Log("fail :(");
//            }
//            AssetBundle bundle = www.assetBundle;
//            if (AssetName == "")
//                Instantiate(bundle.mainAsset);
//            else
//                Instantiate(bundle.LoadAsset(AssetName));
//            // 메모리 반환을 위해 압축되어진 contents 의 AssetBundle을 Unload.
//            bundle.Unload(false);

//        } //  메모리는 web stream 으로부터 반환 된다(www.Dispose() 자동으로 불러진다)
//    }

//    void Update()
//    {
//        // Time.realtimeSinceStartup 게임시작후 계산된 실제 시간
//        // Caching.ClearCache();
//        // Caching.IsVersionCached(url)
//        // Caching.maximumAvailableDiskSpace
//        // Caching.spaceFree
//    }
//}


/*
 * 
 * AssetBundle을 다운로드하는 방법에 두 가지
 * 
 *  1.Non-caching: 새로운 WWW 객체를 생성하여 수행. (현재 스크립트)
 *    AssetBundle은 로컬 저장 장치의 Unity 캐시 폴더에 캐시되지 않음.
 *    
 *  2.Caching: 이 작업은 WWW.LoadFromCacheOrDownload 호출을 사용하여 수행. 
 *    AssetBundle은 로컬 저장 장치의 Unity 캐시 폴더에 캐시됨. 
 *    PC / Mac 독립 실행 형 응용 프로그램 및 iOS / Android 응용 프로그램의 최대 용량은 4GB 이다. 
 *
 */


/* C#에서 using 키워드라고 하면 네임스페이스를 추가하는 키워드 라고 생각하는데
 * using 키워드는 네임스페이스 추가 기능 말고도 다른 기능이 있다.
 * 
 * 1. using은 하나의 블록을 이루고 변수를 선언 할 수있다.
 * 2. using 블록에서 선언한 변수는 using 블록 내부에서만 접근 할 수 있으며
 * 3. 블록이 종료되면 Dispose()를 호출 시키고 변수는 소멸
 * 
 * 정리하면 IDisposable(인터페이스)을 구현한 객체를 읽기 전용으로 사용하는 것을
 * 자동으로 관리해주는 기능
 * 
 * IDisposable 인터페이스는 Dispose() 메소드 하나만을 가지고 있다.
 * 이 IDisposable 인터페이스를 구현한 클래스는 Dispose() 클래스를 
 * 구현해야 하며 여기서 자신의 메모리 할당 내역을 정리해야 한다.(c++ 소멸자)
 * 여기서 WWW 역시 Dispose() 클래스를 구현하고 있으며 여기서 
 * 리소스를 정리하는 적절한 처리가 되어있을 것이다.
 * 
 * using (IDisposable 인터페이스를 구현한 클래스 인스턴스 선언) {
 *  // 리소스 정리가 비정상적으로 이루어질 가능성이 있는 처리
 * }
 *
 * 
 * cf)
 * 
 * Disposing dis = new Disposing();
 * 
 * using(dis)
 * {
 * 
 *   // 로직 처리 
 *   
 * } // dis.Dispose()
 * 
 * int val = dis.Value; // 예외 발생 
 * 
 * 정리) using 외부에 선언한 변수도 using 블럭에 할당할 수 있다.
 *     하지만 using 블럭이 종료되면 Dispose()가 호출되어 
 *     null이 될 수 있으나, 스코프가 유지되므로 다른 변수가 참조하여
 *     문제를 일으킬 수 있다.
 *     
 * 활용) 반드시 Dispose()를 호출해서 소멸해야 하는 객체에서 몇 가지 값을 
 * 읽은 다음 자동으로 폐기하고 싶을 때 사용한다.
 * 
 * 예로 들어, 특정 객체가 생성됨과 동시에 특정 배열에 참조를 보낸다...
 * Dispose()를 호출하지 않는 이상 그 객체의 참조는 배열에 유지된다.
 * 그런데 프로그래머가 실수로 이 개체의 새 객체를 생성한 뒤 값을
 * 읽고 작업을 마친 다음, Dispose()를 호출하지 않고 함수를 종료하고 리턴 했다고 하자
 * 함수가 리턴했지만 그 객체는 메모리에 그대로 남게 된다. 왜냐하면 함수 내부에서
 * 선언한 참조는 소멸되었으나 배열에서의 참조가 남아 있기 때문이다
 * 게다가 배열에 여러 객체의 참조가 이미 들어 있다면 제거해야 할 그 객체를 찾기가
 * 곤란해 진다. 이런 시나리오에서 프로그래머의 실수를 막아주는 것이 using 구문이다.
 * using을 이용한 경우는 자동으로 Dispose()가 호출된다. 반면 프로그래머가 실수를 한 뒤 
 * 배열의 요소 수를 보면 Dispose()가 호출되지 않아 그대로 배열에 상주하고 있을거다.
 * 실수한 객체가 한 두 개라면 적당히 인덱스를 줘서 지울 수도 있겠으나 빈번히 추가된다면
 * 손 쓸 방법이 없다. 이런 문제를 미연에 방지하고자 있는 구문이 using 구문이다.
 */



/*
 *  IDisposable 인터페이스 
 *  
 *  C#은 가비지콜랙터(Garbage Collector)를 가지고 있다.
 *  이 GC는 기본적으로 관리되는 모든 객체들의 참조 링크를 관리하며 더이상 참조되지 않는 객체들을
 *  자동으로 메모리에서 소거하는 작업을 수행함. 
 *  그러나 GC는 창 핸들, 열린 파일, 스트림과 같이 관리되지 않는 리소스들을 인식하지 못함.
 *  
 *  인터페이스란? 클래스에 포함한 여러 속성,메소드 등을 틀만 정해두고 상속받는 클래스에서 오버라이딩하게 하는 것
 *  
 *  interface IAAA
 *  {
 *  }
 *  
 *  인터페이스 이름 앞에 대문자 I는 관례적으로 붙혀준다.
 *  
 *  추상화 클래와 차이점
 *  
 *  추상화 클래스 : 상속을 통해서만 사용할 수 있는 기반 클래스(추상 클래스)
 *  - 필드와 메소드(추상 메소드=>"상속 받는 클래스에서 정의"는 제외 ) 가능
 *  (예제 : Abstract 참조)
 *   
 *  인터페이스
 *  - 필드 지정(불가능) : Field(필드) => 변수(멤버변수)로 객체 상태를 저장하는 곳으로서
 *    클래스나 구조체 내부에 사용되는 변수를 말 한다. (예제 : Field,FieldUse 참조)  
 *  - 메소드(불가능) :추상 메소드가 아닌 인터페이스에서 선언시
 *  - 생성자(불가능) :인터페이스 자체로는 객체를 생성 불가능
 *  - 이벤트(불가능) : 이벤트는 인터페이스에 선언 불가
 * 
 *  - 속성 : get, set 속성을 설정 가능 (추상화 클래스, 인터페이스 둘다 가능)
 *  - 추상 메소드 : 추상화 클래스에서 선언만 허용 하고 상속받는 객체에서 정의 해야함
 *    (예제 : Property,Interface,InterfaceUse,CsharpStudy 참조)
 *  
 * 
 * 
 *  문제점)
 *  
 *  StreamReader reader = new StreamReader("content.txt");
 *  
 *  string content = reader.ReadToEnd();
 *  
 *   // content 활용 코드 수행
 *   
 *  reader.Close();
 *  
 *  위의 코드는 ReadToEnd() 메소드를 수행하는 과정에서 오류가 발생할 가능성이 있다. 
 *  이때에 Close()가 호출되지 않고 반환될 가능성이 있으므로 
 *  이러한 문제를 대응하기 위해 흔히들 try-catch-finally 구문을 사용.
 *  
 *  StreamReader reader = new StreamReader("content.txt");
 *  
 *   try {
 *
 *	    string content = reader.ReadToEnd();

 *   } catch(IOException e) {
 *
 *	    Debug.Log ("Error: " + e.Message);

 *   } finally {
 *
 *	    reader.Close();
 *   }
 *
 * 
 * 위의 코드는 ReadToEnd() 메소드를 수행중에 예외가 발생되면 발생한 예외의 내용을 로그에 출력하게 된다.
 * 그리고 예외가 발생하거나 성공하거나 상관없이 Close() 메소드를 정상적으로 수행. 
 * 하지만 이러한 복잡한 과정 없이 Close() 호출을 알아서 호출해주는 구문이 있다!!!. 
 * 위와 같은 방법이 아닌 using 블록을 사용하여 다음과 같이 처리할 수 있다!!!.
 * 
 *  using (StreamReader reader = new StreamReader("content.txt")) {
 *  
 *	    string content = reader.ReadToEnd();
 *	    
 *	    // content 활용 코드 수행
 *  }
 *
 * 위의 코드에 변경점은 using 키워드를 사용했다는 점과 Close() 를 명시적으로 호출하지 않고 있다는 부분!!!.
 * 
 * 
 * IDisposable... 
 * 
 * SteamReader는 TextReader의 자식 클래스 이다.(SteamReader는 TextReader를 상속) 
 * 그리고 이 TextReader는 IDisposable 인터페이스를 구현하고 있습니다. 
 * 
 * cf) public abstract calss TextReade : MarshalByRefObject, IDisposable
 * 
 * abstract :상속을 통해서만 사용할 수 있는 기반 클래스(추상 클래스) 선언
 * 
 * IDisposable 인터페이스 는 관리되지 않는 리소스 해제를 위한 메커니즘을 제공 한다.
 * cf) public interface IDisposable
 * 
 * 메서드) Dispose() -> 관리되지 않는 리소스의 확보, 해제 또는 다시 설정과 관련된
 * 응용 프로그램 정의 작업을 수행
 * 
 * 정리)
 * IDisposable 인터페이스는 Dispose() 메소드 하나만을 가지고 있음.
 * 이 IDisposable 인터페이스를 구현한 클래스는(상속) Dispose() 클래스를 구현해야 하며
 * 여기서 자신의 메모리 할당 내역을 정리해야 한다.(로직 구성 cpp 에 소멸자 처럼...) 
 * 위에서 보여준 예제에서 보여지는 StreamReader 역시 Dispose() 클래스를 구현하고 있으며
 * 여기서 리소스를 정리하는 적절한 처리가 되어있을 것이다.
 *
 * 
 *   using (IDisposable 인터페이스를 구현한 클래스 인스턴스 선언) {
 *       // 리소스 정리가 정상적으로 이루어지지 않을 가능성이 있는 처리
 *   }
 *   
 *   위의 코드는 IDisposable 인터페이스를 구현하고 있는 클래스를 인스턴스화 하여 사용하며
 *   using 블록을 나가는순간 (심지어 오류가 발생하더라도) Dispose() 가 호출되어 
 *   사용한 리소스가 자동으로 정리 된다.
 *
 *
 *  using 블록 내부에서 발생한 예외는 다음과 같은 방법으로 처리할 수 있다
 *  
 *  try {
 *  
 *	        using (StreamReader reader = new StreamReader ("file.txt")) {
 *		        // 리소스 정리가 정상적으로 이루어지지 않을 가능성이 있는 처리
 *	        }
 *
 *      } catch(IOException e) {
 *
 *	    Debug.Log ("Error: " + e.Message);
 *
 *      }
 *      
 *  (설명) using 바깥쪽에 try-catch 문을 사용해도 using 블록을 빠져나갈 때 Dispose() 가 호출됩니다.


 */


/*
 * AssetBundles를 다운로드하는 방법은 위 방법과 WWW.LoadFromCacheOrDownload를 사용하는 것이다
 * 상황에 맞는 적절한 선택을 하자.
 * 
 * 
using System;
using UnityEngine;
using System.Collections;

public class CachingLoadExample : MonoBehaviour {
   public string bundleURL;
   public string AssetName;
   public int version;

   void Start() {
       StartCoroutine (DownloadAndCache());
   }

   IEnumerator DownloadAndCache (){
       //cache 폴더에 AssetBundle 을 담아야 하므로 캐싱 시스템이 준비될때까지 기달림
       while (!Caching.ready)
           yield return null;

       // 만약 같은 버전이 Cache 폴더에 있는경우 AssetBundle file을 Cache 폴더로부터 로드하고 아니면 URL 로부터 Cache 폴더로 다운로드
       using(WWW www = WWW.LoadFromCacheOrDownload (bundleURL, version)){
           yield return www;
           if (www.error != null)
               throw new Exception("WWW download had an error:" + www.error);
           AssetBundle bundle = www.assetBundle;
           if (AssetName == "")
               Instantiate(bundle.mainAsset);
           else
               Instantiate(bundle.LoadAsset(AssetName));
                    // 메모리 반환을 위해 압축되어진 contents 의 AssetBundle을 Unload.
                   bundle.Unload(false);

       } //  메모리는 web stream 으로부터 반환 된다(www.Dispose() 자동으로 불러진다)
   }
}
*/

/* Caching 클래스
 * 
 * Caching.ready : 캐싱 클래스의 캐싱 작업 준비여부를 담고 있는 프로퍼티
 * 
 * Caching.CleanCache() : 캐싱 폴더를 통째로 털어 버림 => ClearCache() 이걸로 바뀜..유니티 자주 바꾸는거 그만!!
 * 
 * Caching.IsVersionCached(url, version) : 실제적으로 에셋 번들 메니저보단 패치 메니저에 많이 쓰이는 함수로서
 * 이 url(에셋번들)의 현재 버전의 존재 여부를 리턴.
 * 
 * Caching.maximumAvailableDiskSpace : ()가 없는 프로퍼티 타입으로 캐시폴더가 할당할 저장장치의 용량을 설정할 수 있다.
 * byte 단위이기 때문에 1G를 설정하려면 1 * 1024 * 1024 * 1024를 해야한다. (패치 메니저에서 사용)
 * 
 * Caching.spaceFree : ()가 없는 프로퍼티로서 읽기전용(ReadOnly)형태로서 캐시폴더의 남은 용량을 알 수 있다.(패치 메니저에서 사용)
 * 
 * 에셋 번들의 버전별 캐싱, 용량 관리법
 * 
 * for(int i = 0; i < 5; i++)
 * {
 *      using ( WWW www = WWW.LoadFromCacheOrDownload(url, i) )
 *      {
 *          yield return www;
 *      }
 * }
 * 
 * 위와 같이 반복문을 돌려가며 버전 정보를 다르게 하면 같은 에셋 번들이 몇개든 캐싱된다.
 * 이러한 버전 기법이 간단하고 유용하지만, 반대로 엄청 위험한 기능이다.
 * 예를 들어 50명의 캐릭터 모델링과 텍스쳐, 매터리얼일 들어있는 ab_char 이라는 에셋 번들이
 * 있다고 가정하겠다. 이는 캐시 폴더에 0번 버전으로 캐싱 되어 있다. 이름은 www.url.com0.ab_char
 * 라고 하자...그런데 디자이너가 한 모델링의 텍스쳐를 수정 하였다?? 그렇다면 업데이트가 필요하게 된다.
 * 수정한 에셋 번들 ab_char를 버전 1로 다시 캐싱하도록 하겠다..여기가 문제이다...다시 ab_char를 버전 1로
 * 캐싱해도 이전의 버전 0은 날라가지 않는다. 거의 사용하는 리소스와 같은 용량의 데이터가 의미없이 존재하는
 * 것이다. 그렇다고 Caching.ClearCache()를 실행할 수도 없다.(참고로 구 버전에선 Caching.CleanNamedCache(url) 이라는
 * 함수가 있었으나 사라짐 ㅜㅜ) 매번 패치때마다 모든 번들을 받게 할 수는 없다!!!!.
 * 그렇기 때문에 우리는 Caching.maximumAvailableDiskSpace 와 Caching.spaceFree 를 이용한 장난을 좀 쳐야 한다.
 * 에셋 번들은 캐싱 폴더에 적재된 이후에 두가지 이유로 지워진다. 첫 번째는 위에서 말한 수동삭제 Caching.ClearCache() 함수...
 * 저장장치를 싹다 날리는 무서운 함수이다. 두 번째로는 캐시 폴더의 용량이 가득 찬 경우 이다. 번들을 받아야 하는데 더는
 * 저장할 공간이 없는 경우이다. 이런 경우에 Unity3D 엔진은 오래된 에셋 번들부터 목을 쳐 나간다.
 * 한마디로 (구)버전의 에셋 번들을 알아서 지워준다는 것이다. 때문에 현재 클라이언트의 용량을 파악하여 적당한 최대용량을
 * 설정하는 것이 효율적이라고 할 수 있겠다. 하지만 여기에도 주의할 사항이 있다.
 * 자주 업데이트되지 않는 에셋 번들들의 경우인데, 예를 들면 사운드파일이 묶인 번들같은 경우가 해당할 것이다.
 * 이런 자주 업데이트되지 않는 에셋번들은 자칫 삭제대상이 될 수 있으므로 가끔가다 재갱신을 해주어야 한다.
 * 다른 방법으로는 다운받은 에셋 번들을 Filestream 등을 이용하여 다시 원하는 경로로 빼는 방법도 있다.
 * 
 */

