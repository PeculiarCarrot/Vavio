
initialized = {}
shots = 50

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
end

function init(pattern, id)
	initialized[id] = true
	local a = pattern.Math().RandomRange(0, 360)
	local turn = 20
		for i = 0, 360, (360/shots) do
			bullet = pattern.NewBullet()
			bullet.speed = 3
			bullet.type = "capsule"
			bullet.material = "aqua"
			bullet.angle = i + a
			bullet.scale = 1
			bullet.speedMultiplier = 1 + pattern.Math().RandomValue() * .007
			bullet.turn = pattern.Math().RandomRange(-turn, turn)
			pattern.SpawnBullet(bullet)
		end
		a = pattern.Math().RandomRange(0, 360)
		for i = 0, 360, (360/shots) do
			bullet = pattern.NewBullet()
			bullet.speed = 2.5
			bullet.type = "capsule"
			bullet.material = "darkAqua"
			bullet.angle = i + a
			bullet.scale = 1
			bullet.speedMultiplier = 1 + pattern.Math().RandomValue() * .0085
			bullet.turn = pattern.Math().RandomRange(-turn, turn)
			pattern.SpawnBullet(bullet)
		end
		a = pattern.Math().RandomRange(0, 360)
		for i = 0, 360, (360/shots) do
			bullet = pattern.NewBullet()
			bullet.speed = 2
			bullet.type = "capsule"
			bullet.material = "aqua"
			bullet.angle = i + a
			bullet.scale = 1
			bullet.speedMultiplier = 1 + pattern.Math().RandomValue() * .01
			bullet.turn = pattern.Math().RandomRange(-turn, turn)
			pattern.SpawnBullet(bullet)
		end
end