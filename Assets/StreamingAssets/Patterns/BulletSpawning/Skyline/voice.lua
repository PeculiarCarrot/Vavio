
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
	local turn = 0
	local x = pattern.Math().RandomRange(-pattern.StageWidth() / 2, pattern.StageWidth() / 2)
	local y = pattern.StageHeight() * .4
		for i = 0, 360, (360/shots) do
			bullet = pattern.NewBullet()
			bullet.speed = 4
			bullet.type = "capsule"
			bullet.material = "aqua"
			bullet.angle = i + a
			bullet.x = x
			bullet.y = y
			bullet.scale = 1
			bullet.speedMultiplier = 1
			bullet.turn = pattern.Math().RandomRange(-turn, turn)
			pattern.SpawnBullet(bullet)
		end
end