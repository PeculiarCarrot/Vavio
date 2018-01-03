timer = {}
initialized = {}
time = .2

function update(pattern, id, deltaTime)
	
	timer[id] = timer[id] - pattern.GetRealDeltaTime()

	if(timer[id] <= 0) then
		timer[id] = time
		bullet = pattern.NewBullet()
		bullet.speed = 20
		bullet.damage = 12 * 4
		bullet.type = "playerBullet"
		bullet.material = "green"
		bullet.owner = "player"
		bullet.angle = -90
		bullet.y = .5
		bullet.z = 1
		pattern.SpawnBullet(bullet)
	end
end

function init(pattern, id)
	timer[id] = time
end