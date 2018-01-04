fireIndex = {}
fireTimes = {}
initialized = {}
scale = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(4.6666666, 0)
	end
	scale[id] = scale[id] + .8 * pattern.GetRealDeltaTime()

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1

		bullet = pattern.NewBullet()
		bullet.speed = pattern.Math().RandomRange(2,6)
		bullet.speedMultiplier = 1 + pattern.Math().RandomValue() * .01
		bullet.material = "orange"
		bullet.type = "circle"
		bullet.scale = scale[id]
		bullet.angle = -90
		pattern.SpawnBullet(bullet)
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	scale[id] = .5
end