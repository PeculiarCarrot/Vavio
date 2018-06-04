using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BulletFactory {

	private static List<GameObject> objects = new List<GameObject>();

	//Puts everything in the pool to sleep
	public static void SleepAll()
	{
		List<GameObject> toRemove = new List<GameObject>();
		foreach (GameObject b in objects)
		{
			if (b == null)
			{
				toRemove.Add(b);
				continue;
			}
			if (b.activeInHierarchy)
			{
				b.SetActive(false);
				b.GetComponent<BulletProperties>().Reset();
			}
		}
		foreach (GameObject oo in toRemove)
			objects.Remove(oo);
	}

	public static GameObject Create(Transform shooter, PatternController.BulletData b)
	{
		//Grab a bullet from the pool that has the same model (this could be optimized by allowing bullets to change models, but this works)
		GameObject model = PatternController.GetBulletModel(b.type);
		Material material = PatternController.GetBulletMaterial(b.material);

		//Spawn the bullet and apply all properties to it
		GameObject bullet = GetUnused(b.type);
		MovementController mc = bullet.GetComponent<MovementController>();
		mc.patternPath = b.movement;
		mc.speed = b.speed;
		mc.turn = b.turn;
		mc.speedMultiplier = b.speedMultiplier;
		mc.synced = b.synced;
		bullet.GetComponent<PatternController>().patternPath = b.pattern;
		bullet.transform.position = shooter.position + new Vector3(b.x, b.y, 0);
		bullet.transform.rotation = Quaternion.Euler(shooter.eulerAngles.x, shooter.eulerAngles.y, shooter.eulerAngles.z + b.angle);
		bullet.transform.localScale = bullet.transform.localScale * b.scale;

		//If the z is different, we don't move the actual bullet, just the renderer, so things can still collide
		foreach (Renderer renderer in bullet.GetComponentsInChildren<Renderer>())
		{
			renderer.material = material;
			Vector3 v = bullet.transform.position;
			v.z = b.z;
			renderer.gameObject.transform.position = v;
		}

		//Set bullet properties
		BulletProperties bp = bullet.GetComponent<BulletProperties>();
		bp.destroyOnExitStage = b.destroyOnExitStage;
		bp.destroyOnHit = b.destroyOnHit;
		bp.owner = b.owner;
		bp.lifetime = b.lifetime;
		bp.damage = b.damage;
		GameObject.DontDestroyOnLoad(bullet);

		bp.Init();
		return bullet;
	}

	public static void ClearPool()
	{
		objects.Clear();
	}

	//Returns an unused bullet model of the type we're looking for
	private static GameObject GetUnused(string t)
	{
		List<GameObject> toRemove = new List<GameObject>();
		foreach (GameObject b in objects)
		{
			if (b == null)
			{
				toRemove.Add(b);
				continue;
			}
			if (!b.activeInHierarchy && b.name == t + "(Clone)")
			{
				b.SetActive(true);
				b.GetComponent<BulletProperties>().Reset();
				return b;
			}
		}
		foreach (GameObject oo in toRemove)
			objects.Remove(oo);
		GameObject o = Object.Instantiate(PatternController.GetBulletModel(t));
		objects.Add(o);
		return o;
	}
	
}
