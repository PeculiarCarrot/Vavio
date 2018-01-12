fireIndex = {}
fireTimes = {}
initialized = {}
shots = 60

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = pattern.GetFireTimes(2.133333, 0)
	end
	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1

		local a = pattern.Math().RandomRange(0, 360)
		local turn = 30
		local t = pattern.Math().RandomRange(-turn, turn)

		for i = 0, 360, (360/shots) do
			if i == 360 then break end
			bullet = pattern.NewBullet()
			bullet.speed = 4
			bullet.type = "capsule"
			bullet.material = "inverted"
			bullet.angle = i + a
			bullet.scale = 1.5
			bullet.turn = t
			bullet.speedMultiplier = 1
			pattern.SpawnBullet(bullet)
		end
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	
end