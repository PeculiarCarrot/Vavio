fireIndex = {}
fireTimes = {}
initialized = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(13, 0)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1

		bullet = pattern.NewBullet()
		bullet.speed = 6
		bullet.scale = 2
		bullet.material = "white"
		pattern.SpawnBullet(bullet)
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
end