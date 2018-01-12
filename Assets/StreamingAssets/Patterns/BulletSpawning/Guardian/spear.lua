fireIndex = {}
fireTimes = {}
initialized = {}
color = {}
numBullets = 10
t = 0

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	color[id] = "aqua"

		local a = pattern.Math().RandomRange(270 - 90, 270 + 90)
		local d = -.75

		for i=0, numBullets-1, 1 do
				bullet = pattern.NewBullet()
				bullet.speed = 12
				bullet.type = "capsule"
				bullet.material = "inverted"
				bullet.turn = pattern.Math().RandomRange(-t, t)
				bullet.scale = 3
				bullet.x = pattern.Math().Cos(a * pattern.Math().Deg2Rad) * d * i
				bullet.y = pattern.Math().Sin(a * pattern.Math().Deg2Rad) * d * i + .5
				bullet.z = 2
				bullet.angle = a
				bullet.destroyOnExitStage = false
				bullet.lifetime = 3
				pattern.SpawnBullet(bullet)
		end
end