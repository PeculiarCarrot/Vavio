initialized = {}
fireIndex = {}
numSets = {}
fireTimes = {}
a = 0

function init(pattern, id)
	fireIndex[id] = 0
	numSets[id] = 8

	fireTimes[id] = pattern.GetFireTimes(2.133333, 0)
	a = pattern.StageHeight() + pattern.StageWidth() * .25
	initialized[id] = true
end

function update(pattern, id, deltaTime)
	if(not initialized[id]) then
		init(pattern, id)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		for i = 0, numSets[id] - 1, 1 do
			bullet = pattern.NewBullet()
			bullet.speed = 5 + pattern.Math().RandomRange(0, 2)
			bullet.destroyOnExitStage = false
			bullet.lifetime = 20
			bullet.type = "circle"
			bullet.scale = .5
			bullet.angle = pattern.Math().RandomRange(90, 270)
			bullet.x = pattern.Math().Cos((bullet.angle + 90) * pattern.Math().Deg2Rad) * a
			bullet.y = pattern.Math().Sin((bullet.angle + 90) * pattern.Math().Deg2Rad) * a
			bullet.z = 3
			bullet.material = "white"
			pattern.SpawnBullet(bullet)
		end
	end
end