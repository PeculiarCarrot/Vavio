pos = {}
initialized = {}
angle = {}
radius = {}

function init(movement, id)
	movement.resetMoveOnUpdate = true
	movement.speed = movement.speed * 101.7
	initialized[id] = true
	angle[id] = 90
	pos[id] = {}
	pos[id]["x"] = movement.GetX();
	pos[id]["y"] = movement.StageMinY() + movement.StageHeight() * .8;
	radius[id] = movement.GetY() - pos[id]["y"] 
end

function update(movement, id, deltaTime)
	if(initialized[id] == nil) then
		init(movement, id)
	end

	angle[id] = angle[id] + movement.speed * deltaTime
	movement.SetRotation(angle[id])

	if movement.GetStageTime() >= 86.7 then
		scaleX = movement.GetScaleX()
		scaleY = movement.GetScaleY()
		scaleX = scaleX - .1 * deltaTime
		scaleY = scaleY - .1 * deltaTime
		radius[id] = radius[id] - .4 * deltaTime
		if radius[id] < 0 then radius[id] = 0 end
		if scaleX < 0 then scaleX = 0 end
		if scaleY < 0 then scaleY = 0 end
		movement.SetScaleX(scaleX)
		movement.SetScaleY(scaleY)
	end
	movement.SetPos(pos[id]["x"] + movement.Math().Cos(movement.Math().Deg2Rad * angle[id]) * radius[id],
		pos[id]["y"] + movement.Math().Sin(movement.Math().Deg2Rad * angle[id]) * radius[id])
end