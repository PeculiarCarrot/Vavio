fireIndex = {}
fireTimes = {}
spin = {}
angle = {}
initialized = {}
shots = 60

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(2.333333, 0)
	end

	angle[id] = pattern.Math().RandomRange(0, 360)

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1

		for i = angle[id], angle[id] + 360, (360/shots) do
			bullet = pattern.NewBullet()
			bullet.speed = 2.7
			bullet.type = "circle"
			bullet.material = "orange"
			bullet.angle = i
			bullet.scale = .5
			bullet.speedMultiplier = 1
			pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	angle[id] = 0
	spin[id] = 100
end