function init(movement)
	movement.resetMoveOnUpdate = true
end

function update(movement, deltaTime)
	movement.SetMove(movement.speed * deltaTime)
end