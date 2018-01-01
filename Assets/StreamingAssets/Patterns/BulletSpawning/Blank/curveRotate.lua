fireIndex = {}
fireTimes = {}
initialized = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(9.333333, .4)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		bullet = pattern.NewBullet()
		bullet.turn = -20
		bullet.speed = 3;
		bullet.material = "orange"
		bullet.type = "circle"
		bullet.scale = .5
		bullet.angle = -90
		bullet.speedMultiplier = 1.00
		pattern.SpawnBullet(bullet)
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
end