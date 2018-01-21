fireIndex = {}
fireTimes = {}
fireIndex2 = {}
fireTimes2 = {}
initialized = {}
lastTime = {}
angle = {}
angle2 = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(8.5333333, .4, 1.875, 1.875)
		fireTimes2[id] = pattern.GetFireTimes(4.2666666, .4)
	end

	a = angle[id]
	angle2[id] = angle2[id] + 45 * deltaTime
	if(pattern.GetStageTime() >= fireTimes2[id][fireIndex2[id]]) then
		fireIndex2[id] = fireIndex2[id] + 1
		bullet = pattern.NewBullet()
		bullet.speed = 3
		bullet.angle = angle2[id]
		pattern.SpawnBullet(bullet)

		bullet = pattern.NewBullet()
		bullet.speed = 3
		bullet.angle = angle2[id] + 180
		pattern.SpawnBullet(bullet)
	end
	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		if pattern.GetStageTime() - lastTime[id] > 1 then
			angle[id] = pattern.GetAngleToPlayer()
			a = angle[id]
		end

		lastTime[id] = pattern.GetStageTime()

		bullet = pattern.NewBullet()
		bullet.speed = 7
		bullet.angle = a
		pattern.SpawnBullet(bullet)
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	angle[id] = 0
	fireIndex2[id] = 0
	angle2[id] = 0
	lastTime[id] = -10
end