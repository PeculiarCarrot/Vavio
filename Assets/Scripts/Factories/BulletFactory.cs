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
		bullet.GetComponent<MovementController>().patternPath = b.movement;
		bullet.GetComponent<MovementController>().speed = b.speed;
		bullet.GetComponent<MovementController>().speedMultiplier = b.speedMultiplier;
		bullet.GetComponent<MovementController>().synced = b.synced;
		bullet.GetComponent<PatternController>().patternPath = b.pattern;
		bullet.transform.position = shooter.position + new Vector3(b.x, b.y, b.z);
		bullet.transform.rotation = Quaternion.Euler(shooter.eulerAngles.x, shooter.eulerAngles.y, shooter.eulerAngles.z + b.angle);
		bullet.transform.localScale = bullet.transform.localScale * b.scale;

		foreach (Renderer renderer in bullet.GetComponentsInChildren<Renderer>())
		{
			renderer.material = material;
		}

		BulletProperties bp = bullet.GetComponent<BulletProperties>();
		bp.destroyOnExitStage = b.destroyOnExitStage;
		bp.destroyOnHit = b.destroyOnHit;
		bp.owner = b.owner;
		bp.lifetime = b.lifetime;

		bp.Init();
		return bullet;
	}

	private static GameObject GetUnused(string t)
	{
		foreach(GameObject b in objects)
			if(!b.activeInHierarchy && b.name == t+"(Clone)")
			{
				b.SetActive(true);
				b.GetComponent<BulletProperties>().Reset();
				return b;
			}
		GameObject o = Object.Instantiate(PatternController.GetBulletModel(t));
		objects.Add(o);
		return o;
	}
	
}
