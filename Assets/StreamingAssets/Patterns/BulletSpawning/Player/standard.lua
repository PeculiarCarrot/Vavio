fireIndex = {}
fireTimes = {}
initialized = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(10, 0)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1

		bullet = pattern.NewBullet()
		bullet.speed = 15
		bullet.damage = 20
		bullet.material = "green"
		pattern.SpawnBullet(bullet)
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
end