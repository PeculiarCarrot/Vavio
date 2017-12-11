fireIndex = {}
fireTimes = {}
initialized = {}
scale = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(9.333333, 0)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		scale[id] = scale[id] + .02
		fireIndex[id] = fireIndex[id] + 1

		bullet = pattern.NewBullet()
		bullet.speed = 3
		bullet.material = "orange"
		bullet.type = "cube"
		bullet.scale = scale[id]
		bullet.angle = -90
		pattern.SpawnBullet(bullet)
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	scale[id] = .2
end