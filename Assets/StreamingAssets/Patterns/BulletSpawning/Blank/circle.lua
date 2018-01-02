
initialized = {}
angle = 320

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
end

function init(pattern, id)
	initialized[id] = true
	local goalX = 0;
	local goalY = 0;
	goalX = goalX - pattern.GetX()
	goalY = goalY - pattern.GetY()
	dist = pattern.StageWidth() / 2
	bullets = 40
	local a = pattern.Math().RandomRange(0, 360)

		for i = 0, angle, (angle / bullets) do
			bullet = pattern.NewBullet()
			bullet.speed = -11
			bullet.speedMultiplier = .986
			bullet.type = "circle"
			bullet.material = "aqua"
			bullet.lifetime = 1.4
			bullet.angle = i + a
			bullet.x = pattern.Math().Cos(bullet.angle * pattern.Math().Deg2Rad) * dist + goalX
			bullet.y = pattern.Math().Sin(bullet.angle * pattern.Math().Deg2Rad) * dist + goalY
			bullet.scale = .6
			bullet.destroyOnExitStage = false;
			pattern.SpawnBullet(bullet)
		end
end