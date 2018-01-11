fireIndex = {}
fireTimes = {}
initialized = {}
anglePer = {}
numBullets = 2
right = true
color = 0

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(2.133333, 0)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		right = not right
		color = color + 1

		if color > 3 then color = 0 end
		
		for i=0, numBullets-1, 1 do
			bullet = pattern.NewBullet()
			bullet.speed = 4 + pattern.Math().RandomRange(0, 2)
			--bullet.speedMultiplier = .997
			bullet.type = "capsule"
			bullet.scale = 1
			if right then
				bullet.angle = 180
				bullet.x = pattern.StageWidth() * .55
			else
				bullet.angle = 0
				bullet.x = -pattern.StageWidth() * .55
			end
			if(color == 0) then
				bullet.material = "orange"
			elseif color == 1 then
				bullet.material = "darkRed"
			elseif color == 2 then
				bullet.material = "purple"
			elseif color == 3 then
				bullet.material = "aqua"
			end
			bullet.y = pattern.Math().RandomRange(-pattern.StageHeight() / 2, pattern.StageHeight() / 2)
			pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
end