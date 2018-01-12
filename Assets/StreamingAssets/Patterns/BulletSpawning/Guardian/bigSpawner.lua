fireIndex = {}
fireTimes = {}
initialized = {}
anglePer = {}
numBullets = 2
right = true
color = 0
t = 20

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(8.5333333, 0)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		right = not right
		color = color + 1

		if color > 3 then color = 0 end
		
		for i=0, numBullets-1, 1 do
			bullet = pattern.NewBullet()
			bullet.speed = 3 + pattern.Math().RandomRange(0, 2)
			--bullet.speedMultiplier = .997
			bullet.turn = pattern.Math().RandomRange(-t, t)
			bullet.type = "capsule"
			bullet.scale = 2
			if right then
				bullet.angle = 180
				bullet.x = pattern.StageWidth() * .55
			else
				bullet.angle = 0
				bullet.x = -pattern.StageWidth() * .55
			end
			bullet.material = "inverted"
			bullet.y = pattern.Math().RandomRange(-pattern.StageHeight() / 2, pattern.StageHeight() / 2)
			pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
end