// ControllerManager.cs
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

public class ControllerManager : MonoBehaviour
{
    public static ControllerManager Instance { get; private set; }

    [Header("最大プレイヤー数")]
    [SerializeField] private int maxPlayers = 4;

    // --- 外部購読用イベント ---
    /// <summary>
    /// プレイヤー番号(1始まり), 接続:true/切断:false を通知
    /// </summary>
    public event Action<int,bool> OnGamepadConnectionChanged;

    // プレイヤー番号(内部0始まり) -> Gamepad
    private readonly Dictionary<int,Gamepad> playerToPad = new Dictionary<int,Gamepad>();
    // デバイスID -> プレイヤー番号(内部0始まり)（復帰時に元の割り当てをなるべく維持）
    private readonly Dictionary<int,int> deviceIdToPlayer = new Dictionary<int,int>();

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        RebuildMapping(); // 起動時に割り当て
        InputSystem.onDeviceChange += OnDeviceChange; // 接続/切断などを監視
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            InputSystem.onDeviceChange -= OnDeviceChange;
            Instance = null;
        }
    }

    private void OnDeviceChange(InputDevice device,InputDeviceChange change)
    {
        if(!(device is Gamepad)) return;

        switch(change)
        {
            case InputDeviceChange.Added:
            case InputDeviceChange.Enabled:
            case InputDeviceChange.Reconnected:
            case InputDeviceChange.Removed:
            case InputDeviceChange.Disabled:
            case InputDeviceChange.Disconnected:
                RebuildMapping();
                break;
        }
    }

    /// <summary>
    /// 現在接続されている Gamepad を maxPlayers の範囲でプレイヤーに割り当て直す。
    /// 可能なら以前のプレイヤー番号を維持。
    /// </summary>
    private void RebuildMapping()
    {
        // 今の接続デバイス（ID昇順で安定化）
        var pads = Gamepad.all.OrderBy(p => p.deviceId).ToList();

        // 旧状態を保持（通知判定用）
        var oldMapping = new Dictionary<int,Gamepad>(playerToPad);

        // 既存の割り当てをクリア
        playerToPad.Clear();

        // 1. 以前に使っていた deviceId は元のプレイヤー番号を優先して再割り当て
        var takenPlayers = new HashSet<int>();
        foreach(var pad in pads)
        {
            if(deviceIdToPlayer.TryGetValue(pad.deviceId,out var playerIdx))
            {
                if(playerIdx >= 0 && playerIdx < maxPlayers && !takenPlayers.Contains(playerIdx))
                {
                    playerToPad[playerIdx] = pad;
                    takenPlayers.Add(playerIdx);
                }
            }
        }

        // 2. 空いているプレイヤースロットに残りのパッドを割り当て
        foreach(var pad in pads)
        {
            // 既に割り当て済みならスキップ
            if(playerToPad.Values.Contains(pad)) continue;

            int slot = FirstEmptyPlayerSlot(takenPlayers);
            if(slot == -1) break;

            playerToPad[slot] = pad;
            deviceIdToPlayer[pad.deviceId] = slot;
            takenPlayers.Add(slot);
        }

        // 接続が外れた deviceId は掃除
        var aliveIds = new HashSet<int>(pads.Select(p => p.deviceId));
        var oldKeys = deviceIdToPlayer.Keys.ToList();
        foreach(var id in oldKeys)
        {
            if(!aliveIds.Contains(id))
            {
                // 維持したいなら消さずに残す実装も可
                deviceIdToPlayer.Remove(id);
            }
        }

        // --- 接続/切断の変化を通知 ---
        for(int i = 0;i < maxPlayers;i++)
        {
            bool wasConnected = oldMapping.ContainsKey(i);
            bool isConnected = playerToPad.ContainsKey(i);

            if(wasConnected != isConnected)
            {
                OnGamepadConnectionChanged?.Invoke(i + 1,isConnected); // 1始まりで通知
            }
        }
    }

    private int FirstEmptyPlayerSlot(HashSet<int> taken)
    {
        for(int i = 0;i < maxPlayers;i++)
        {
            if(!taken.Contains(i)) return i;
        }
        return -1;
    }

    // --- ここからパブリックAPI（1始まりで統一） ---

    /// <summary>
    /// 指定プレイヤー(1始まり)の Gamepad を取得
    /// </summary>
    public bool TryGetGamepad(int playerNum,out Gamepad pad)
    {
        int idx = playerNum - 1; // 外部は1始まり -> 内部0始まり
        if(idx < 0 || idx >= maxPlayers)
        {
            pad = null;
            return false;
        }

        if(playerToPad.TryGetValue(idx,out pad) && pad != null)
        {
            return pad.added && pad.enabled && pad.device != null;
        }
        pad = null;
        return false;
    }

    /// <summary>
    /// 指定プレイヤー(1始まり)のボタンが「このフレームで押されたか」を非同期で待つ（キャンセル可）
    /// </summary>
    public async UniTask WaitForButtonDownAsync(int playerNum,GamepadButton button,CancellationToken ct = default)
    {
        while(!ct.IsCancellationRequested)
        {
            if(TryGetGamepad(playerNum,out var pad))
            {
                if(WasPressedThisFrame(pad,button))
                    return;
            }

            await UniTask.Yield(PlayerLoopTiming.Update,ct);
        }

        ct.ThrowIfCancellationRequested();
    }

    /// <summary>
    /// 指定プレイヤー(1始まり)のボタンが現在押下されているか
    /// </summary>
    public bool IsPressed(int playerNum,GamepadButton button)
    {
        return TryGetGamepad(playerNum,out var pad) && IsPressed(pad,button);
    }

    /// <summary>
    /// 指定プレイヤー(1始まり)が現在接続されているか
    /// </summary>
    public bool IsConnected(int playerNum)
    {
        return TryGetGamepad(playerNum,out _);
    }

    /// <summary>
    /// 現在割り当てられているプレイヤー数（接続中のパッド数と最大数の小さい方）
    /// </summary>
    public int CurrentPlayerCount => playerToPad.Count;

    /// <summary>
    /// 指定プレイヤー(1始まり)の左/右スティック値を返す（存在しなければ Vector2.zero）
    /// </summary>
    public Vector2 GetLeftStick(int playerNum) =>
        TryGetGamepad(playerNum,out var pad) ? pad.leftStick.ReadValue() : Vector2.zero;

    public Vector2 GetRightStick(int playerNum) =>
        TryGetGamepad(playerNum,out var pad) ? pad.rightStick.ReadValue() : Vector2.zero;

    // --- 以下、GamepadButton を ButtonControl にマッピング ---

    private static bool WasPressedThisFrame(Gamepad pad,GamepadButton button)
    {
        if(TryGetControl(pad,button,out var control))
        {
            return control.wasPressedThisFrame;
        }

        // トリガーなど Axis 系は簡易判定（0.5閾値）。必要なら状態保持して厳密ダウン判定を実装。
        if(TryGetAxisPressed(pad,button,out var pressed))
        {
            // 「押された瞬間」を厳密に取りたい場合は過去フレームの pressed を保持して遷移検出する
            // ここでは簡易に現在値のみで代替
            return pressed;
        }

        return false;
    }

    private static bool IsPressed(Gamepad pad,GamepadButton button)
    {
        if(TryGetControl(pad,button,out var control))
        {
            return control.isPressed;
        }

        if(TryGetAxisPressed(pad,button,out var pressed))
        {
            return pressed;
        }

        return false;
    }

    private static bool TryGetControl(Gamepad pad,GamepadButton button,out ButtonControl control)
    {
        control = null;
        switch(button)
        {
            // Face buttons (Xbox想定: South=A, East=B, West=X, North=Y)
            case GamepadButton.South: // A
                control = pad.buttonSouth; return true;
            case GamepadButton.East:  // B
                control = pad.buttonEast; return true;
            case GamepadButton.West:  // X
                control = pad.buttonWest; return true;
            case GamepadButton.North: // Y
                control = pad.buttonNorth; return true;

            // Menu系
            case GamepadButton.Start:
                control = pad.startButton; return true;
            case GamepadButton.Select:
                control = pad.selectButton; return true;

            // Shoulders
            case GamepadButton.LeftShoulder:
                control = pad.leftShoulder; return true;
            case GamepadButton.RightShoulder:
                control = pad.rightShoulder; return true;

            // Sticks (押し込み)
            case GamepadButton.LeftStick:
                control = pad.leftStickButton; return true;
            case GamepadButton.RightStick:
                control = pad.rightStickButton; return true;

            // D-Pad
            case GamepadButton.DpadUp:
                control = pad.dpad.up; return true;
            case GamepadButton.DpadDown:
                control = pad.dpad.down; return true;
            case GamepadButton.DpadLeft:
                control = pad.dpad.left; return true;
            case GamepadButton.DpadRight:
                control = pad.dpad.right; return true;
        }
        return false;
    }

    /// <summary>
    /// トリガー(L/R)をボタン的に扱いたいときの簡易押下判定（しきい値）。
    /// 「押された瞬間」まで厳密に取りたい場合は過去値を保持して遷移検出を実装してください。
    /// </summary>
    private static bool TryGetAxisPressed(Gamepad pad,GamepadButton button,out bool isPressed)
    {
        const float PressThreshold = 0.5f;
        isPressed = false;
        switch(button)
        {
            case GamepadButton.LeftTrigger:
                isPressed = pad.leftTrigger.ReadValue() >= PressThreshold; return true;
            case GamepadButton.RightTrigger:
                isPressed = pad.rightTrigger.ReadValue() >= PressThreshold; return true;
            default:
                return false;
        }
    }
}
