fireIndex = {}
fireTimes = {}
spin = {}
angle = {}
dir = {}
initialized = {}
maxAngle = 90

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(17.06666, 0)
	end

	angle[id] = angle[id] + spin[id] * deltaTime * dir

	if(angle[id] < -maxAngle or angle[id] > maxAngle) then
		dir = -dir
		if(angle[id] < -maxAngle) then
			angle[id] = -maxAngle
		end
		if(angle[id] > maxAngle) then
			angle[id] = maxAngle
		end
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		bullet = pattern.NewBullet()
		bullet.speed = 4
		bullet.type = "capsule"
		bullet.material = "lightRed"
		bullet.angle = bullet.angle + angle[id]
		bullet.scale = .75
		bullet.speedMultiplier = 1
		pattern.SpawnBullet(bullet)
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	angle[id] = 0
	spin[id] = 120
	dir = 1
end