fireIndex = {}
fireTimes = {}
initialized = {}
anglePer = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(2.16666, .5)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		numBullets = pattern.Math().RandomRangeInt(10, 23)

		angle = pattern.Math().RandomRange(0, 360)
		for i = angle, angle + 360, (360 / numBullets) do
			bullet = pattern.NewBullet()
			bullet.speed = pattern.Math().RandomRange(1, 2.5)
			bullet.speedMultiplier = 1.001
			bullet.angle = i
			bullet.type = "circle"
			bullet.material = "aqua"
			pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	anglePer[id] = 25
end