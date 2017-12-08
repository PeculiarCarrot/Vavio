

function init(movement)
	movement.resetMoveOnUpdate = false
	movement.ignoreAngle = true
	movement.targetType = "player"
	spd = movement.Math().RandomValue() * 2 + 2
	movement.friction = .97
	spd = 3
	turnSpd = .03
end

function update(movement, deltaTime)
	movement.FindTarget()
	local speed = spd * movement.speed
	local dx = movement.GetTargetX() - movement.GetX()
	local dy = movement.GetTargetY() - movement.GetY()
	local a = movement.Math().Atan2(dy, dx);
	movement.SetRotation(movement.Math().LerpAngle(movement.GetAngle(), a * movement.Math().Rad2Deg, turnSpd))
	
	movement.SetMove(
		speed * movement.Math().Cos(movement.Math().Deg2Rad * movement.GetAngle()),
		speed * movement.Math().Sin(movement.Math().Deg2Rad * movement.GetAngle()))
end