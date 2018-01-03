
initialized = {}
shots = 40

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
end

function init(pattern, id)
	initialized[id] = true
	local x = pattern.Math().RandomRange(-pattern.StageWidth() / 2, pattern.StageWidth() / 2)
	local a = pattern.Math().RandomRange(0, 360)
		for i = 0, 360, (360/shots) do
			bullet = pattern.NewBullet()
			bullet.speed = 2
			bullet.type = "circle"
			bullet.material = "orange"
			bullet.angle = i + a
			bullet.scale = .7
			bullet.x = x
			pattern.SpawnBullet(bullet)
		end
end