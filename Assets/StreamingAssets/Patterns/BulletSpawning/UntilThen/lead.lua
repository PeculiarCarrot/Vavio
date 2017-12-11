
initialized = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
end

function init(pattern, id)
	initialized[id] = true
	local angle = pattern.Math().RandomRange(-45, 45)
		for i = 0, 180, (360/30) do
			bullet = pattern.NewBullet()
			bullet.speed = 3
			bullet.type = "circle"
			bullet.material = "orange"
			bullet.angle = i + angle + 180
			bullet.scale = .5
			pattern.SpawnBullet(bullet)
		end
end