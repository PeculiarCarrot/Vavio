fireIndex = {}
fireTimes = {}
initialized = {}
angle = {}
numBullets = 4

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(10.1333333, .8)
	end

	angle[id] = angle[id] + 100 * deltaTime

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		for i=0, numBullets-1, 1 do
			bullet = pattern.NewBullet()
			bullet.speed = 3 + i * .25
			bullet.type = "capsule"
			bullet.scale = .8
			bullet.material = "purple"
			--bullet.speedMultiplier = .998
			bullet.angle = angle[id]
			pattern.SpawnBullet(bullet)

			bullet = pattern.NewBullet()
			bullet.speed = 3 + i * .25
			bullet.type = "capsule"
			bullet.scale = .8
			bullet.material = "purple"
			--bullet.speedMultiplier = .998
			bullet.angle = angle[id] - 180
			pattern.SpawnBullet(bullet)

			bullet = pattern.NewBullet()
			bullet.speed = 3 + i * .25
			bullet.type = "capsule"
			bullet.scale = .8
			bullet.material = "aqua"
			--bullet.speedMultiplier = .998
			bullet.angle = angle[id] - 90
			pattern.SpawnBullet(bullet)

			bullet = pattern.NewBullet()
			bullet.speed = 3 + i * .25
			bullet.type = "capsule"
			bullet.scale = .8
			bullet.material = "aqua"
			--bullet.speedMultiplier = .998
			bullet.angle = angle[id] - 270
			pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	angle[id] = 0
end