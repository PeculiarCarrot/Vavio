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
		if(fireIndex[id] % 2 == 0) then
			numBullets = 10;
		else
			numBullets = 9;
		end
		for i=0, numBullets-1, 1 do
			bullet = pattern.NewBullet()
			bullet.speed = 2
			bullet.type = "circle"
			bullet.material = "red"
			bullet.scale = .5
			bullet.angle = i * anglePer[id] - ((numBullets-1) * anglePer[id]) / 2
			pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	anglePer[id] = 16
end