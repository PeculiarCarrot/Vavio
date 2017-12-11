
initialized = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
end

function init(pattern, id)
	initialized[id] = true
	local angle = pattern.Math().RandomRange(0, 360)
		for i = 0, 360, (360/70) do
			bullet = pattern.NewBullet()
			bullet.speed = 3
			--bullet.speedMultiplier = 1.006
			bullet.type = "circle"
			bullet.material = "orange"
			bullet.angle = i + angle
			bullet.scale = .5
			pattern.SpawnBullet(bullet)
		end
end