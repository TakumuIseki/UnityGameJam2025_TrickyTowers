using System.Runtime.InteropServices;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Logicool Dual Action用コントローラー設定
/// Logicool F310rコントローラーは背面にXInput/DirectInput切り替えスイッチがあり、
/// DirectInputモード時にジョイスティックとして認識されるため、入力が認知されない。
/// そのため、DirectInputモード時にもXInputとして認識されるように設定を追加する。
/// </summary>
[InputControlLayout(stateType = typeof(LogicoolDuualActionState))]
#if UNITY_EDITOR
// UnityEditor起動時に構成を登録
[InitializeOnLoad]
#endif
public class LogicoolDualActionGamePad : Gamepad
{
    static LogicoolDualActionGamePad()
    {
        //レイアウト新規登録用
        InputSystem.RegisterLayout<LogicoolDualActionGamePad>(
            matches: new InputDeviceMatcher()
                // HIDデバイスであることを指定
                .WithInterface("HID")
                // メーカー名を指定
                .WithManufacturer("Logicool")
                // 製品名を指定
                .WithProduct("Logicool Dual Action")
                // バージョンを指定
                .WithVersion("1394")
                // デバイスクラスを指定
                .WithDeviceClass("Gamepad")
                .WithCapability("vendorId", 0xF0D)
                .WithCapability("productId", 0xC1)         
        );

        //既存の機器のレイアウト変更用
        InputSystem.RegisterLayoutMatcher<LogicoolDualActionGamePad>(
            new InputDeviceMatcher()
                .WithInterface("HID")
                .WithManufacturer("Logicool")
                .WithProduct("Logicool Dual Action")
        );

    }

    //登録が走ったことを確認
    [RuntimeInitializeOnLoadMethod]
    static void Init() { 
         UnityEngine.Debug.Log("initController");
    }
}

//サイズの算出方法いまいちよくわかっていない
//今回は安直にProコンが20だったので20に設定
[StructLayout(LayoutKind.Explicit, Size = 20)]
public struct LogicoolDuualActionState : IInputStateTypeInfo
{
    //HIDデバイスであることを伝える？
    public FourCC format => new FourCC('H','I','D');

    //LeftStick
    [InputControl(name = "leftStick", format = "VC2S", layout = "Stick")]
    [InputControl(name = "leftStick/x", offset = 0, format = "USHT", parameters = "normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5")]
    [InputControl(name = "leftStick/left", offset = 0, format = "USHT", parameters = "normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5,clamp=1,clampMin=0.15,clampMax=0.5,invert")]
    [InputControl(name = "leftStick/right", offset = 0, format = "USHT", parameters = "normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5,clamp=1,clampMin=0.5,clampMax=0.85")]
    [InputControl(name = "leftStick/y", offset = 1, format = "USHT", parameters = "invert,normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5")]
    [InputControl(name = "leftStick/up", offset = 1, format = "USHT", parameters = "normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5,clamp=1,clampMin=0.15,clampMax=0.5,invert")]
    [InputControl(name = "leftStick/down", offset = 1, format = "USHT", parameters = "normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5,clamp=1,clampMin=0.5,clampMax=0.85,invert=false")]
    [FieldOffset(3)] public ushort leftStickX;
    [FieldOffset(4)] public ushort leftStickY;

    //RightStick
    [InputControl(name = "rightStick", format = "VC2S", layout = "Stick")]
    [InputControl(name = "rightStick/x", offset = 0, format = "USHT", parameters = "normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5")]
    [InputControl(name = "rightStick/left", offset = 0, format = "USHT", parameters = "normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5,clamp=1,clampMin=0,clampMax=0.5,invert")]
    [InputControl(name = "rightStick/right", offset = 0, format = "USHT", parameters = "normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5,clamp=1,clampMin=0.5,clampMax=1")]
    [InputControl(name = "rightStick/y", offset = 1, format = "USHT", parameters = "invert,normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5")]
    [InputControl(name = "rightStick/up", offset = 1, format = "USHT", parameters = "normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5,clamp=1,clampMin=0.15,clampMax=0.5,invert")]
    [InputControl(name = "rightStick/down", offset = 1, format = "USHT", parameters = "normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5,clamp=1,clampMin=0.5,clampMax=0.85,invert=false")]
    [FieldOffset(5)] public ushort rightStickX;
    [FieldOffset(6)] public ushort rightStickY;

    //Dpad
    [InputControl(name ="dpad", format ="BIT", layout = "Dpad", sizeInBits = 4,defaultState=8)]
    [InputControl(name ="dpad/up", format ="BIT", layout = "DiscreteButton", parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7", bit = 0, sizeInBits = 4)]
    [InputControl(name ="dpad/right", format ="BIT", layout = "DiscreteButton", parameters = "minValue=1,maxValue=3", bit = 0, sizeInBits = 4)]
    [InputControl(name ="dpad/down", format ="BIT", layout = "DiscreteButton", parameters = "minValue=3,maxValue=5", bit = 0, sizeInBits = 4)]
    [InputControl(name ="dpad/left", format ="BIT", layout = "DiscreteButton", parameters = "minValue=5,maxValue=7", bit = 0, sizeInBits = 4)]
    [FieldOffset(3)] public byte dpadButton;

    //Button1
    [InputControl(name = "buttonWest", displayName = "Y",bit = (uint)Button.West,usage = "SecondaryAction")]
    [InputControl(name = "buttonSouth", displayName = "B",bit = (uint)Button.South, usage = "Back")]
    [InputControl(name = "buttonEast", displayName = "A",bit = (uint)Button.East, usage = "PrimaryAction")]
    [InputControl(name = "buttonNorth", displayName = "X",bit = (uint)Button.North)]
    [InputControl(name = "leftShoulder", displayName = "L",bit = (uint)Button.L)]
    [InputControl(name = "rightShoulder", displayName = "R",bit = (uint)Button.R)]
    [InputControl(name = "leftTrigger", offset= 1,displayName = "ZL", format ="BIT", bit = (uint)Button.ZL)]
    [InputControl(name = "rightTrigger",offset= 1, displayName = "ZR", format ="BIT", bit = (uint)Button.ZR)]
    [FieldOffset(1)] public uint button1;

    //Button2
    [InputControl(name = "select", displayName = "Minus",bit = (uint)Button.Minus)]
    [InputControl(name = "start", displayName = "Plus",bit = (uint)Button.Plus)]
    [InputControl(name = "leftStickPress", displayName = "Left Stick",bit = (uint)Button.StickL)]
    [InputControl(name = "rightStickPress", displayName = "Right Stick",bit = (uint)Button.StickR)]
    [FieldOffset(2)] public uint button2;

    
    //Buttonの対応BIＴをenumで保存
    public enum Button
    {
        West = 0,
        South = 1,
        East = 2,
        North = 3,
        L = 4,
        R = 5,
        ZL =6,
        ZR = 7,

        Plus = 0,
        Minus = 1,
        StickL = 2,
        StickR = 3,
    }

    //動作の閾値を設定
    public LogicoolDuualActionState WithButton(Button button,bool value = true)
    {
        var bit = (uint)1 << (int)button;
        if (value)
            button1 |= bit;
        else
            button1 &= ~bit;
        // dpad default state
        button1 |= 8 << 24;
        leftStickX = 0x8000;
        leftStickY = 0x8000;
        rightStickX = 0x8000;
        rightStickY = 0x8000;
        return this;
    }
}