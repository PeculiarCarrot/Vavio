fireIndex = {}
fireTimes = {}
spin = {}
angle = {}
initialized = {}
num = 10

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(4.3333333, .4)
	end

	angle[id] = angle[id] + spin[id] * deltaTime

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1

		for i = angle[id], angle[id] + 360, (360 / num) do
			bullet = pattern.NewBullet()
			bullet.speed = 6
			bullet.speedMultiplier = .995
			bullet.material = "orange"
			bullet.angle = i
			pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	angle[id] = 0
	spin[id] = 30
end