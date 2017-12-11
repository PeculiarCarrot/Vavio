fireIndex = {}
fireTimes = {}
initialized = {}
anglePer = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(0.58333333333, .6)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		if(fireIndex[id] % 2 == 0) then
			numBullets = 5;
		else
			numBullets = 4;
		end
		for i=0, numBullets-1, 1 do
			bullet = pattern.NewBullet()
			bullet.speed = 2
			bullet.angle = i * anglePer[id] - ((numBullets-1) * anglePer[id]) / 2
			pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	anglePer[id] = 25
end