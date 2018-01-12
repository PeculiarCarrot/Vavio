fireIndex = {}
fireTimes = {}
initialized = {}
color = {}
numBullets = 20
t = 0

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(4.26666666, 0)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1

		local x = pattern.Math().RandomRange(-pattern.StageWidth() * .5, pattern.StageWidth() * .5)
		local s = 8 + pattern.Math().RandomRange(0, 3)

		for i=0, numBullets-1, 1 do
				bullet = pattern.NewBullet()
				bullet.speed = s
				bullet.type = "capsule"
				bullet.material = "inverted"
				bullet.turn = pattern.Math().RandomRange(-t, t)
				bullet.scale = 2
				bullet.x = x
				bullet.z = 2
				bullet.y = .5 * i
				bullet.angle = -90
				bullet.destroyOnExitStage = false
				bullet.lifetime = 5
				pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	color[id] = "aqua"
end