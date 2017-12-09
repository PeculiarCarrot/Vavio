spd = {}
turnSpd = {}
initialized = {}

function init(movement, id)
	movement.resetMoveOnUpdate = false
	movement.ignoreAngle = false
	movement.synced = true
	movement.targetType = "player"
	spd[id] = movement.Math().RandomValue() * 2 + 2
	movement.friction = .9
	turnSpd[id] = movement.Math().RandomValue() * 1.4 + .3
	initialized[id] = true
end

function update(movement, id, deltaTime)
	if(not initialized[id]) then
		init(pattern, id)
	end

	movement.FindTarget()
	local speed = spd[id] * movement.speed
	local dx = movement.GetTargetX() - movement.GetX()
	local dy = movement.GetTargetY() - movement.GetY()
	local a = movement.Math().Atan2(dy, dx);
	movement.SetRotation(movement.Math().LerpAngle(movement.GetAngle(), a * movement.Math().Rad2Deg, deltaTime * turnSpd[id]))
	
	movement.SetMove(movement.GetMoveX() + spd[id], 0)
end