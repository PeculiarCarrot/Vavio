fireIndex = {}
fireTimes = {}
initialized = {}
color = {}
numBullets = 30
t = 0

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(0.6333333, 0)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1

		local index = pattern.Math().RandomRangeInt(3, numBullets - 4)
		if color == "aqua" then color = "darkAqua" else color = "aqua" end

		for i=0, numBullets-1, 1 do
			if i ~= index then
				bullet = pattern.NewBullet()
				bullet.speed = 3
				bullet.type = "circle"
				bullet.material = color
				bullet.turn = pattern.Math().RandomRange(-t, t)
				bullet.scale = 2
				bullet.y = 0
				bullet.z = 2
				bullet.x = -pattern.StageWidth() * .5 + (i * ((pattern.StageWidth() + 1) / numBullets))
				pattern.SpawnBullet(bullet)
			end
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	color[id] = "aqua"
end