pos = {}
initialized = {}
left = {}
moving = {}
fireIndex = {}
fireTimes = {}


function init(movement, id)
	initialized[id] = true
	fireIndex[id] = 0
	pos[id] = 0
	left[id] = true
	moving[id] = false
	movement.resetMoveOnUpdate = true
	movement.speed = 10
end

function update(movement, id, deltaTime)
	if(initialized[id] == nil) then
		init(movement, id)
	end
	if(fireTimes[id] == nil) then
		fireTimes[id] = movement.GetFireTimes(0.462, .9, 60)
	end

	if(movement.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		if(moving[id]) then
			if(left[id]) then
				pos[id] = pos[id] - 1
			else
				pos[id] = pos[id] + 1
			end
		end
		moving[id] = not moving[id]
		if(pos[id] == 1 and not left[id]) or (pos[id] == -1 and left[id]) then
			left[id] = not left[id]
		end
	end
	if(moving[id] and left[id]) then
		movement.SetMove(0, movement.speed)
	elseif(moving[id] and not left[id]) then
		movement.SetMove(0, -movement.speed)
	end
end