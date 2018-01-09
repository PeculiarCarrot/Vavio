fireIndex = {}
fireTimes = {}
initialized = {}
angle = {}
numBullets = 1

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(8.533333, .4)
	end

	angle[id] = angle[id] + 200 * deltaTime

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
			bullet = pattern.NewBullet()
			bullet.speed = 1
			bullet.type = "circle"
			bullet.scale = .8
			bullet.material = "orange"
			bullet.movement = "Skyline/sine"
			--bullet.speedMultiplier = .998
			bullet.angle = angle[id]
			pattern.SpawnBullet(bullet)
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	angle[id] = 0
end