using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior {

	public struct EnemyBehaviorStep
	{
		public float time;
		public Vector3 speed;
		public float friction, accel;
	}

	private EnemyBase enemy;
	private EnemyBehaviorStep[] steps = new EnemyBehaviorStep[0];
	private int currentStep;
	private float currentStepTime = 0;

	public static EnemyBehavior FromJSON(string json)
	{
		JSONObject o = new JSONObject(json);
		EnemyBehavior e = new EnemyBehavior();

		for(int i = 0; i < o.list.Count; i++)
		{
			string key = (string)o.keys[i];
			JSONObject no = (JSONObject)o.list[i];
			if(key == "steps")
			{
				e.steps = new EnemyBehaviorStep[no.list.Count];
				for(int j = 0; j < no.list.Count; j++)
				{
					JSONObject step = (JSONObject)no.list[j];
					for(int k = 0; k < step.list.Count; k++)
					{
						string stepKey = (string)step.keys[k];
						JSONObject stepValue = (JSONObject)step.list[k];
						switch(stepKey)
						{
							case "time":
								e.steps[j].time = (float)stepValue.n;
								break;
							case "x":
								e.steps[j].speed.x = (float)stepValue.n;
								break;
							case "y":
								e.steps[j].speed.y = (float)stepValue.n;
								break;
							case "z":
								e.steps[j].speed.z = (float)stepValue.n;
								break;
							case "friction":
								e.steps[j].friction = (float)stepValue.n;
								break;
							case "accel":
								e.steps[j].accel = (float)stepValue.n;
								break;
						}
					}
				}
			}
		}

		return e;
	}

	public void SetEnemy(EnemyBase e)
	{
		enemy = e;
	}

	public void Update () {
		if(enemy.CanDoBehavior())
		{
			//Debug.Log(Stage.deltaTime + " - "+Time.deltaTime);
			if(steps.Length > 0)
			{
				currentStepTime += Stage.deltaTime;
				if(currentStepTime >= steps[currentStep].time)
				{
					currentStepTime -= steps[currentStep].time;
					if(currentStep < steps.Length - 1)
						currentStep++;
					else
						currentStep = 0;
				}

				ApplyCurrentStep();
			}
		}
	}

	private void ApplyCurrentStep()
	{
			enemy.accel = steps[currentStep].accel;
			enemy.friction = steps[currentStep].friction;
			enemy.velocity = steps[currentStep].speed * enemy.accel;
	}
}
