fireIndex = {}
initialized = {}
fireTimes = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(4.666666, 1)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1

		bullet = pattern.NewBullet()
		bullet.movement = "General/basic"
		bullet.speed = 3
		pattern.SpawnBullet(bullet)

		bullet = pattern.NewBullet()
		bullet.movement = "General/basic"
		bullet.speed = 3
		bullet.angle = 180
		pattern.SpawnBullet(bullet)
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
end