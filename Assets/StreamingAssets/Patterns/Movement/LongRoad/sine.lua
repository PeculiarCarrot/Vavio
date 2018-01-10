baseX = {}
baseY = {}
time = {}
amplitude = 1
period = 2
spd = .5
initialized = {}

function init(movement, id)
	baseX[id] = movement.GetX()
	baseY[id] = movement.GetY()
	time[id] = 0
	movement.resetMoveOnUpdate = true
	movement.ignoreAngle = true
	movement.synced = true
	initialized[id] = true
end

function update(movement, id, deltaTime)
	if(not initialized[id]) then
		init(pattern, id)
	end

	time[id] = time[id] + deltaTime

	movement.SetPos(baseX[id] + 
		(movement.Math().Cos(movement.GetAngle() * movement.Math().Deg2Rad) * spd * movement.speed * time[id]) +
		movement.Math().Cos((movement.GetAngle() + 90)* movement.Math().Deg2Rad) * amplitude * movement.Math().Sin(time[id] * period),
		baseY[id] + 
		(movement.Math().Sin(movement.GetAngle() * movement.Math().Deg2Rad) * spd * movement.speed * time[id]) +
		movement.Math().Sin((movement.GetAngle() + 90)* movement.Math().Deg2Rad) * amplitude * movement.Math().Sin(time[id] * period))
end