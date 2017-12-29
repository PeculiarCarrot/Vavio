pos = {}
initialized = {}
angle = {}
radius = 1

function init(movement, id)
	movement.resetMoveOnUpdate = true
	movement.speed = movement.speed * 101.7
	initialized[id] = true
	angle[id] = 90
	pos[id] = {}
	pos[id]["x"] = movement.GetX();
	pos[id]["y"] = movement.StageMinY() + movement.StageHeight() * .8;
	radius = movement.GetY() - pos[id]["y"] 
end

function update(movement, id, deltaTime)
	if(initialized[id] == nil) then
		init(movement, id)
	end
	angle[id] = angle[id] + movement.speed * deltaTime
	movement.SetPos(pos[id]["x"] + movement.Math().Cos(movement.Math().Deg2Rad * angle[id]) * radius,
		pos[id]["y"] + movement.Math().Sin(movement.Math().Deg2Rad * angle[id]) * radius)
end