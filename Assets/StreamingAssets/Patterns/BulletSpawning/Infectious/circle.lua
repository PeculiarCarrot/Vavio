fireIndex = {}
fireTimes = {}
spin = {}
angle = {}
initialized = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(2.133333333, .4)
	end

	angle[id] = angle[id] + spin[id] * deltaTime

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1

		for i = angle[id], angle[id] + 360, (360/15) do
			bullet = pattern.NewBullet()
			bullet.speed = 2
			bullet.type = "circle"
			bullet.material = "darkPurple"
			bullet.angle = i
			bullet.scale = 1.4
			bullet.z = 1
			bullet.speedMultiplier = .999
			pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	angle[id] = 0
	spin[id] = 14
end