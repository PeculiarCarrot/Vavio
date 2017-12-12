initialized = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
end

function init(pattern, id)
	initialized[id] = true
	local angle = pattern.Math().RandomRange(0, 360)
	for i = angle, angle + 360, (360/20) do
			bullet = pattern.NewBullet()
			bullet.speed = 2
			bullet.type = "circle"
			bullet.material = "orange"
			bullet.angle = i
			bullet.scale = .5
			bullet.speedMultiplier = 1
			pattern.SpawnBullet(bullet)
		end
end