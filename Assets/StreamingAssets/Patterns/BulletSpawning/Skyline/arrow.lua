
initialized = {}
size = 8
mul = .5
shots = 20

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
end

function init(pattern, id)
	initialized[id] = true

	--for k = 0, 1, 1 do
		local xx = pattern.Math().RandomRange(-pattern.StageWidth() / 2, pattern.StageWidth() / 2)
		local c = pattern.Math().RandomRangeInt(0, 2)

		local color = "red"
		if c == 0 then color = "red"
		else color = "aqua" end

		spd = pattern.Math().RandomRange(2, 3)
		
		local a = pattern.Math().RandomRange(0, 360)
	local turn = 0
	local x = pattern.Math().RandomRange(-pattern.StageWidth() / 2, pattern.StageWidth() / 2)
	local y = 0
		for i = 0, 360, (360/shots) do
			bullet = pattern.NewBullet()
		bullet.speed = spd
	--bullet.speedMultiplier = .8
		bullet.type = "circle"
		bullet.material = color
		bullet.scale = pattern.Math().RandomRange(.7, 1.5)
			bullet.angle = i + a
			bullet.x = x
			bullet.y = y
			bullet.z = c + 1
			bullet.speedMultiplier = 1
			bullet.turn = pattern.Math().RandomRange(-turn, turn)
			pattern.SpawnBullet(bullet)
		end
end