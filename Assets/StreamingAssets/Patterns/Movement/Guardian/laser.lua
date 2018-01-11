function init(movement, id)
	movement.SetScaleX(2)
	movement.SetScaleY(100)
	movement.SetScaleZ(4)
end

function update(movement, id, deltaTime)
	movement.SetScaleX(movement.GetScaleX() - movement.GetScaleX() * (5 * deltaTime))
	if(movement.GetScaleX() < .3) then
		movement.SetScaleX(movement.GetScaleX() - movement.GetScaleX() * (5 * deltaTime))
	end
	if(movement.GetScaleX() < .01) then
		movement.Die()
	end
end