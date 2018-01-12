fireIndex = {}
fireTimes = {}
initialized = {}
color = {}
numBullets = 7
t = 0

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(17.066666, 0)
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1

		for i=0, numBullets-1, 1 do

		local a = pattern.Math().RandomRange(270 - 90, 270 + 90)
				bullet = pattern.NewBullet()
				bullet.speed = 2 + pattern.Math().RandomRange(0, 1)
				bullet.type = "circle"
				bullet.material = "inverted"
				--bullet.turn = pattern.Math().RandomRange(-t, t)
				bullet.scale = .75
				bullet.z = pattern.Math().RandomRange(0, 2)
				bullet.angle = a
				bullet.lifetime = 6
				pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	color[id] = "aqua"

		
end