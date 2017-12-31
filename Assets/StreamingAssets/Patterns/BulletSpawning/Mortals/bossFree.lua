fireIndex = {}
fireTimes = {}
initialized = {}
anglePer = {}
color = 0
colorTimer = 0
colorTime = .75
angle = 0
angle2 = 0
angle3 = 180

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(12, 0)
	end

		colorTimer = colorTimer + deltaTime
		if colorTimer > colorTime then
			colorTimer = colorTimer - colorTime
			color = color + 1
		end

		if color > 3 then color = 0 end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		numBullets = 5

		angle = angle + deltaTime * 400
		angle2 = angle2 - deltaTime * 400
		angle3 = angle3 + deltaTime * 600
		for i = angle, angle + 360, (360 / numBullets) do
			bullet = pattern.NewBullet()
			bullet.speed = 3
			bullet.speedMultiplier = .999
			bullet.angle = i
			bullet.type = "circle"
			if(color == 0) then
				bullet.material = "orange"
			elseif color == 1 then
				bullet.material = "darkRed"
			elseif color == 2 then
				bullet.material = "purple"
			elseif color == 3 then
				bullet.material = "aqua"
			end
			bullet.z = 2
			bullet.scale = .5
			pattern.SpawnBullet(bullet)
		end
		for i = angle2, angle2 + 360, (360 / numBullets) do
			bullet = pattern.NewBullet()
			bullet.speed = 3
			bullet.speedMultiplier = .999
			bullet.angle = i
			bullet.type = "circle"
			if(color == 0) then
				bullet.material = "orange"
			elseif color == 1 then
				bullet.material = "darkRed"
			elseif color == 2 then
				bullet.material = "purple"
			elseif color == 3 then
				bullet.material = "aqua"
			end
			bullet.z = 2
			bullet.scale = .5
			pattern.SpawnBullet(bullet)
		end
		for i = angle3, angle3 + 360, (360 / (numBullets)) do
			bullet = pattern.NewBullet()
			bullet.speed = 2.5
			bullet.speedMultiplier = 1.01
			bullet.turn = -10
			bullet.angle = i
			bullet.type = "capsule"
			bullet.material = "white"
			bullet.z = 1.5
			pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	anglePer[id] = 25
end