
initialized = {}
shots = 40

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
end

function init(pattern, id)
	initialized[id] = true
	local a = pattern.Math().RandomRange(0, 360)
		for i = 0, 360, (360/shots) do
			bullet = pattern.NewBullet()
			bullet.speed = .2 + pattern.Math().RandomValue() * .3
			bullet.type = "capsule"
			bullet.material = "red"
			bullet.angle = i + a
			bullet.scale = 1
			bullet.y = .25
			bullet.speedMultiplier = 1.000001 + pattern.Math().RandomValue() * .00001
			pattern.SpawnBullet(bullet)
		end
		a = pattern.Math().RandomRange(0, 360)
		for i = 0, 360, (360/shots) do
			bullet = pattern.NewBullet()
			bullet.speed = .2 + pattern.Math().RandomValue() * .3
			bullet.type = "capsule"
			bullet.material = "red"
			bullet.angle = i + a
			bullet.scale = 1
			bullet.y = .25
			bullet.speedMultiplier = 1.000001 + pattern.Math().RandomValue() * .00001
			pattern.SpawnBullet(bullet)
		end
		a = pattern.Math().RandomRange(0, 360)
		for i = 0, 360, (360/shots) do
			bullet = pattern.NewBullet()
			bullet.speed = .2 + pattern.Math().RandomValue() * .3
			bullet.type = "capsule"
			bullet.material = "red"
			bullet.angle = i + a
			bullet.scale = 1
			bullet.y = .25
			bullet.speedMultiplier = 1.000001 + pattern.Math().RandomValue() * .00001
			pattern.SpawnBullet(bullet)
		end
end