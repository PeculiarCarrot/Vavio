fireIndex = {}
fireTimes = {}
initialized = {}
lastTime = {}
angle = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(8.5333333, .4, 1.875, 1.875)
	end

	a = angle[id]
	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		if pattern.GetStageTime() - lastTime[id] > 1 then
			angle[id] = pattern.GetAngleToPlayer()
			a = angle[id]
		end

		lastTime[id] = pattern.GetStageTime()

		bullet = pattern.NewBullet()
		bullet.speed = 9
		bullet.angle = a
		pattern.SpawnBullet(bullet)
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	lastTime[id] = -10
	angle[id] = 0
end