fireIndex = {}
fireTimes = {}
initialized = {}
anglePer = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(4.33333, 0)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		numBullets = pattern.Math().RandomRange(4, 6)
		anglePer[id] = pattern.Math().RandomRange(15, 30)
		orange = pattern.Math().RandomValue() < .5
		for i=0, numBullets-1, 1 do
			bullet = pattern.NewBullet()
			bullet.speed = 2
			bullet.scale = .5
			bullet.type = "circle"
			if(orange) then
				bullet.material = "aqua"
				bullet.z = 1
			else
				bullet.material = "darkAqua"
			end
			--bullet.speedMultiplier = .998
			bullet.angle = i * anglePer[id] - ((numBullets-1) * anglePer[id]) / 2
			pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	anglePer[id] = 10
end