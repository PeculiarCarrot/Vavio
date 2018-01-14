fireIndex = {}
fireTimes = {}
spin = {}
angle = {}
initialized = {}
shots = 10

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(2.3333333, .6)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		angle[id] = pattern.Math().RandomRange(0, 360)

		for i = angle[id], angle[id] + 360, (360/shots) do
			bullet = pattern.NewBullet()
			bullet.speed = 3
			bullet.type = "circle"
			bullet.material = "darkRed"
			bullet.angle = i
			bullet.scale = .7
			pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	angle[id] = 0
	spin[id] = 0
end