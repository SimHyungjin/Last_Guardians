#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimationEventAdder : MonoBehaviour
{
    [MenuItem("Tools/Animation/Add Animation Events To Controller")]
    private static void AddEventsToClips()
    {
        var selected = Selection.activeObject as RuntimeAnimatorController;
        if (selected == null)
        {
            return;
        }

        foreach (var clip in selected.animationClips)
        {
            var existingEvents = AnimationUtility.GetAnimationEvents(clip);
            if (existingEvents.Length > 0)
            {
                continue;
            }

            if (clip.name == "ATTACK")
            {
                var newEvent = new AnimationEvent
                {
                    time = 0.25f,
                    functionName = "Attack"
                };

                AnimationUtility.SetAnimationEvents(clip, new[] { newEvent });
                Debug.Log($"[ATTACK] 이벤트 추가 완료.");
            }
            else if (clip.name == "DEATH")
            {
                var newEvent = new AnimationEvent
                {
                    time = 0.4f,
                    functionName = "Death"
                };

                AnimationUtility.SetAnimationEvents(clip, new[] { newEvent });
                Debug.Log($"[DEATH] 이벤트 추가 완료.");
            }
        }

        AssetDatabase.SaveAssets();
    }
}
#endif