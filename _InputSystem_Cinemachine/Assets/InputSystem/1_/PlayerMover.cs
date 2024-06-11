using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Invoke C Sharp Events

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
    private Controls _cintrols;

    [SerializeField] private float _moveForce = 5;
    [SerializeField] private float _jumpForce = 5;
    private Rigidbody _rigidbody;   
    private Vector2 _moveInputValue;

    private void OnEnable()// GameObjectが有効化された時
    {
        _rigidbody = GetComponent<Rigidbody>();       
        _cintrols = new Controls();// Actionスクリプトのインスタンス生成。Input Actionの初期化

        // Actionイベントに　「登録」

        //Action Type = Valueの場合
        _cintrols.Player.Move.started   += OnMove;//started   – 入力が0から0以外に変化したとき
        _cintrols.Player.Move.performed += OnMove;//performed – 入力が0以外に変化したとき
        _cintrols.Player.Move.canceled  += OnMove;//canceled  – 入力が0以外から0に変化したとき

        //Action Type = Buttonの場合
        _cintrols.Player.Jump.performed += OnJump;//started   – 入力が0から0以外に変化したとき
                                                    
                                                    //canceled  – 入力が0以外から0に変化したとき、またはperformedが呼ばれた後に入力の大きさが閾値Release以下に変化したとき

        //閾値の設定は、トップメニューのEdit > Project Settings > Input System Packageの項目から変更できます。
        //閾値Pressの値はDefault Press Button Point、
        //閾値Releaseの値は「Press × Button Release Threshold」となります。閾値ReleaseはPressと掛け算した値であることに注意する必要があります

        _cintrols.Enable();// Input Actionを機能させるためには、「有効化」　する必要がある
    }

    private void OnDisable()// 無効化された時　　　 このメソッドじゃなくても、ポーズ画面でActionMapを使いたくない時などに　使ってもいい
    {      
        _cintrols.Player.Move.started  -= OnMove;
        _cintrols.Player.Move.performed -= OnMove;
        _cintrols.Player.Move.canceled  -= OnMove;
        _cintrols.Player.Jump.performed -= OnJump;

        _cintrols?.Disable();// 自身が無効化されるタイミングなどで　// Actionを無効化する
    }

    private void OnDestroy()//破壊された時
    {
        _cintrols?.Dispose();// 自身でインスタンス化したActionクラスはIDisposableを実装しているので、必ずDisposeする必要がある
    }

    //Disable()メソッドは、入力アクションを一時的に無効化しますが、再度有効化(Enable())することが可能です。例えば、ゲームの一時停止やメニューの表示時に一時的に入力を無効化したい場合に使用します。
    //Dispose()メソッドは、入力アクションを完全に破棄し、リソースを解放します。これを呼び出すと、再度有効化することはできません。アプリケーションの終了時や、もうその入力アクションを一切使用しないことが確定している場合に使用します。





    private void OnMove(InputAction.CallbackContext context)
    {       
        if (context.started) {
            Debug.Log("入力が0から0以外に変化した");
            return;
        }

        _moveInputValue = context.ReadValue<Vector2>();// Moveアクションの入力取得　// Actionの入力値を読み込む
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)//performed – 入力の大きさが閾値Press以上に変化したとき
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);// ジャンプする力を与える
    }



    //③
        //Action Type = Pass Throughの場合
        //基本的にデバイス入力がある間にperformedが呼ばれ続けます。
        //デバイスが切り替わった場合は、切り替わり前のデバイスが無効（Disabled）となり、
        //canceledコールバックが呼び出されます。
//performed 入力があったら
//canceled Action無効化







    private void FixedUpdate()
    {
        // 移動方向の力を与える
        _rigidbody.AddForce(new Vector3(
            _moveInputValue.x,
            0,
            _moveInputValue.y
        ) * _moveForce);
    }

    void Update()
    {       
        var keyboardCurrent = Keyboard.current;// 現在のキーボード情報

        if (keyboardCurrent == null)// キーボード接続チェック // キーボードが接続されていないと // Keyboard.currentがnullになる  
        {            
            return;
        }
     
        var aKey = keyboardCurrent.aKey; // Aキーの入力状態取得

        if (aKey.wasPressedThisFrame) // Aキーが押された瞬間かどうか
        {
            Debug.Log("Aキーが押された！");
        }
       
        if (aKey.wasReleasedThisFrame) // Aキーが離された瞬間かどうか
        {
            Debug.Log("Aキーが離された！");
        }
        
        if (aKey.isPressed) // Aキーが押されているかどうか
        {
            Debug.Log("Aキーが押されている！");
        }


        
        var mouseCurrent = Mouse.current;// 現在のマウス情報

        if (mouseCurrent == null) // マウス接続チェック // マウスが接続されていないと // Mouse.currentがnullになる
        {      
            return;
        }
    
        var cursorPosition = mouseCurrent.position.ReadValue(); // マウスカーソル位置取得       
        var mouseLeftButton = mouseCurrent.leftButton;     // 左ボタンの入力状態取得
        
        if (mouseLeftButton.wasPressedThisFrame) // 左ボタンが押された瞬間かどうか
        {
            Debug.Log($"左ボタンが押された！ {cursorPosition}");
        }
        
        if (mouseLeftButton.wasReleasedThisFrame)// 左ボタンが離された瞬間かどうか
        {
            Debug.Log($"左ボタンが離された！{cursorPosition}");
        }
        
        if (mouseLeftButton.isPressed)// 左ボタンが押されているかどうか
        {
            Debug.Log($"左ボタンが押されている！{cursorPosition}");
        }
    }
}
