function init(movement, id)
	movement.resetMoveOnUpdate = false
end

function update(movement, id, deltaTime)
	movement.SetMove(movement.speed)
end