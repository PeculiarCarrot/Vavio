turnSpd = {}
initialized = {}
spd = {}

function init(movement, id)
	movement.resetMoveOnUpdate = false
	movement.ignoreAngle = true
	movement.targetType = "player"
	spd[id] = movement.Math().RandomValue() * 2 + 2
	movement.friction = .97
	spd[id] = 2
	turnSpd[id] = .03
end

function update(movement, id, deltaTime)
	movement.FindTarget()
	local speed = spd[id] * movement.speed
	local dx = movement.GetTargetX() - movement.GetX()
	local dy = movement.GetTargetY() - movement.GetY()
	local a = movement.Math().Atan2(dy, dx);
	movement.SetRotation(movement.Math().LerpAngle(movement.GetAngle(), a * movement.Math().Rad2Deg, turnSpd[id]))
	
	movement.SetMove(
		speed * movement.Math().Cos(movement.Math().Deg2Rad * movement.GetAngle()),
		speed * movement.Math().Sin(movement.Math().Deg2Rad * movement.GetAngle()))

	turnSpd[id] = turnSpd[id] * .99
end