pos = {}
fireIndex = {}
fireTimes = {}
initialized = {}
angle = {}
radius = 1
color = 0

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	pos[id] = {}
	pos[id]["x"] = pattern.GetX();
	pos[id]["y"] = pattern.StageMinY() + pattern.StageHeight() * .8;
end

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(movement, id)
	end
	if pattern.GetStageTime() < 85 then
		bullet = pattern.NewBullet()
		bullet.speed = 1
		bullet.lifetime = 2
		bullet.angle = -90
		bullet.z = 2
		bullet.type = "capsule"
		bullet.material = "white"
		pattern.SpawnBullet(bullet)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(1.5, 42.7)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		numBullets = 11

		turn = pattern.Math().RandomRange(-30, 30)
		color = color + 1

		if color > 3 then color = 0 end

		angle = pattern.Math().RandomRange(0, 360)
		if pattern.GetStageTime() < 85 then
			for i = angle, angle + 360, (360 / numBullets) do
				bullet = pattern.NewBullet()
				bullet.speed = 3
				bullet.speedMultiplier = .999
				bullet.angle = i
				bullet.type = "capsule"
				bullet.turn = turn
				bullet.material = "white"
				bullet.scale = 1
				bullet.z = 1
				bullet.lifetime = 10
				pattern.SpawnBullet(bullet)
			end
		end
	end
end