///
/// スキルを管理するコンポーネント。
///
using System;
using UnityEngine;

public class SkillManagerComponent : MonoBehaviour
{
    private static readonly float REQUIRED_SKILL_POINT = 60.0f; // スキル発動に必要なポイント。
    private static readonly float INVINCIBLE_DURATION = 30.0f;  // 無敵スキルの持続時間。

    private float currentSkillPoint_ = 0.0f;                    // 現在のスキルポイント。
    private int myRank_ = 10;                                   // 自分の順位。
    private Transform UiCanvas_;                                // スキルのUIをまとめたキャンバス。

    private GameObject[] otherPlayerUnits_;                     // 他のプレイヤーユニット。
    private TetriminoSpawnerComponent tetriminoSpawner_;        // 子のスポナーを取得。
    private TetriminoControllerComponent skillTargetTetrimino_; // 操作中のテトリミノを格納する変数。

    private bool isInvincibleSkill_ = false;                    // 無敵スキルが有効かどうかをミノが参照できるようにする。
    private float invincibleTimer_ = 0.0f;                      // 無敵スキルの持続時間。

    void Start()
    {
        // 子のUI用Canvasを探し、UiCanvas_に格納する。
        UiCanvas_ = transform.Find("Character/UiCanvas");

        // UIは非表示にしておく。
        UiCanvas_.gameObject.SetActive(false);
    }

    void Update()
    {
        // スキルポイントを蓄積。
        currentSkillPoint_ += myRank_ * Time.deltaTime;

        // スキルポイントが貯まったらスキルをスキルを有効化。
        if (currentSkillPoint_ >= REQUIRED_SKILL_POINT)
        {
            ActiveSkill();
        }

        // 無敵スキルの持続時間を管理。
        DeactivateInvincibleSkill();
    }

    /// <summary>
    /// 他のプレイヤーユニットを格納する変数。
    /// </summary>
    /// <param name="units">ゲームマネージャーで取得した他のプレイヤーユニット。</param>
    public void SetOtherPlayerUnits(GameObject[] units)
    {
        otherPlayerUnits_ = units;
    }


    /// <summary>
    /// スキルのUI表示と発動の処理。
    /// </summary>
    private void ActiveSkill()
    {
        // UIを表示する。
        UiCanvas_.gameObject.SetActive(true);

        // 特定のキー入力で特定のスキルを発動する。
        UseSkill(Input.GetKeyDown(KeyCode.Keypad2), CallBindSkill);
        UseSkill(Input.GetKeyDown(KeyCode.Keypad6), InvincibleSkill);
        UseSkill(Input.GetKeyDown(KeyCode.Keypad4), CallScaleUpSkillOnOthers);
        UseSkill(Input.GetKeyDown(KeyCode.Keypad8), CallLockRotationSkillOnOthers);
    }

    /// <summary>
    /// 特定のキーを入力したら特定のスキルを発動する処理。
    /// </summary>
    /// <param name="getKeyDown">入力するキーを設定。</param>
    /// <param name="skill">発動するスキルの関数を設定。</param>
    private void UseSkill(bool getKeyDown, Action skill)
    {
        if (getKeyDown)
        {
            // 引数の関数を処理する。
            skill?.Invoke();

            // ポイントを初期化。
            currentSkillPoint_ = 0.0f;

            // UIを非表示にする。
            UiCanvas_.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 自身が今操作しているテトリミノが他のテトリミノと接触したとき結合させる処理を呼び出す。
    /// </summary>
    private void CallBindSkill()
    {
        GetControlledTetrimino();
        skillTargetTetrimino_.SetIsActiveBindSkill();
        skillTargetTetrimino_.transform.position = tetriminoSpawner_.transform.position;
    }

    /// <summary>
    /// 自身が無敵になる処理を呼び出す。
    /// </summary>
    private void InvincibleSkill()
    {
        isInvincibleSkill_ = true;
        //GetControlledTetrimino();
        //skillTargetTetrimino_.SetIsActiveBindSkill();
        //skillTargetTetrimino_.transform.position = tetriminoSpawner_.transform.position;
    }

    /// <summary>
    /// 他プレイヤーが操作しているテトリミノを巨大化する処理を呼び出す。
    /// </summary>
    private void CallScaleUpSkillOnOthers()
    {
        ForEachOtherPlayerTetrimino(tetrimino => tetrimino.ScaleUpSkill());
    }

    /// <summary>
    /// 他プレイヤーが操作しているテトリミノを回転不可にする処理を呼び出す。
    /// </summary>
    private void CallLockRotationSkillOnOthers()
    {
        ForEachOtherPlayerTetrimino(tetrimino => tetrimino.LockRotationSkill());
    }

    /// <summary>
    /// 操作中のテトリミノを取得する。
    /// </summary>
    private void GetControlledTetrimino()
    {
        // PlayerUnitの子オブジェクトからTetriminoSpawnerComponentを探す。
        tetriminoSpawner_ = GetComponentInChildren<TetriminoSpawnerComponent>();

        // 操作中のテトリミノを取得する。
        skillTargetTetrimino_ = tetriminoSpawner_.controlledTetrimino_;

        if (skillTargetTetrimino_ == null)
        {
            Debug.LogWarning("TetriminoSpawnerComponent が見つかりませんでした。");
        }
    }

    /// <summary>
    /// 他のプレイヤーが操作しているテトリミノに対して、指定したスキルを実行する。
    /// </summary>
    /// <param name="skill"> 実行するスキル。</param>
    private void ForEachOtherPlayerTetrimino(Action<TetriminoControllerComponent> skill)
    {
        foreach (var unit in otherPlayerUnits_)
        {
            // 他のプレイヤーユニットのSkillManagerComponentを取得。
            var otherSkillManager = unit.GetComponentInChildren<SkillManagerComponent>();
            if (otherSkillManager == null)
            {
                Debug.LogWarning("SkillManagerComponent が見つかりませんでした。");
                continue;
            }

            // 他のプレイヤーが無敵スキルを使っていたらスキルを発動しない。
            if (otherSkillManager.isInvincibleSkill_)
            {
                continue;
            }

            // 他のプレイヤーの操作中のテトリミノを取得。
            otherSkillManager.GetControlledTetrimino();
            var tetrimino = otherSkillManager.skillTargetTetrimino_;
            if (tetrimino == null)
            {
                Debug.LogWarning("skillTargetTetrimino_ が null です。");
                continue;
            }

            skill?.Invoke(tetrimino);
        }
    }

    /// <summary>
    /// 一定時間が経ったら、無敵を解除する。
    /// </summary>
    private void DeactivateInvincibleSkill()
    {
        if (isInvincibleSkill_)
        {
            invincibleTimer_ += Time.deltaTime;
            if (invincibleTimer_ >= INVINCIBLE_DURATION)
            {
                invincibleTimer_ = 0.0f;
                isInvincibleSkill_ = false;
            }
        }
    }
}