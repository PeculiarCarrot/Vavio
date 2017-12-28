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
		fireTimes[id] = pattern.GetFireTimes(2.133333, 0)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		
		for i=0, numBullets-1, 1 do
			bullet = pattern.NewBullet()
			bullet.speed = 2 + pattern.Math().RandomRange(0, 1)
			bullet.type = "capsule"
			bullet.material = "orange"
			bullet.scale = 1
			bullet.x = bullet.x + pattern.Math().RandomRange(-10, 10)
			bullet.y = 2
			pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
end