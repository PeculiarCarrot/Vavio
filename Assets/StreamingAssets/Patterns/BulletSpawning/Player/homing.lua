timer = {}
initialized = {}
time = .2

function update(pattern, id, deltaTime)
	
	timer[id] = timer[id] - 1 / 60

	if(timer[id] <= 0) then
		timer[id] = time
		bullet = pattern.NewBullet()
		bullet.speed = 20
		bullet.damage = 12 * 4
		bullet.type = "playerBullet"
		bullet.movement = "Player/homing"
		bullet.material = "aqua"
		bullet.owner = "player"
		bullet.scale = .5
		bullet.angle = -90
		bullet.y = .5
		bullet.z = 1
		pattern.SpawnBullet(bullet)
	end
end

function init(pattern, id)
	timer[id] = time
end