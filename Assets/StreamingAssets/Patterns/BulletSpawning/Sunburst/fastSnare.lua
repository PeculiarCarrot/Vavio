fireIndex = {}
fireTimes = {}
initialized = {}
anglePer = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(2.133333, .4)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		numBullets = pattern.Math().RandomRange(3, 10)
		for i=0, numBullets-1, 1 do
			bullet = pattern.NewBullet()
			bullet.speed = 2 + pattern.Math().RandomRange(0, 2)
			bullet.type = "circle"
			bullet.material = "aqua"
			bullet.scale = .5
			bullet.angle = i * anglePer[id] - ((numBullets-1) * anglePer[id]) / 2
			pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	anglePer[id] = 15
end