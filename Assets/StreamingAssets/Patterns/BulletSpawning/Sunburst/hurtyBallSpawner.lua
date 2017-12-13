fireIndex = {}
fireTimes = {}
initialized = {}
anglePer = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(8.533333, 0)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		
			bullet = pattern.NewBullet()
			bullet.speed = 6 + pattern.Math().RandomRange(0, 6)
			bullet.type = "circle"
			bullet.material = "orange"
			bullet.scale = 2 + pattern.Math().RandomRange(0, 4)
			bullet.x = bullet.x + pattern.Math().RandomRange(-10, 10)
			pattern.SpawnBullet(bullet)
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
end