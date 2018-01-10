fireIndex = {}
fireTimes = {}
initialized = {}
anglePer = {}
numBullets = 5
t = 0

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(5.066666, .8)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		
		for i=0, numBullets-1, 1 do
			bullet = pattern.NewBullet()
			bullet.speed = 2 + pattern.Math().RandomRange(0, 2)
			bullet.type = "capsule"
			bullet.material = "purple"
			bullet.turn = pattern.Math().RandomRange(-t, t)
			bullet.scale = 1
			bullet.y = 1
			bullet.x = bullet.x + pattern.Math().RandomRange(-pattern.StageWidth() / 2, pattern.StageWidth() / 2)
			pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
end