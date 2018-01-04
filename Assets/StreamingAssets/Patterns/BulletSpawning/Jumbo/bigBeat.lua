fireIndex = {}
fireTimes = {}
initialized = {}
anglePer = {}
orange = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(4.3333333, .9)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		numBullets = pattern.Math().RandomRangeInt(10, 30)

		orange[id] = not orange[id]

		angle = pattern.Math().RandomRange(0, 360)
		for i = angle, angle + 360, (360 / numBullets) do
			bullet = pattern.NewBullet()
			bullet.speed = pattern.Math().RandomRange(1.3, 1.6)
			--bullet.speedMultiplier = 1.001
			bullet.angle = i
			bullet.type = "circle"
			if(orange[id]) then
				bullet.material = "orange"
				bullet.z = 1
			else
				bullet.material = "aqua"
			end
			pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	anglePer[id] = 25
	orange[id] = false
end