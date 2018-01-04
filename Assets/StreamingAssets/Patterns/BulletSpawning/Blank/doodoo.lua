
initialized = {}
shots = 20

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
			bullet.speed = 5
			bullet.type = "capsule"
			bullet.material = "aqua"
			bullet.angle = i + a
			bullet.scale = 1
			bullet.turn = pattern.Math().RandomRange(-turn, turn)
			pattern.SpawnBullet(bullet)
		end
end