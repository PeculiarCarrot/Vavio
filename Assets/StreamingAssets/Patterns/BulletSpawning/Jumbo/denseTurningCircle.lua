fireIndex = {}
fireTimes = {}
spin = {}
angle = {}
initialized = {}
shots = 40

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(2.166666, .5)
	end

	angle[id] = pattern.Math().RandomRange(0, 360)
	turn = pattern.Math().RandomRange(-15, 15)

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1

		for i = angle[id], angle[id] + 360, (360/shots) do
			bullet = pattern.NewBullet()
			bullet.speed = 3
			bullet.type = "capsule"
			bullet.material = "darkRed"
			bullet.angle = i
			bullet.turn = turn
			bullet.scale = .75
			bullet.speedMultiplier = 1
			pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	angle[id] = 0
	spin[id] = 100
end