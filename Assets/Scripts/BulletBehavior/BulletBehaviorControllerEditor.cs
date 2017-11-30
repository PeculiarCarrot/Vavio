using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BulletBehaviorController))]
public class BulletBehaviorControllerEditor : Editor {

	private bool settingPoint;

	 public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        /*if(GUILayout.Button(settingPoint ? "Cancel setting spawn point" : "Set spawn point"))
        {
           settingPoint = !settingPoint;
        }*/
    }

	void OnSceneGUI()
	{
		/*Event e = Event.current;
		if(settingPoint)
		{
			BulletBehaviorController bbc = (BulletBehaviorController)target;
			Selection.activeGameObject = bbc.gameObject;
				Vector2 mouse = GUIUtility.GUIToScreenPoint(e.mousePosition);
				 Vector2 mousePos = Event.current.mousePosition;
				 mousePos.y = -SceneView.lastActiveSceneView.camera.pixelHeight * 0 + mousePos.y;
				Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);
				mouse = new Vector2(ray.origin.x, ray.origin.y);

				bbc.SetPos(mouse.x, mouse.y);
			if (e.type == EventType.MouseDown && e.button == 0)
			{
				settingPoint = false;
			}
		}*/
	}

}
