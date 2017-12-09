function init(movement, id)
	movement.SetScaleX(2)
	movement.SetScaleY(100)
	movement.SetScaleZ(4)
end

function update(movement, id, deltaTime)
	movement.SetScaleX(movement.GetScaleX() * .94)
	if(movement.GetScaleX() < .02) then
		movement.Die()
	end
end