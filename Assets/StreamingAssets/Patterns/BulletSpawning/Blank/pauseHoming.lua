
initialized = {}
shots = 30
pad = .48

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
end

function init(pattern, id)
	initialized[id] = true
	local a = pattern.Math().RandomRange(0, 360)
	local turn = 20
		for i = 0, shots, 1 do
			bullet = pattern.NewBullet()
			bullet.speed = .5 + pattern.Math().RandomRange(0, 2)
			bullet.type = "capsule"
			bullet.material = "red"
			bullet.movement = "Blank/pauseHoming"
			local dir = pattern.Math().RandomValue() < .5
			local dir2 = pattern.Math().RandomValue() < .5
			if dir then
				bullet.y = pattern.Math().RandomRange(-pattern.StageHeight() / 2, pattern.StageHeight() / 2)
				if dir2 then bullet.x = pattern.StageWidth() * pad else bullet.x = -pattern.StageWidth() * pad end
			else
				bullet.x = pattern.Math().RandomRange(-pattern.StageWidth() / 2, pattern.StageWidth() / 2)
				if dir2 then bullet.y = pattern.StageHeight() * pad else bullet.y = -pattern.StageHeight() * pad end
			end
			bullet.scale = 1
			bullet.lifetime = 24
			pattern.SpawnBullet(bullet)
		end
end