
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
	local turn = 10
	local x = pattern.Math().RandomRange(-pattern.StageWidth() / 2, pattern.StageWidth() / 2)

	local y = pattern.Math().RandomRange(-pattern.StageHeight() / 2, pattern.StageHeight() / 2)
	local t = pattern.Math().RandomRange(-turn, turn)

	local r = pattern.Math().RandomValue()

	if r < .25 then
		x = -pattern.StageWidth() * .45
	elseif r < .5 then
		x = pattern.StageWidth() * .45
	elseif r < .75 then
		y = -pattern.StageHeight() * .45
	else
		y = pattern.StageHeight() * .45
	end

	local c = "red"
	if pattern.Math().RandomValue() < .5 then c = "aqua" end

		for i = 0, 360, (360/shots) do
			bullet = pattern.NewBullet()
			bullet.speed = 3
			bullet.type = "circle"
			bullet.material = c
			bullet.angle = i + a
			bullet.x = x
			bullet.y = y
			bullet.scale = 2.5
			if(c == "red") then
				bullet.z = 1
			end
			bullet.turn = t
			bullet.speedMultiplier = 1
			pattern.SpawnBullet(bullet)
		end
end