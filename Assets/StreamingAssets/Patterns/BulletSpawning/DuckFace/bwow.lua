fireIndex = {}
fireTimes = {}
spin = {}
angle = {}
initialized = {}
shots = 8

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(2.33333, 1)
	end

	angle[id] = angle[id] + spin[id] * deltaTime

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1

		for i = angle[id], angle[id] + 360, (360/shots) do
			bullet = pattern.NewBullet()
			bullet.speed = 2
			bullet.type = "capsule"
			bullet.material = "red"
			bullet.angle = i
			bullet.scale = 1
			bullet.speedMultiplier = 1
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