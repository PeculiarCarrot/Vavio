baseX = {}
baseY = {}
amplitude = 3
period = .5
spd = 3
initialized = {}

function init(movement, id)
	baseX[id] = movement.GetX()
	baseY[id] = movement.GetY()
	movement.resetMoveOnUpdate = true
	movement.ignoreAngle = true
	movement.synced = true
	initialized[id] = true
end

function update(movement, id, deltaTime)
	if(not initialized[id]) then
		init(pattern, id)
	end

	--movement.Print(movement.GetAngle())

	if(movement.GetAngle() == 270) then
		movement.SetPos(movement.GetX() + spd * deltaTime,
			baseY[id] + amplitude * movement.Math().Sin(movement.GetX() * period))
	else
		movement.SetPos(movement.GetX() - spd * deltaTime,
			baseY[id] + amplitude * movement.Math().Sin(movement.GetX() * period))
	end
end