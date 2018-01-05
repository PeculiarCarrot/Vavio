timer = {}
initialized = {}
time = .06

function update(pattern, id, deltaTime)
	
	timer[id] = timer[id] - deltaTime

	if(timer[id] <= 0) then
		timer[id] = time
		bullet = pattern.NewBullet()
		bullet.speed = 30
		bullet.damage = 12 * 2
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