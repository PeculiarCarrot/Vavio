function init(movement)
	movement.resetMoveOnUpdate = true
	movement.ignoreAngle = true
	movement.targetType = "player"
end

function update(movement, deltaTime)
	movement.FindTarget()
	if(movement.GetX() < movement.GetTargetX()) then
		movement.SetMove(movement.GetMoveX() + movement.GetSpeed() * deltaTime)
	elseif(movement.GetX() > movement.GetTargetX()) then
		movement.SetMove(movement.GetMoveX() - movement.GetSpeed() * deltaTime)
	end	

	if(movement.GetY() < movement.GetTargetY()) then
		movement.SetMove(movement.GetMoveX(), movement.GetMoveY() + movement.GetSpeed() * deltaTime)
	elseif(movement.GetY() > movement.GetTargetY()) then
		movement.SetMove(movement.GetMoveX(), movement.GetMoveY() - movement.GetSpeed() * deltaTime)
	end
end