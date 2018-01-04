fireIndex = {}
fireTimes = {}
initialized = {}
turns = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(9.333333, .4)
	end

	turns[id] = turns[id] + 10 * deltaTime

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		bullet = pattern.NewBullet()
		bullet.turn = turns[id]
		bullet.speed = 3;
		bullet.lifetime = 5
		bullet.material = "orange"
		bullet.type = "circle"
		bullet.scale = .5
		bullet.angle = -90
		--bullet.speedMultiplier = .1
		pattern.SpawnBullet(bullet)
		bullet = pattern.NewBullet()
		bullet.turn = -turns[id]
		bullet.speed = 3;
		bullet.lifetime = 5
		bullet.material = "orange"
		bullet.type = "circle"
		bullet.scale = .5
		bullet.angle = 90
		--bullet.speedMultiplier = .1
		pattern.SpawnBullet(bullet)
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	turns[id] = 50
end