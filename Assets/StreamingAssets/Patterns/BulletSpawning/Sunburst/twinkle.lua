fireIndex = {}
fireTimes = {}
initialized = {}
anglePer = {}
numBullets = 5

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(8.533333, 0)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		
		for i=0, numBullets-1, 1 do
			bullet = pattern.NewBullet()
			bullet.speed = 3 + pattern.Math().RandomRange(0, 2)
			bullet.type = "circle"
			bullet.material = "lightAqua"
			bullet.scale = .3
			bullet.x = bullet.x + pattern.Math().RandomRange(-10, 10)
			pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
end