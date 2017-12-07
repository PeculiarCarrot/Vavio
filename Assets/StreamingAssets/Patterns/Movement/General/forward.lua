function init(movement)
	movement.resetPosOnUpdate = true
end

function update(movement, deltaTime)
	movement.SetPos(movement.GetSpeed() * deltaTime)
end