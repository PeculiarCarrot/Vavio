
initialized = {}
shots = 50

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
end

function init(pattern, id)
	initialized[id] = true

	for i = 0, 1 do
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

		local c = "orange"

			for i = 0, 360, (360/shots) do
				bullet = pattern.NewBullet()
				bullet.speed = 5
				bullet.type = "capsule"
				bullet.material = c
				bullet.angle = i + a
				bullet.x = x
				bullet.y = y
				bullet.scale = 1
				bullet.turn = t
				bullet.speedMultiplier = 1
				pattern.SpawnBullet(bullet)
			end
	end
end