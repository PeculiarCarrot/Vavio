fireIndex = {}
fireTimes = {}
initialized = {}
anglePer = {}
numBullets = 1
color = 0

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(6, 0)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		color = color + 1

		if color > 4 then color = 0 end
		
		for i=0, numBullets-1, 1 do
			bullet = pattern.NewBullet()
			bullet.speed = 1 + pattern.Math().RandomRange(0, 1)
			bullet.type = "circle"
			if(color == 0) then
				bullet.material = "orange"
			elseif color == 1 then
				bullet.material = "darkRed"
			elseif color == 2 then
				bullet.material = "purple"
			elseif color == 3 then
				bullet.material = "aqua"
			elseif color == 4 then
				bullet.material = "white"
			end
			bullet.scale = 1
			bullet.movement = "Mortals/sine"
			bullet.angle = pattern.GetAngle() - 90
			bullet.x = bullet.x + pattern.Math().RandomRange(-8, 8)
			pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
end