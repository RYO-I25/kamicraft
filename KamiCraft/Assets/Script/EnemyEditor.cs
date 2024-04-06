#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(EnemyStatus))]
public class EnemyEditor : Editor
{
    private EnemyStatus _target;

    private void Awake()
    {
        _target = target as EnemyStatus;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.HelpBox("EnemyControllerと共にアタッチしてください", MessageType.Info, true);
        _target.enemyType = (EnemyStatus.EnemyType)EditorGUILayout.EnumPopup("EnemyType", _target.enemyType);
        

        switch (_target.enemyType)
        {
            case EnemyStatus.EnemyType.Stay:
                {

                }
                break;
            case EnemyStatus.EnemyType.Round:
                {
                    _target.roundSetteing.speed = Mathf.Max(0.01f, EditorGUILayout.FloatField("移動速度", _target.roundSetteing.speed));
                    _target.roundSetteing.roundPos = EditorGUILayout.Vector3Field("目標値", _target.roundSetteing.roundPos);
                    _target.roundSetteing.posOffset = EditorGUILayout.Vector3Field("移動地点のオフセット", _target.roundSetteing.posOffset);
                }
                break;
            case EnemyStatus.EnemyType.Follow:
                {
                    _target.followSetteing.speed = Mathf.Max(0.01f, EditorGUILayout.FloatField("移動速度", _target.followSetteing.speed));
                    EditorGUILayout.HelpBox("プレイヤーを発見する範囲はEnemyの子供PlayerSearchのコリジョンを設定してください", MessageType.Info, true);
                }
                break;
            case EnemyStatus.EnemyType.Jump:
                {
                    _target.jumpSetteing.speed = EditorGUILayout.FloatField("移動速度", _target.jumpSetteing.speed);
                    _target.jumpSetteing.jumpPower = Mathf.Max(0.1f, EditorGUILayout.FloatField("ジャンプ力", _target.jumpSetteing.jumpPower));
                }
                break;
            case EnemyStatus.EnemyType.Roll:
                {
                    _target.rollSetteing.rollSpeed = EditorGUILayout.FloatField("回転速度", _target.rollSetteing.rollSpeed);
                    _target.rollSetteing.AtaackTime = Mathf.Max(0.1f, EditorGUILayout.FloatField("攻撃時間", _target.rollSetteing.AtaackTime));
                    _target.rollSetteing.StayTime = Mathf.Max(0.1f, EditorGUILayout.FloatField("静止時間", _target.rollSetteing.StayTime));
                }
                break;
        }

        _target.bulletAttack = EditorGUILayout.ToggleLeft("弾丸攻撃する", _target.bulletAttack);
        if (_target.bulletAttack)
        {
            _target.bulletSetteing.speed = Mathf.Max(0.01f, EditorGUILayout.FloatField("射出速度", _target.bulletSetteing.speed));
            _target.bulletSetteing.interval = Mathf.Max(0.01f, EditorGUILayout.FloatField("次の射出までの時間", _target.bulletSetteing.interval));
            EditorGUILayout.HelpBox("射出位置はEnemy内のMuzzleの座標を操作してください", MessageType.Info, true);
        }


        // GUIの更新があったら実行
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(_target);
        }
    }
}
#endif
