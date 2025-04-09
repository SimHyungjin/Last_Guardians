using UnityEngine;

/// <summary>
/// 터치 입력 대신 테스트를 위해 키보드 입력(WASD)으로 캐릭터를 이동시키는 임시 스크립트입니다.
/// 귀찮아서 Gpt한테 써달라 한거니 전 모릅니다.
/// </summary>
public class KeyboardMoveTest : MonoBehaviour
{
    private void Update()
    {
        Vector2 inputDir = new Vector2(
            Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0,
            Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0
        );

        if (inputDir.sqrMagnitude > 0.01f)
        {
            inputDir = inputDir.normalized;

            // 이동
            transform.position += (Vector3)(inputDir * InGameManager.Instance.playerManager.player.playerData.moveSpeed * Time.deltaTime);

            // 회전 (방향 쪽으로만 보정)
            float targetAngle = Mathf.Atan2(inputDir.y, inputDir.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
