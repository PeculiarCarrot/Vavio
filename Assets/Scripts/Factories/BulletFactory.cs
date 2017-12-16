using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BulletFactory {

	private static List<GameObject> objects = new List<GameObject>();

	public static GameObject Create(Transform shooter, PatternController.BulletData b)
	{
		GameObject model = PatternController.GetBulletModel(b.type);
		Material material = PatternController.GetBulletMaterial(b.material);

		//Spawn the bullet and apply all properties to it
		GameObject bullet = GetUnused(b.type);
		MovementController mc = bullet.GetComponent<MovementController>();
		mc.patternPath = b.movement;
		mc.speed = b.speed;
		mc.speedMultiplier = b.speedMultiplier;
		mc.synced = b.synced;
		bullet.GetComponent<PatternController>().patternPath = b.pattern;
		bullet.transform.position = shooter.position + new Vector3(b.x, b.y, 0);
		bullet.transform.rotation = Quaternion.Euler(shooter.eulerAngles.x, shooter.eulerAngles.y, shooter.eulerAngles.z + b.angle);
		bullet.transform.localScale = bullet.transform.localScale * b.scale;

		foreach (Renderer renderer in bullet.GetComponentsInChildren<Renderer>())
		{
			renderer.material = material;
			Vector3 v = bullet.transform.position;
			v.z = b.z;
			renderer.gameObject.transform.position = v;
		}

		BulletProperties bp = bullet.GetComponent<BulletProperties>();
		bp.destroyOnExitStage = b.destroyOnExitStage;
		bp.destroyOnHit = b.destroyOnHit;
		bp.owner = b.owner;
		bp.lifetime = b.lifetime;
		bp.damage = b.damage;

		bp.Init();
		return bullet;
	}

	public static void ClearPool()
	{
		objects.Clear();
	}

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
