fireIndex = {}
fireTimes = {}
initialized = {}
anglePer = {}
shots = 7

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(9.3333333, .05)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		for i = 0, 360, (360/shots) do
				bullet = pattern.NewBullet()
				bullet.speed = 3 + pattern.Math().RandomRange(0, 1)
				bullet.type = "circle"
				bullet.material = "orange"
				bullet.scale = .75
				bullet.angle = i + pattern.Math().RandomRange(-20, 20)
				pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
end