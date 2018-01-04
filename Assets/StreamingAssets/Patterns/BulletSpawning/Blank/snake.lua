fireIndex = {}
fireTimes = {}
spin = {}
angle = {}
time = {}
dir = {}
initialized = {}
maxAngle = 90
spd = 1.5

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(9.333333, .4)
	end
	time[id] = time[id] + deltaTime

	if(pattern.Math().Abs(pattern.GetAngle() - 270) > 1) then
		angle[id] = -maxAngle * pattern.Math().Sin(time[id] * spd)
		angle[id] = angle[id] + 180
	else
		angle[id] = maxAngle * pattern.Math().Sin(time[id] * spd)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		bullet = pattern.NewBullet()
		bullet.speed = 4
		bullet.type = "capsule"
		bullet.material = "red"
		if(pattern.Math().Abs(pattern.GetAngle() - 270) > 1) then
			bullet.material = "orange"
			bullet.z = 1
		end
		bullet.angle = bullet.angle + angle[id]
		bullet.scale = 1
		bullet.speedMultiplier = 1
		pattern.SpawnBullet(bullet)
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	angle[id] = 0
	spin[id] = 40
	time[id] = 0
	dir = 1
end