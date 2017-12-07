function init(movement)
	movement.resetMoveOnUpdate = false
end

function update(movement, deltaTime)
	movement.SetMove(movement.speed)
end