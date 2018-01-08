fireIndex = {}
fireTimes = {}
initialized = {}
anglePer = {}
numBullets = 8

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(2.1333333, 0)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		
		for i=0, numBullets-1, 1 do
			bullet = pattern.NewBullet()
			bullet.speed = 3 + pattern.Math().RandomRange(0, 2)
			bullet.type = "circle"
			bullet.material = "orange"
			bullet.scale = .7
			bullet.z = 1
			bullet.x = bullet.x + pattern.Math().RandomRange(-pattern.StageWidth() * .25, pattern.StageWidth() * .25)
			pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
end