function init(movement, id)
	movement.SetScaleX(.05)
	movement.SetScaleY(100)
end

function update(movement, id, deltaTime)
	movement.SetScaleX(movement.GetScaleX() + .2 * deltaTime)
end