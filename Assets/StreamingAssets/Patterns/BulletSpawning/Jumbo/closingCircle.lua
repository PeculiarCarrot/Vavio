
initialized = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
end

function init(pattern, id)
	initialized[id] = true
	local goalX = pattern.Math().RandomValue() * pattern.StageWidth() - pattern.StageWidth() / 2;
	local goalY = pattern.Math().RandomValue() * pattern.StageHeight() - pattern.StageHeight() / 2;
	goalX = goalX - pattern.GetX()
	goalY = goalY - pattern.GetY()
	dist = 20
	local t = 20
	local turn1 = pattern.Math().RandomRange(-t, t)

		for i = 0, 360, (360 / 15) do
			bullet = pattern.NewBullet()
			bullet.speed = 5
			bullet.speedMultiplier = .999
			bullet.type = "circle"
			bullet.material = "red"
			bullet.x = pattern.Math().Cos(i * pattern.Math().Deg2Rad) * dist + goalX
			bullet.y = pattern.Math().Sin(i * pattern.Math().Deg2Rad) * dist + goalY
			bullet.angle = i - 180
			bullet.scale = .5
			bullet.lifetime = 6
			bullet.turn = turn1
			bullet.destroyOnExitStage = false;
			pattern.SpawnBullet(bullet)
		end
end