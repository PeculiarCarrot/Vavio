fireIndex = {}
fireTimes = {}
initialized = {}
anglePer = {}
shots = 6

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(9.3333333, 0)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		for i = 0, 90 - 1, (90/shots) do
				bullet = pattern.NewBullet()
				bullet.speed = 3 + pattern.Math().RandomRange(0, 1)
				bullet.type = "circle"
				bullet.material = "white"
				bullet.scale = 1
				bullet.angle = i + -90
				pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
end